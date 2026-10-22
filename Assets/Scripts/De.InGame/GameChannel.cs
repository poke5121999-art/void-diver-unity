using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using NemoLib.Log;
using NemoLib.Popup;
using NemoLib.Scene;
using Netcode.Transports;
using Steamworks;
using UniRx;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using System.Threading.Tasks;
using VContainer;

public class GameChannel : NativeBase, INetworkChannel
{
	private const string SEND_TO_CLIENT_PACKET_KEY = "SendToClientPacket";

	private const string SEND_TO_HOST_PACKET_KEY = "SendToHostPacket";

	private const int JoinFailedCode = 3001;

	private const int JoinRequestMaxAttempts = 5;

	private const float ConnectionTimeoutSeconds = 20f;

	[Inject]
	private NetworkManager _networkManager;

	[Inject]
	private RoomContext _roomContext;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private EnvironmentSettings _settings;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private IRoomMatchmaker _roomMatchmaker;

	[Inject]
	private SceneManager _sceneManager;

	[Inject]
	private AppManager _appManager;

	private readonly MessageBroker _messageBroker = new MessageBroker();

	private readonly Dictionary<Type, Action<BaseMessage>> _handlers = new Dictionary<Type, Action<BaseMessage>>();

	private TaskCompletionSource<int> _joinLobbyTcs;

	private GamePlayerData _pendingPlayerData;

	private CompositeDisposable _disposable;

	private bool _joinResponseReceived;

	private bool _clientConnected;

	private int _requestSequence;

	private readonly Dictionary<int, UniTaskCompletionSource<BaseMessage>> _requests = new Dictionary<int, UniTaskCompletionSource<BaseMessage>>();

	private NetworkTransport Transport => _networkManager.NetworkConfig.NetworkTransport;

	private ulong HostId => NetworkManager.ServerClientId;

	private ulong LocalClientId => _networkManager.LocalClientId;

	public bool IsHost => _networkManager.IsHost;

	public Subject<Unit> OnJoined { get; private set; } = new Subject<Unit>();

	public Subject<Unit> OnStopped { get; private set; } = new Subject<Unit>();

	public Subject<Unit> OnHostStopped { get; private set; } = new Subject<Unit>();

	public ReactiveProperty<ERoomConnectState> RxConnectState { get; private set; } = new ReactiveProperty<ERoomConnectState>(ERoomConnectState.None);

	private bool IsNetworkInactive
	{
		get
		{
			if (_networkManager == null)
			{
				return true;
			}
			if (_networkManager.IsListening)
			{
				return false;
			}
			if (_networkManager.ShutdownInProgress)
			{
				return false;
			}
			if (_networkManager.IsServer)
			{
				return false;
			}
			return !_networkManager.IsClient;
		}
	}

	public GameChannel()
	{
		_networkManager.OnTransportFailure += OnTransportFailure;
		_networkManager.OnClientConnectedCallback += OnClientConnected;
		_networkManager.OnClientDisconnectCallback += OnClientDisconnected;
		_networkManager.OnConnectionEvent += OnConnectionEvent;
		_networkManager.OnClientStopped += OnClientStopped;
		_networkManager.OnServerStopped += OnServerStopped;
		RegisterHandlers();
		SteamNetworkingSocketsTransport.DiagnosticContextProvider = BuildNetworkDiagnosticContext;
	}

	private string BuildNetworkDiagnosticContext()
	{
		if (_gameContext == null)
		{
			return "scene=? campaign=?";
		}
		string arg = (_gameContext.IsInLounge ? "Lounge" : (_gameContext.IsInStage ? "Stage" : _gameContext.CurrentScene.ToString()));
		return string.Format("scene={0} campaign={1}", arg, _gameContext.CampaignId);
	}

	public void SetUp(string address, ushort port)
	{
		UnityTransport unityTransport = _networkManager.NetworkConfig.NetworkTransport as UnityTransport;
		if (unityTransport == null)
		{
			_logger.Error("unityTransport not supported");
		}
		else
		{
			unityTransport.SetConnectionData(address, port);
		}
	}

	public void SetUp(ulong steamId)
	{
		SteamNetworkingSocketsTransport steamNetworkingSocketsTransport = _networkManager.NetworkConfig.NetworkTransport as SteamNetworkingSocketsTransport;
		if (steamNetworkingSocketsTransport == null)
		{
			_logger.Error("SteamNetworkingSocketsTransport not supported");
		}
		else
		{
			steamNetworkingSocketsTransport.ConnectToSteamID = steamId;
		}
	}

	private void RegisterHandler()
	{
		_networkManager.CustomMessagingManager?.RegisterNamedMessageHandler("SendToClientPacket", OnMessageReceived);
	}

	public async UniTask<int> TryJoinGame(GamePlayerData playerData)
	{
		using (new BlockScope(calling: true, background: false))
		{
			_disposable?.Dispose();
			_disposable = new CompositeDisposable();
			Receive<ResJoinGame>().Subscribe(OnJoinGame).AddTo(_disposable);
			Receive<NtfPlayerJoined>().Subscribe(OnPlayerJoined).AddTo(_disposable);
			Receive<NtfPlayerLeft>().Subscribe(OnPlayerLeft).AddTo(_disposable);
			Receive<NtfUpdateGamePlayer>().Subscribe(OnUpdateGamePlayer).AddTo(_disposable);
			Receive<NtfUpdateRoomState>().Subscribe(OnUpdateRoomState).AddTo(_disposable);
			Receive<NtfRtt>().Subscribe(OnRtt).AddTo(_disposable);
			if (!_networkManager.IsHost)
			{
				_roomContext.Init(0, isSingleMode: false, isPrivate: false);
				_pendingPlayerData = playerData;
				ConnectionData connectionData = new ConnectionData();
				connectionData.Version = _appManager.Version;
				_networkManager.NetworkConfig.ConnectionData = SerializeHelper.Serialize(connectionData);
				_networkManager.NetworkConfig.EnableSceneManagement = true;
				_joinLobbyTcs = new TaskCompletionSource<int>();
				_joinResponseReceived = false;
				_clientConnected = false;
				_networkManager.StartClient();
				_networkManager.InitConfig();
				RegisterHandler();
				MonitorConnectionTimeoutAsync().Forget();
				int result = await _joinLobbyTcs.Task;
				_joinLobbyTcs = null;
				return result;
			}
			RegisterHandler();
			ReqJoinGame reqJoinGame = new ReqJoinGame();
			reqJoinGame.PlayerData = playerData;
			SendToHost(reqJoinGame);
			RxConnectState.Value = ERoomConnectState.None;
			return 0;
		}
	}

	public void LeftRoom()
	{
		_networkManager.Shutdown();
	}

	public async UniTask ShutdownAsync()
	{
		if (IsNetworkInactive)
		{
			return;
		}
		UniTaskCompletionSource tcs = new UniTaskCompletionSource();
		IDisposable disposable = (_networkManager.IsHost ? OnHostStopped.Take(1).Subscribe(delegate
		{
			tcs.TrySetResult();
		}) : OnStopped.Take(1).Subscribe(delegate
		{
			tcs.TrySetResult();
		}));
		_networkManager.Shutdown(discardMessageQueue: true);
		try
		{
			await tcs.Task.Timeout(TimeSpan.FromSeconds(5.0));
			await UniTask.WaitUntil(() => IsNetworkInactive);
		}
		catch (TimeoutException)
		{
			_logger.Warning("Shutdown timeout");
		}
	}

	private async UniTask<bool> WaitNetworkInactiveAsync(float timeoutSeconds = 5f)
	{
		if (IsNetworkInactive)
		{
			return true;
		}
		try
		{
			await UniTask.WaitUntil(() => IsNetworkInactive).Timeout(TimeSpan.FromSeconds(timeoutSeconds));
		}
		catch (TimeoutException)
		{
			return false;
		}
		return true;
	}

	public async UniTask RecreateTransportAsync()
	{
		if (_networkManager == null)
		{
			return;
		}
		if (!(_networkManager.NetworkConfig.NetworkTransport is SteamNetworkingSocketsTransport))
		{
			return;
		}
		if (!(await WaitNetworkInactiveAsync()))
		{
			_logger.Error("Transport recreation aborted: network failed to shut down");
			return;
		}
		if (_networkManager == null)
		{
			return;
		}
		await UniTask.Delay(TimeSpan.FromMilliseconds(250.0));
		if (_networkManager == null)
		{
			return;
		}
		UnityEngine.Object.Destroy(_networkManager.NetworkConfig.NetworkTransport);
		await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
		SteamNetworkingSocketsTransport steamNetworkingSocketsTransport = _networkManager.gameObject.AddComponent<SteamNetworkingSocketsTransport>();
		steamNetworkingSocketsTransport.Initialize(_networkManager);
		_networkManager.NetworkConfig.NetworkTransport = steamNetworkingSocketsTransport;
	}

	public void SendToHost(BaseMessage baseMessage)
	{
		Packet packet = new Packet(baseMessage);
		FastBufferWriter writer = new FastBufferWriter(packet.PacketSize, Unity.Collections.Allocator.Temp);
		writer.WriteValueSafe(packet);
		NetworkDelivery delivery = ((_networkManager.CustomMessagingManager == null) ? NetworkDelivery.ReliableSequenced : packet.GetDelivery(_networkManager.MaximumTransmissionUnitSize));
		_networkManager.CustomMessagingManager.SendNamedMessage("SendToHostPacket", 0uL, writer, delivery);
	}

	public void SendToClient(ulong clientId, BaseMessage baseMessage)
	{
		Packet packet = new Packet(baseMessage);
		FastBufferWriter writer = new FastBufferWriter(packet.PacketSize, Unity.Collections.Allocator.Temp);
		writer.WriteValueSafe(packet);
		NetworkDelivery delivery = ((_networkManager.CustomMessagingManager == null) ? NetworkDelivery.ReliableSequenced : packet.GetDelivery(_networkManager.MaximumTransmissionUnitSize));
		_networkManager.CustomMessagingManager.SendNamedMessage("SendToClientPacket", clientId, writer, delivery);
	}

	public async UniTask<T> RequestAsync<T>(BaseMessage baseMessage) where T : BaseMessage
	{
		// TODO(decomp): generic method has no body in the binary (address 0x0); reconstructed from the
		// <RequestAsync>d__57<T> fields (blockScope, sequence, tcs, message; awaits UniTask<BaseMessage> then a
		// yield). Block-scope arguments, timeout handling and the exact yield timing are unknown.
		using (new BlockScope())
		{
			int sequence = ++_requestSequence;
			baseMessage.Sequence = sequence;
			UniTaskCompletionSource<BaseMessage> tcs = new UniTaskCompletionSource<BaseMessage>();
			_requests[sequence] = tcs;
			SendToHost(baseMessage);
			T message = (await tcs.Task) as T;
			await UniTask.Yield();
			return message;
		}
	}

	private void OnMessageReceived(ulong clientId, FastBufferReader reader)
	{
		reader.ReadValueSafe(out Packet value);
		if (value.Message == null)
		{
			_logger.Error(string.Format("Dropped malformed packet from client {0}: {1}", clientId, value.DropReason ?? "deserialization returned null"));
			return;
		}
		BaseMessage message = value.Message;
		message.SenderId = clientId;
		if (!_handlers.TryGetValue(message.GetType(), out var handler))
		{
			_logger.Warning(string.Format("No handler found for message type: {0}", message.GetType()));
			return;
		}
		handler?.Invoke(message);
		if (message.Sequence >= 1 && _requests.TryGetValue(message.Sequence, out var tcs))
		{
			tcs.TrySetResult(message);
			_requests.Remove(message.Sequence);
		}
	}

	private void LogReceivedMessage(BaseMessage message)
	{
		_logger.Debug(string.Format("{0}\n{1}", message.GetType(), Newtonsoft.Json.JsonConvert.SerializeObject(message)));
	}

	private void OnJoinGame(ResJoinGame message)
	{
		if (_networkManager.IsHost || _joinResponseReceived)
		{
			return;
		}
		_joinResponseReceived = true;
		LogReceivedMessage(message);
		message.PlayerDatas.ForEach(delegate(GamePlayerData data)
		{
			_gameContext.Players.Add(data.ClientId, data.ToGamePlayer());
			_gameContext.PlayerRtt.Add(data.ClientId, 0uL);
		});
		_gameContext.RxRoomDifficulty.Value = message.RoomDifficulty;
		_gameContext.RxCampaignId.Value = message.CampaignId;
		_gameContext.MapSize = message.MapSize;
		_gameContext.SectorIds = message.SectorIds;
		SendToHost(new ReqJoinCompleted());
		_joinLobbyTcs?.TrySetResult(0);
		OnJoined.OnNext(Unit.Default);
	}

	private void OnPlayerJoined(NtfPlayerJoined message)
	{
		LogReceivedMessage(message);
		_gameContext.Players.Add(message.PlayerData.ClientId, message.PlayerData.ToGamePlayer());
		_gameContext.PlayerRtt.Add(message.PlayerData.ClientId, 0uL);
	}

	private void OnPlayerLeft(NtfPlayerLeft message)
	{
		LogReceivedMessage(message);
		_gameContext.Players.Remove(message.ExitedClientId);
		_gameContext.PlayerRtt.Remove(message.ExitedClientId);
	}

	private void OnUpdateGamePlayer(NtfUpdateGamePlayer message)
	{
		if (!_networkManager.IsHost && _gameContext.Players.TryGetValue(message.ChangeData.ClientId, out var value))
		{
			value.ApplyChanges(message.ChangeData);
		}
	}

	private void OnRtt(NtfRtt message)
	{
		ClientRtt[] clientRtts = message.ClientRtts;
		foreach (ClientRtt clientRtt in clientRtts)
		{
			if (_gameContext.PlayerRtt.ContainsKey(clientRtt.ClientId))
			{
				_gameContext.PlayerRtt[clientRtt.ClientId] = clientRtt.Rtt;
			}
		}
	}

	private void OnUpdateRoomState(NtfUpdateRoomState message)
	{
		LogReceivedMessage(message);
		_roomContext.RxRoomState.Value = message.State;
	}

	public IObservable<T> Receive<T>()
	{
		return _messageBroker.Receive<T>();
	}

	public async UniTask<int> ConnectAndJoinBySteamLobbyAsync(ulong lobbyId, GamePlayerData playerData)
	{
		if (_roomMatchmaker.IsAlreadyJoined)
		{
			_roomMatchmaker.LeaveLobby();
		}
		RxConnectState.Value = ERoomConnectState.ConnectingToLobby;
		CSteamID steamLobbyId = new CSteamID(lobbyId);
		int num = ((!steamLobbyId.IsValid()) ? JoinFailedCode : (await _roomMatchmaker.JoinLobby(lobbyId)));
		if (num != 0)
		{
			_logger.Warning(string.Format("Failed to join lobby: {0}, resultCode: {1}", lobbyId, num));
			return num;
		}
		MatchingLobby matchingLobby = JsonUtility.FromJson<MatchingLobby>(SteamMatchmaking.GetLobbyData(steamLobbyId, "MatchingLobby"));
		if (matchingLobby == null || matchingLobby.HostId == 0)
		{
			_logger.Warning("Invalid MatchingLobby data");
			return JoinFailedCode;
		}
		RxConnectState.Value = ERoomConnectState.ConnectingToHost;
		await _sceneManager.LoadSceneAsync("Scene/LoadingScene.unity", UnityEngine.SceneManagement.LoadSceneMode.Single);
		await ShutdownAsync();
		await RecreateTransportAsync();
		SetUp(matchingLobby.HostId);
		int num2 = await TryJoinGame(playerData);
		if (num2 == 0)
		{
			RxConnectState.Value = ERoomConnectState.None;
			return 0;
		}
		RxConnectState.Value = ERoomConnectState.Failed;
		_roomMatchmaker.LeaveLobby();
		return num2;
	}

	public async UniTask<int> ConnectAndJoinByAddressAsync(string address, ushort port, GamePlayerData playerData)
	{
		RxConnectState.Value = ERoomConnectState.ConnectingToHost;
		await _sceneManager.LoadSceneAsync("Scene/LoadingScene.unity", UnityEngine.SceneManagement.LoadSceneMode.Single);
		await ShutdownAsync();
		SetUp(address, port);
		int num = await TryJoinGame(playerData);
		if (num == 0)
		{
			RxConnectState.Value = ERoomConnectState.None;
			return 0;
		}
		RxConnectState.Value = ERoomConnectState.Failed;
		return num;
	}

	private void RegisterHandlers()
	{
		_handlers.Add(typeof(ResBagSlotToStash), msg => _messageBroker.Publish(msg as ResBagSlotToStash));
		_handlers.Add(typeof(ResBuyInterior), msg => _messageBroker.Publish(msg as ResBuyInterior));
		_handlers.Add(typeof(ResCraftingGoods), msg => _messageBroker.Publish(msg as ResCraftingGoods));
		_handlers.Add(typeof(ResDealArtifact), msg => _messageBroker.Publish(msg as ResDealArtifact));
		_handlers.Add(typeof(ResEquipLootingSlot), msg => _messageBroker.Publish(msg as ResEquipLootingSlot));
		_handlers.Add(typeof(ResEquipStashSlot), msg => _messageBroker.Publish(msg as ResEquipStashSlot));
		_handlers.Add(typeof(ResExchangeCoin), msg => _messageBroker.Publish(msg as ResExchangeCoin));
		_handlers.Add(typeof(ResInventorySlotToStashBagSlot), msg => _messageBroker.Publish(msg as ResInventorySlotToStashBagSlot));
		_handlers.Add(typeof(ResInventorySlotToStashSlot), msg => _messageBroker.Publish(msg as ResInventorySlotToStashSlot));
		_handlers.Add(typeof(ResJoinGame), msg => _messageBroker.Publish(msg as ResJoinGame));
		_handlers.Add(typeof(ResLevelUp), msg => _messageBroker.Publish(msg as ResLevelUp));
		_handlers.Add(typeof(ResMoveAllInventoryToStash), msg => _messageBroker.Publish(msg as ResMoveAllInventoryToStash));
		_handlers.Add(typeof(ResParadoxLock), msg => _messageBroker.Publish(msg as ResParadoxLock));
		_handlers.Add(typeof(ResParadoxPurify), msg => _messageBroker.Publish(msg as ResParadoxPurify));
		_handlers.Add(typeof(ResParadoxUnlock), msg => _messageBroker.Publish(msg as ResParadoxUnlock));
		_handlers.Add(typeof(ResPlayCutscene), msg => _messageBroker.Publish(msg as ResPlayCutscene));
		_handlers.Add(typeof(ResRegisterItemUsed), msg => _messageBroker.Publish(msg as ResRegisterItemUsed));
		_handlers.Add(typeof(ResRepairAllGoods), msg => _messageBroker.Publish(msg as ResRepairAllGoods));
		_handlers.Add(typeof(ResRepairGoods), msg => _messageBroker.Publish(msg as ResRepairGoods));
		_handlers.Add(typeof(ResRerollLostItems), msg => _messageBroker.Publish(msg as ResRerollLostItems));
		_handlers.Add(typeof(ResRest), msg => _messageBroker.Publish(msg as ResRest));
		_handlers.Add(typeof(ResSafeSlotToStashSlot), msg => _messageBroker.Publish(msg as ResSafeSlotToStashSlot));
		_handlers.Add(typeof(ResScrapGoods), msg => _messageBroker.Publish(msg as ResScrapGoods));
		_handlers.Add(typeof(ResSellArtifact), msg => _messageBroker.Publish(msg as ResSellArtifact));
		_handlers.Add(typeof(ResSetPet), msg => _messageBroker.Publish(msg as ResSetPet));
		_handlers.Add(typeof(ResShopBuyGoods), msg => _messageBroker.Publish(msg as ResShopBuyGoods));
		_handlers.Add(typeof(ResShopSellGoods), msg => _messageBroker.Publish(msg as ResShopSellGoods));
		_handlers.Add(typeof(ResSyncInventory), msg => _messageBroker.Publish(msg as ResSyncInventory));
		_handlers.Add(typeof(ResSyncSafeInventory), msg => _messageBroker.Publish(msg as ResSyncSafeInventory));
		_handlers.Add(typeof(ResSyncShop), msg => _messageBroker.Publish(msg as ResSyncShop));
		_handlers.Add(typeof(ResSyncUnitModel), msg => _messageBroker.Publish(msg as ResSyncUnitModel));
		_handlers.Add(typeof(ResTakeLostItem), msg => _messageBroker.Publish(msg as ResTakeLostItem));
		_handlers.Add(typeof(ResTycoonCancel), msg => _messageBroker.Publish(msg as ResTycoonCancel));
		_handlers.Add(typeof(ResTycoonFinish), msg => _messageBroker.Publish(msg as ResTycoonFinish));
		_handlers.Add(typeof(ResTycoonHire), msg => _messageBroker.Publish(msg as ResTycoonHire));
		_handlers.Add(typeof(ResTycoonPurify), msg => _messageBroker.Publish(msg as ResTycoonPurify));
		_handlers.Add(typeof(ResTycoonRegisterArtifact), msg => _messageBroker.Publish(msg as ResTycoonRegisterArtifact));
		_handlers.Add(typeof(ResUnequipToStashSlot), msg => _messageBroker.Publish(msg as ResUnequipToStashSlot));
		_handlers.Add(typeof(ResUnlockTalent), msg => _messageBroker.Publish(msg as ResUnlockTalent));
		_handlers.Add(typeof(NtfAllClientsIntroFinished), msg => _messageBroker.Publish(msg as NtfAllClientsIntroFinished));
		_handlers.Add(typeof(NtfAllPlayerReadyToEnterStage), msg => _messageBroker.Publish(msg as NtfAllPlayerReadyToEnterStage));
		_handlers.Add(typeof(NtfAssignedZones), msg => _messageBroker.Publish(msg as NtfAssignedZones));
		_handlers.Add(typeof(NtfCameraShake), msg => _messageBroker.Publish(msg as NtfCameraShake));
		_handlers.Add(typeof(NtfCanceledEnterStage), msg => _messageBroker.Publish(msg as NtfCanceledEnterStage));
		_handlers.Add(typeof(NtfChangeActiveSkill), msg => _messageBroker.Publish(msg as NtfChangeActiveSkill));
		_handlers.Add(typeof(NtfChangeCharacterSkin), msg => _messageBroker.Publish(msg as NtfChangeCharacterSkin));
		_handlers.Add(typeof(NtfChangeSkill), msg => _messageBroker.Publish(msg as NtfChangeSkill));
		_handlers.Add(typeof(NtfCloseCampaign), msg => _messageBroker.Publish(msg as NtfCloseCampaign));
		_handlers.Add(typeof(NtfCollisionTriggerEnter), msg => _messageBroker.Publish(msg as NtfCollisionTriggerEnter));
		_handlers.Add(typeof(NtfDisplayFx), msg => _messageBroker.Publish(msg as NtfDisplayFx));
		_handlers.Add(typeof(NtfDisplayNoticeToast), msg => _messageBroker.Publish(msg as NtfDisplayNoticeToast));
		_handlers.Add(typeof(NtfEmoji), msg => _messageBroker.Publish(msg as NtfEmoji));
		_handlers.Add(typeof(NtfEndInteraction), msg => _messageBroker.Publish(msg as NtfEndInteraction));
		_handlers.Add(typeof(NtfEndMission), msg => _messageBroker.Publish(msg as NtfEndMission));
		_handlers.Add(typeof(NtfEquipmentBroken), msg => _messageBroker.Publish(msg as NtfEquipmentBroken));
		_handlers.Add(typeof(NtfGameReady), msg => _messageBroker.Publish(msg as NtfGameReady));
		_handlers.Add(typeof(NtfHitCollision), msg => _messageBroker.Publish(msg as NtfHitCollision));
		_handlers.Add(typeof(NtfHpDamage), msg => _messageBroker.Publish(msg as NtfHpDamage));
		_handlers.Add(typeof(NtfHpRecovery), msg => _messageBroker.Publish(msg as NtfHpRecovery));
		_handlers.Add(typeof(NtfItemUsed), msg => _messageBroker.Publish(msg as NtfItemUsed));
		_handlers.Add(typeof(NtfLightFuelDamage), msg => _messageBroker.Publish(msg as NtfLightFuelDamage));
		_handlers.Add(typeof(NtfLightFuelRecovery), msg => _messageBroker.Publish(msg as NtfLightFuelRecovery));
		_handlers.Add(typeof(NtfMonsterKill), msg => _messageBroker.Publish(msg as NtfMonsterKill));
		_handlers.Add(typeof(NtfMonsterTargetChanged), msg => _messageBroker.Publish(msg as NtfMonsterTargetChanged));
		_handlers.Add(typeof(NtfOnFreezeFireDamage), msg => _messageBroker.Publish(msg as NtfOnFreezeFireDamage));
		_handlers.Add(typeof(NtfOnHitInvincibility), msg => _messageBroker.Publish(msg as NtfOnHitInvincibility));
		_handlers.Add(typeof(NtfOnImmuneStatusEffect), msg => _messageBroker.Publish(msg as NtfOnImmuneStatusEffect));
		_handlers.Add(typeof(NtfPing), msg => _messageBroker.Publish(msg as NtfPing));
		_handlers.Add(typeof(NtfPlayAnimation), msg => _messageBroker.Publish(msg as NtfPlayAnimation));
		_handlers.Add(typeof(NtfPlayerJoined), msg => _messageBroker.Publish(msg as NtfPlayerJoined));
		_handlers.Add(typeof(NtfPlayerKilledToast), msg => _messageBroker.Publish(msg as NtfPlayerKilledToast));
		_handlers.Add(typeof(NtfPlayerLeft), msg => _messageBroker.Publish(msg as NtfPlayerLeft));
		_handlers.Add(typeof(NtfPlayersReady), msg => _messageBroker.Publish(msg as NtfPlayersReady));
		_handlers.Add(typeof(NtfPlayNpcAnimation), msg => _messageBroker.Publish(msg as NtfPlayNpcAnimation));
		_handlers.Add(typeof(NtfPlaySfx), msg => _messageBroker.Publish(msg as NtfPlaySfx));
		_handlers.Add(typeof(NtfRemoveFx), msg => _messageBroker.Publish(msg as NtfRemoveFx));
		_handlers.Add(typeof(NtfRtt), msg => _messageBroker.Publish(msg as NtfRtt));
		_handlers.Add(typeof(NtfSelfReviveText), msg => _messageBroker.Publish(msg as NtfSelfReviveText));
		_handlers.Add(typeof(NtfShowConsumeGauge), msg => _messageBroker.Publish(msg as NtfShowConsumeGauge));
		_handlers.Add(typeof(NtfSkillIndicator), msg => _messageBroker.Publish(msg as NtfSkillIndicator));
		_handlers.Add(typeof(NtfSpawnSequentialTimeline), msg => _messageBroker.Publish(msg as NtfSpawnSequentialTimeline));
		_handlers.Add(typeof(NtfStageResultLocalInfo), msg => _messageBroker.Publish(msg as NtfStageResultLocalInfo));
		_handlers.Add(typeof(NtfStageResultPlayerInfo), msg => _messageBroker.Publish(msg as NtfStageResultPlayerInfo));
		_handlers.Add(typeof(NtfStartCampaign), msg => _messageBroker.Publish(msg as NtfStartCampaign));
		_handlers.Add(typeof(NtfStartInteraction), msg => _messageBroker.Publish(msg as NtfStartInteraction));
		_handlers.Add(typeof(NtfStartInteractionHolding), msg => _messageBroker.Publish(msg as NtfStartInteractionHolding));
		_handlers.Add(typeof(NtfStartMission), msg => _messageBroker.Publish(msg as NtfStartMission));
		_handlers.Add(typeof(NtfStressDamage), msg => _messageBroker.Publish(msg as NtfStressDamage));
		_handlers.Add(typeof(NtfStressRecovery), msg => _messageBroker.Publish(msg as NtfStressRecovery));
		_handlers.Add(typeof(NtfTriggerLuaEvent), msg => _messageBroker.Publish(msg as NtfTriggerLuaEvent));
		_handlers.Add(typeof(NtfUnitBubbleText), msg => _messageBroker.Publish(msg as NtfUnitBubbleText));
		_handlers.Add(typeof(NtfUnitMonologueText), msg => _messageBroker.Publish(msg as NtfUnitMonologueText));
		_handlers.Add(typeof(NtfUpdateAnimation), msg => _messageBroker.Publish(msg as NtfUpdateAnimation));
		_handlers.Add(typeof(NtfUpdateArea), msg => _messageBroker.Publish(msg as NtfUpdateArea));
		_handlers.Add(typeof(NtfUpdateAreaDecoration), msg => _messageBroker.Publish(msg as NtfUpdateAreaDecoration));
		_handlers.Add(typeof(NtfUpdateCampaignState), msg => _messageBroker.Publish(msg as NtfUpdateCampaignState));
		_handlers.Add(typeof(NtfUpdateGamePlayer), msg => _messageBroker.Publish(msg as NtfUpdateGamePlayer));
		_handlers.Add(typeof(NtfUpdateInventory), msg => _messageBroker.Publish(msg as NtfUpdateInventory));
		_handlers.Add(typeof(NtfUpdateMapInfo), msg => _messageBroker.Publish(msg as NtfUpdateMapInfo));
		_handlers.Add(typeof(NtfUpdateNpcLevel), msg => _messageBroker.Publish(msg as NtfUpdateNpcLevel));
		_handlers.Add(typeof(NtfUpdateRoomState), msg => _messageBroker.Publish(msg as NtfUpdateRoomState));
		_handlers.Add(typeof(NtfUpdateSafeInventory), msg => _messageBroker.Publish(msg as NtfUpdateSafeInventory));
		_handlers.Add(typeof(NtfUpdateShop), msg => _messageBroker.Publish(msg as NtfUpdateShop));
		_handlers.Add(typeof(NtfUpdateUnitModel), msg => _messageBroker.Publish(msg as NtfUpdateUnitModel));
		_handlers.Add(typeof(NtfUserChat), msg => _messageBroker.Publish(msg as NtfUserChat));
		_handlers.Add(typeof(NtfUserTextKeyChat), msg => _messageBroker.Publish(msg as NtfUserTextKeyChat));
		_handlers.Add(typeof(NtfVoteCampaign), msg => _messageBroker.Publish(msg as NtfVoteCampaign));
		_handlers.Add(typeof(NtfVoteGiveUpCampaign), msg => _messageBroker.Publish(msg as NtfVoteGiveUpCampaign));
		_handlers.Add(typeof(NtfWaveFinished), msg => _messageBroker.Publish(msg as NtfWaveFinished));
		_handlers.Add(typeof(NtfWaveStarted), msg => _messageBroker.Publish(msg as NtfWaveStarted));
	}

	private void OnTransportFailure()
	{
		_logger.Debug(_networkManager.DisconnectReason ?? "", Color.green);
	}

	private void OnClientConnected(ulong clientId)
	{
		if (!_networkManager.IsHost)
		{
			_clientConnected = true;
			SendJoinRequestWithRetryAsync().Forget();
		}
	}

	private async UniTask MonitorConnectionTimeoutAsync()
	{
		TimeSpan step = TimeSpan.FromMilliseconds(200.0);
		float elapsed = 0f;
		while (elapsed < ConnectionTimeoutSeconds)
		{
			if (_clientConnected || IsJoinSettled())
			{
				return;
			}
			await UniTask.Delay(step, ignoreTimeScale: true);
			elapsed += (float)step.TotalSeconds;
		}
		if (!_clientConnected && !IsJoinSettled())
		{
			_logger.Warning(string.Format("Client connection timed out after {0}s (OnClientConnected never fired); aborting join", ConnectionTimeoutSeconds));
			_joinLobbyTcs?.TrySetResult(JoinFailedCode);
		}
	}

	private async UniTask SendJoinRequestWithRetryAsync()
	{
		GamePlayerData playerData = _pendingPlayerData;
		_pendingPlayerData = null;
		if (playerData == null)
		{
			return;
		}
		int attempt = 1;
		while (!IsJoinSettled())
		{
			_logger.Debug(string.Format("ReqJoinGame sent (attempt {0}/{1})", attempt, JoinRequestMaxAttempts), new Color(0f, 1f, 1f, 1f));
			ReqJoinGame reqJoinGame = new ReqJoinGame();
			reqJoinGame.PlayerData = playerData;
			if (await RequestAsync<ResJoinGame>(reqJoinGame) != null)
			{
				return;
			}
			attempt++;
			if (attempt > JoinRequestMaxAttempts)
			{
				if (!IsJoinSettled())
				{
					_logger.Warning(string.Format("ReqJoinGame got no ResJoinGame after {0} attempts; aborting join", JoinRequestMaxAttempts));
					_joinLobbyTcs?.TrySetResult(JoinFailedCode);
				}
				return;
			}
		}
	}

	private bool IsJoinSettled()
	{
		if (_joinResponseReceived || _joinLobbyTcs == null)
		{
			return true;
		}
		return _joinLobbyTcs.Task.IsCompleted;
	}

	private void OnClientDisconnected(ulong clientId)
	{
		_logger.Debug(string.Format("ClientId : {0}, {1}", clientId, _networkManager.DisconnectReason), Color.green);
		if (!_networkManager.IsHost && _joinLobbyTcs != null)
		{
			if (!int.TryParse(_networkManager.DisconnectReason, out var result))
			{
				result = JoinFailedCode;
			}
			_joinLobbyTcs.TrySetResult(result);
		}
	}

	private void OnConnectionEvent(NetworkManager networkManager, ConnectionEventData eventData)
	{
		string arg = (eventData.PeerClientIds.Any() ? string.Join(",", eventData.PeerClientIds.ToArray()) : "");
		_logger.Debug(string.Format("{0}, Id:{1}, Peers:[{2}]", eventData.EventType, eventData.ClientId, arg), new Color(0f, 1f, 1f, 1f));
	}

	private void OnClientStopped(bool isServer)
	{
		_logger.Debug(string.Format("IsServer: {0}, {1}", isServer, _networkManager.DisconnectReason), Color.green);
		_networkManager.NetworkConfig.EnableSceneManagement = false;
		_roomContext.Clear();
		OnStopped.OnNext(Unit.Default);
	}

	private void OnServerStopped(bool isServer)
	{
		_logger.Debug(string.Format("OnServerStopped : IsHost {0}", _networkManager.IsHost), Color.green);
		OnHostStopped.OnNext(Unit.Default);
	}
}
