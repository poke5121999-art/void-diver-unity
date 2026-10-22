using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.Log;
using Newtonsoft.Json;
using UniRx;
using Unity.Collections;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;
using VContainer;

public class GameHost : NativeBase, INetworkHost
{
	private const string SEND_TO_CLIENT_PACKET_KEY = "SendToClientPacket";

	private const string SEND_TO_HOST_PACKET_KEY = "SendToHostPacket";

	[Inject]
	private NetworkManager _networkManager;

	[Inject]
	private RoomContext _roomContext;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private IRoomMatchmaker _roomMatchmaker;

	[Inject]
	private AppManager _appManager;

	private readonly Dictionary<Type, Action<BaseMessage>> _handlers = new Dictionary<Type, Action<BaseMessage>>();

	private readonly MessageBroker _messageBroker = new MessageBroker();

	private CompositeDisposable _disposable;

	private NetworkTransport Transport => _networkManager.NetworkConfig.NetworkTransport;

	private ulong HostId => 0uL;

	private ulong LocalClientId => _networkManager.LocalClient.ClientId;

	public bool IsAllPlayersReady => _roomContext.ClientStates.All((KeyValuePair<ulong, EClientState> _) => _.Value == EClientState.Playable);

	public Subject<Unit> OnStarted { get; } = new Subject<Unit>();

	public Subject<Unit> OnStopped { get; } = new Subject<Unit>();

	public Subject<(string address, ushort port)> OnConnectionDataSet { get; } = new Subject<(string address, ushort port)>();

	public HashSet<ulong> JoinedClientIds { get; } = new HashSet<ulong>();

	public HashSet<ulong> SnapshotSentClientIds { get; } = new HashSet<ulong>();

	public Subject<ulong> OnJoinCompleted { get; } = new Subject<ulong>();

	public Subject<ulong> OnClientCaughtUp { get; } = new Subject<ulong>();

	public GameHost()
	{
		_networkManager.OnServerStarted += OnServerStarted;
		_networkManager.OnServerStopped += OnServerStopped;
		_networkManager.OnTransportFailure += OnTransportFailure;
		_networkManager.OnClientConnectedCallback += OnClientConnected;
		_networkManager.OnClientDisconnectCallback += OnClientDisconnect;
		_networkManager.ConnectionApprovalCallback += OnConnectionApproval;
		RegisterHandlers();
	}

	public ulong GetCurrentRtt(ulong clientId)
	{
		if (clientId == 0)
		{
			return 0uL;
		}
		ulong transportIdFromClientId = _networkManager.GetTransportIdFromClientId(clientId);
		if (transportIdFromClientId == ulong.MaxValue)
		{
			return 0uL;
		}
		return _networkManager.NetworkConfig.NetworkTransport.GetCurrentRtt(transportIdFromClientId);
	}

	public void SetUp(string address, ushort port)
	{
		UnityTransport unityTransport = _networkManager.NetworkConfig.NetworkTransport as UnityTransport;
		if (unityTransport == null)
		{
			_logger.Error("unityTransport not supported");
			return;
		}
		unityTransport.SetConnectionData(address, port);
		OnConnectionDataSet.OnNext((address, port));
	}

	public void CreateRoom(ERoomDifficulty difficulty)
	{
		_disposable?.Dispose();
		SetState(ERoomState.Creating);
		_roomContext.Init(1, isSingleMode: true);
		_gameContext.RxRoomDifficulty.Value = difficulty;
		_disposable = new CompositeDisposable();
		Receive<ReqJoinGame>().Subscribe(OnJoinGame).AddTo(_disposable);
		Receive<ReqJoinCompleted>().Subscribe(OnReqJoinCompleted).AddTo(_disposable);
		Receive<ReqUpdateClientState>().Subscribe(OnUpdateClientState).AddTo(_disposable);
		Observable.Interval(TimeSpan.FromSeconds(1.0), Scheduler.MainThread).Subscribe(delegate
		{
			BroadcastRtt();
		}).AddTo(_disposable);
		_networkManager.NetworkConfig.ConnectionData = SerializeHelper.Serialize(new ConnectionData());
		_networkManager.NetworkConfig.EnableSceneManagement = false;
		_networkManager.StartHost();
		_networkManager.InitConfig();
		_networkManager.CustomMessagingManager.RegisterNamedMessageHandler("SendToHostPacket", OnMessageReceived);
	}

	public async UniTask<ECreateLobbyResult> PublishRoomAsync(string roomTitle, int playerCountMax, string playerName, int playerLevel, ERoomPublishType publishType, ERoomDifficulty difficulty)
	{
		_gameContext.RxRoomDifficulty.Value = difficulty;
		_roomContext.Init(playerCountMax, isSingleMode: false, publishType == ERoomPublishType.Private);
		SetState(ERoomState.Ready);
		return await _roomMatchmaker.CreateLobby(new MatchingLobbyOptions
		{
			Title = roomTitle,
			PlayerCountMax = _roomContext.PlayerCountMax,
			PublishType = publishType,
			RoomDifficulty = difficulty,
			PlayerName = playerName,
			PlayerLevel = playerLevel
		});
	}

	public void CloseRoom()
	{
		_networkManager.Shutdown();
	}

	public void LoadScene(string sceneName)
	{
		foreach (ulong item in _roomContext.ClientStates.Keys.ToList())
		{
			_roomContext.ClientStates[item] = EClientState.SceneLoading;
		}
		_networkManager.NetworkConfig.EnableSceneManagement = true;
		_networkManager.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
	}

	public void SetState(ERoomState state)
	{
		SendToAllExceptHost(new NtfUpdateRoomState
		{
			State = state
		});
		_roomContext.RxRoomState.Value = state;
	}

	private Packet CreatePacket(BaseMessage baseMessage, out FastBufferWriter writer, out NetworkDelivery delivery)
	{
		Packet packet = new Packet(baseMessage);
		writer = new FastBufferWriter(packet.PacketSize, Allocator.Temp);
		writer.WriteNetworkSerializable(in packet);
		delivery = ((_networkManager.CustomMessagingManager == null) ? NetworkDelivery.ReliableSequenced : packet.GetDelivery(_networkManager.MaximumTransmissionUnitSize));
		return packet;
	}

	private void SendToTargets(BaseMessage baseMessage, IReadOnlyList<ulong> targets)
	{
		CreatePacket(baseMessage, out var writer, out var delivery);
		if (targets.Count >= 1)
		{
			_networkManager.CustomMessagingManager?.SendNamedMessage("SendToClientPacket", targets, writer, delivery);
		}
	}

	public void SendToClient(ulong clientId, BaseMessage baseMessage)
	{
		CreatePacket(baseMessage, out var writer, out var delivery);
		_networkManager.CustomMessagingManager?.SendNamedMessage("SendToClientPacket", clientId, writer, delivery);
	}

	public void SendToAll(BaseMessage baseMessage, HashSet<ulong> excludeIds = null)
	{
		CreatePacket(baseMessage, out var writer, out var delivery);
		IReadOnlyList<ulong> readOnlyList = ((excludeIds == null) ? _networkManager.ConnectedClientsIds : ((IReadOnlyList<ulong>)_networkManager.ConnectedClientsIds.Where((ulong _) => !excludeIds.Contains(_)).ToList()));
		if (readOnlyList.Count >= 1)
		{
			_networkManager.CustomMessagingManager?.SendNamedMessage("SendToClientPacket", readOnlyList, writer, delivery);
		}
	}

	public void SendToSnapshotApplied(BaseMessage baseMessage, HashSet<ulong> excludeIds = null)
	{
		SendToGated(baseMessage, SnapshotSentClientIds, excludeIds);
	}

	public void SendToJoined(BaseMessage baseMessage, HashSet<ulong> excludeIds = null)
	{
		SendToGated(baseMessage, JoinedClientIds, excludeIds);
	}

	private void SendToGated(BaseMessage baseMessage, HashSet<ulong> gate, HashSet<ulong> excludeIds)
	{
		CreatePacket(baseMessage, out var writer, out var delivery);
		IEnumerable<ulong> source = ((excludeIds == null) ? _networkManager.ConnectedClientsIds : ((IReadOnlyList<ulong>)_networkManager.ConnectedClientsIds.Where((ulong _) => !excludeIds.Contains(_)).ToList()));
		List<ulong> list = source.Where(gate.Contains).ToList();
		if (list.Count >= 1)
		{
			_networkManager.CustomMessagingManager?.SendNamedMessage("SendToClientPacket", list, writer, delivery);
		}
	}

	public void SendToAllExceptHost(BaseMessage baseMessage, HashSet<ulong> excludeIds = null)
	{
		CreatePacket(baseMessage, out var writer, out var delivery);
		IEnumerable<ulong> source = ((excludeIds == null) ? _networkManager.ConnectedClientsIds : ((IReadOnlyList<ulong>)_networkManager.ConnectedClientsIds.Where((ulong _) => !excludeIds.Contains(_)).ToList()));
		List<ulong> list = source.Where((ulong _) => _ != HostId).ToList();
		if (list.Count >= 1)
		{
			_networkManager.CustomMessagingManager?.SendNamedMessage("SendToClientPacket", list, writer, delivery);
		}
	}

	private void OnMessageReceived(ulong clientID, FastBufferReader reader)
	{
		reader.ReadNetworkSerializable(out Packet value);
		BaseMessage message = value.Message;
		if (message == null)
		{
			_logger.Error(string.Format("Dropped malformed packet from client {0}: {1}", clientID, value.DropReason ?? "deserialization returned null"));
		}
		else if (value.HostOnly)
		{
			message.SenderId = clientID;
			if (!_handlers.TryGetValue(message.GetType(), out var value2))
			{
				_logger.Warning(string.Format("No handler found for message type: {0}", message.GetType()));
			}
			else
			{
				value2(message);
			}
		}
	}

	private int TryJoinPlayer(ulong clientId, GamePlayerData playerData, int sequence)
	{
		if (clientId != 0 && _gameContext.Players.ContainsKey(clientId))
		{
			ResendJoinResult(clientId, sequence);
			return 0;
		}
		if (_roomContext.RoomState == ERoomState.Playing)
		{
			return NetworkResultCode.GAME_ALREADY_STARTED;
		}
		if (clientId != 0 && _roomContext.RoomState != ERoomState.Ready)
		{
			return NetworkResultCode.GAME_ALREADY_STARTED;
		}
		if (_gameContext.Players.Count >= _roomContext.PlayerCountMax)
		{
			return NetworkResultCode.PLAYER_FULL;
		}
		playerData.ClientId = clientId;
		HashSet<int> usedIndices = _gameContext.Players.Values.Select((GamePlayer p) => p.PlayerIndex).ToHashSet();
		playerData.PlayerIndex = Enumerable.Range(0, _roomContext.PlayerCountMax).First((int i) => !usedIndices.Contains(i));
		BaseMessage baseMessage;
		if (clientId == 0)
		{
			baseMessage = new ResJoinGame
			{
				Sequence = sequence,
				RoomDifficulty = _gameContext.RoomDifficulty
			};
		}
		else
		{
			FlushPlayerChanges(clientId);
			List<GamePlayerData> list = _gameContext.Players.Values.Select((GamePlayer p) => p.ToGamePlayerData()).ToList();
			list.Add(playerData);
			baseMessage = new ResJoinGame
			{
				Sequence = sequence,
				PlayerDatas = list.ToArray(),
				RoomDifficulty = _gameContext.RoomDifficulty,
				CampaignId = _gameContext.CampaignId,
				MapSize = _gameContext.MapSize,
				SectorIds = _gameContext.SectorIds
			};
		}
		SendToClient(clientId, baseMessage);
		SnapshotSentClientIds.Add(clientId);
		HashSet<ulong> hashSet = new HashSet<ulong>();
		hashSet.Add(0uL);
		hashSet.Add(clientId);
		SendToGated(new NtfPlayerJoined
		{
			PlayerData = playerData
		}, SnapshotSentClientIds, hashSet);
		_gameContext.Players[clientId] = playerData.ToGamePlayer();
		_gameContext.PlayerRtt[clientId] = 0uL;
		_roomContext.ClientStates.Add(clientId, EClientState.SceneLoading);
		return 0;
	}

	private void ResendJoinResult(ulong clientId, int sequence)
	{
		GamePlayerData[] playerDatas = _gameContext.Players.Values.Select((GamePlayer p) => p.ToGamePlayerData()).ToArray();
		ResJoinGame resJoinGame = new ResJoinGame();
		resJoinGame.Sequence = sequence;
		resJoinGame.PlayerDatas = playerDatas;
		resJoinGame.RoomDifficulty = _gameContext.RoomDifficulty;
		resJoinGame.CampaignId = _gameContext.CampaignId;
		resJoinGame.MapSize = _gameContext.MapSize;
		resJoinGame.SectorIds = _gameContext.SectorIds;
		SendToClient(clientId, resJoinGame);
	}

	private void FlushPlayerChanges(ulong excludeClientId)
	{
		HashSet<ulong> hashSet = new HashSet<ulong>();
		hashSet.Add(excludeClientId);
		foreach (GamePlayer value in _gameContext.Players.Values)
		{
			if (value.HasChange)
			{
				GamePlayerData changeData = value.Flush();
				SendToGated(new NtfUpdateGamePlayer
				{
					ChangeData = changeData
				}, SnapshotSentClientIds, hashSet);
			}
		}
	}

	private bool TryLeavePlayer(ulong clientId)
	{
		if (!_gameContext.Players.Remove(clientId))
		{
			return false;
		}
		_gameContext.PlayerRtt.Remove(clientId);
		SnapshotSentClientIds.Remove(clientId);
		JoinedClientIds.Remove(clientId);
		SendToAllExceptHost(new NtfPlayerLeft
		{
			ExitedClientId = clientId
		});
		return true;
	}

	public void SendNoticeToastToAll(string textKey, long fontSize = 24L, HashSet<ulong> excludeIds = null)
	{
		SendToAll(new NtfDisplayNoticeToast
		{
			TextKey = textKey,
			FontSize = fontSize
		}, excludeIds);
	}

	public void SendNoticeToast(string textKey, ulong targetClientId, long fontSize = 24L)
	{
		SendToClient(targetClientId, new NtfDisplayNoticeToast
		{
			TextKey = textKey,
			FontSize = fontSize
		});
	}

	public void SendPlayerKilledToastToAll(NtfPlayerKilledToast notice, HashSet<ulong> excludeIds = null)
	{
		SendToAll(notice, excludeIds);
	}

	private void BroadcastRtt()
	{
		ClientRtt[] clientRtts = JoinedClientIds.Where((ulong id) => id != HostId).Select(delegate(ulong id)
		{
			ClientRtt clientRtt = new ClientRtt();
			clientRtt.ClientId = id;
			clientRtt.Rtt = GetCurrentRtt(id);
			return clientRtt;
		}).ToArray();
		if (clientRtts.Length != 0)
		{
			SendToAllExceptHost(new NtfRtt
			{
				ClientRtts = clientRtts
			});
		}
	}

	private void LogReceivedMessage(BaseMessage message)
	{
		_logger.Debug(string.Format("{0}\n{1}", message.GetType(), JsonConvert.SerializeObject(message)));
	}

	private void OnJoinGame(ReqJoinGame message)
	{
		LogReceivedMessage(message);
		int num = TryJoinPlayer(message.SenderId, message.PlayerData, message.Sequence);
		if (num != 0)
		{
			_networkManager.DisconnectClient(message.SenderId, string.Format("{0}", num));
		}
	}

	private void OnReqJoinCompleted(ReqJoinCompleted message)
	{
		LogReceivedMessage(message);
		if (JoinedClientIds.Add(message.SenderId))
		{
			OnJoinCompleted.OnNext(message.SenderId);
		}
	}

	private void OnUpdateClientState(ReqUpdateClientState message)
	{
		LogReceivedMessage(message);
		_roomContext.ClientStates[message.SenderId] = message.State;
		EClientState state = message.State;
		if (state == EClientState.SceneLoaded && _roomContext.RoomState == ERoomState.Creating && message.SenderId == 0)
		{
			SetState(ERoomState.Ready);
		}
		if (state == EClientState.SceneLoaded && IsLateJoinDuringLoungeCampaign(message.SenderId))
		{
			OnClientCaughtUp.OnNext(message.SenderId);
		}
		if (state == EClientState.SceneLoaded && _roomContext.RoomState == ERoomState.Ready)
		{
			_roomContext.ClientStates[message.SenderId] = EClientState.Playable;
			SendToClient(message.SenderId, new NtfGameReady());
			SendPlayersReadyAsync().Forget();
		}
		UpdateClientStatesToPlayable();
	}

	private bool IsLateJoinDuringLoungeCampaign(ulong senderId)
	{
		if (_roomContext.RoomState != ERoomState.Playing)
		{
			return false;
		}
		if (!_gameContext.IsInLounge)
		{
			return false;
		}
		return _roomContext.ClientStates.Where((KeyValuePair<ulong, EClientState> _) => _.Key != senderId).Any((KeyValuePair<ulong, EClientState> _) => _.Value == EClientState.Playable);
	}

	private void UpdateClientStatesToPlayable()
	{
		if (!_roomContext.ClientStates.Any() || _roomContext.RoomState != ERoomState.Playing)
		{
			return;
		}
		if (_roomContext.ClientStates.All((KeyValuePair<ulong, EClientState> _) => _.Value == EClientState.SceneLoaded))
		{
			_roomContext.ClientStates.Keys.ToList().ForEach(delegate(ulong _)
			{
				_roomContext.ClientStates[_] = EClientState.Playable;
			});
			SendPlayersReadyAsync().Forget();
		}
	}

	private async UniTask SendPlayersReadyAsync()
	{
		await UniTask.DelayFrame(1);
		if (!_roomContext.ClientStates.Any((KeyValuePair<ulong, EClientState> _) => _.Value != EClientState.Playable))
		{
			SendToJoined(new NtfPlayersReady());
		}
	}

	public IObservable<T> Receive<T>()
	{
		return _messageBroker.Receive<T>();
	}

	private void RegisterHandlers()
	{
		_handlers.Add(typeof(ReqAcquireParadox), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqAcquireParadox);
		});
		_handlers.Add(typeof(ReqAddLuaBuff), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqAddLuaBuff);
		});
		_handlers.Add(typeof(ReqAddPointMarker), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqAddPointMarker);
		});
		_handlers.Add(typeof(ReqAnyToBagSlot), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqAnyToBagSlot);
		});
		_handlers.Add(typeof(ReqBagSlotToAny), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqBagSlotToAny);
		});
		_handlers.Add(typeof(ReqBuyInterior), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqBuyInterior);
		});
		_handlers.Add(typeof(ReqChangeCharacter), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqChangeCharacter);
		});
		_handlers.Add(typeof(ReqChangeCharacterSkills), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqChangeCharacterSkills);
		});
		_handlers.Add(typeof(ReqChangeCharacterSkin), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqChangeCharacterSkin);
		});
		_handlers.Add(typeof(ReqCheatAddGoods), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqCheatAddGoods);
		});
		_handlers.Add(typeof(ReqCheatAddParadox), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqCheatAddParadox);
		});
		_handlers.Add(typeof(ReqCheatAddRandomParadox), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqCheatAddRandomParadox);
		});
		_handlers.Add(typeof(ReqCheatApplyCharacterOptions), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqCheatApplyCharacterOptions);
		});
		_handlers.Add(typeof(ReqCheatApplyMonsterHp), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqCheatApplyMonsterHp);
		});
		_handlers.Add(typeof(ReqCheatApplySoul), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqCheatApplySoul);
		});
		_handlers.Add(typeof(ReqCheatLevelUp), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqCheatLevelUp);
		});
		_handlers.Add(typeof(ReqCheatMovePosition), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqCheatMovePosition);
		});
		_handlers.Add(typeof(ReqCheatRecoverBrightness), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqCheatRecoverBrightness);
		});
		_handlers.Add(typeof(ReqCheatResetSkillCooldown), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqCheatResetSkillCooldown);
		});
		_handlers.Add(typeof(ReqCheatReviveCharacter), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqCheatReviveCharacter);
		});
		_handlers.Add(typeof(ReqCheatSetCoin), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqCheatSetCoin);
		});
		_handlers.Add(typeof(ReqCheatSetExp), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqCheatSetExp);
		});
		_handlers.Add(typeof(ReqCheatSetGold), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqCheatSetGold);
		});
		_handlers.Add(typeof(ReqCheatSkillCharge), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqCheatSkillCharge);
		});
		_handlers.Add(typeof(ReqCheatSkillMonsterAll), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqCheatSkillMonsterAll);
		});
		_handlers.Add(typeof(ReqCheatSpawnMonster), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqCheatSpawnMonster);
		});
		_handlers.Add(typeof(ReqCheatSpawnTrap), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqCheatSpawnTrap);
		});
		_handlers.Add(typeof(ReqCheatSpawnWaveExecutor), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqCheatSpawnWaveExecutor);
		});
		_handlers.Add(typeof(ReqCheatUnlockAllTalent), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqCheatUnlockAllTalent);
		});
		_handlers.Add(typeof(ReqClientIntroFinished), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqClientIntroFinished);
		});
		_handlers.Add(typeof(ReqConsumeGoods), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqConsumeGoods);
		});
		_handlers.Add(typeof(ReqCraftingGoods), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqCraftingGoods);
		});
		_handlers.Add(typeof(ReqDealArtifact), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqDealArtifact);
		});
		_handlers.Add(typeof(ReqDropInventorySlot), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqDropInventorySlot);
		});
		_handlers.Add(typeof(ReqDropSafeSlot), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqDropSafeSlot);
		});
		_handlers.Add(typeof(ReqEmoji), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqEmoji);
		});
		_handlers.Add(typeof(ReqEquipInventorySlot), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqEquipInventorySlot);
		});
		_handlers.Add(typeof(ReqEquipLootingSlot), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqEquipLootingSlot);
		});
		_handlers.Add(typeof(ReqEquipSafeSlot), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqEquipSafeSlot);
		});
		_handlers.Add(typeof(ReqEquipStashSlot), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqEquipStashSlot);
		});
		_handlers.Add(typeof(ReqExchangeCoin), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqExchangeCoin);
		});
		_handlers.Add(typeof(ReqFinishCutscene), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqFinishCutscene);
		});
		_handlers.Add(typeof(ReqFinishInteractionWaiting), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqFinishInteractionWaiting);
		});
		_handlers.Add(typeof(ReqForceEscape), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqForceEscape);
		});
		_handlers.Add(typeof(ReqGainAccumulatedExp), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqGainAccumulatedExp);
		});
		_handlers.Add(typeof(ReqGainGoods), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqGainGoods);
		});
		_handlers.Add(typeof(ReqInputCancelInteract), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqInputCancelInteract);
		});
		_handlers.Add(typeof(ReqInputMoveAim), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqInputMoveAim);
		});
		_handlers.Add(typeof(ReqInputRun), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqInputRun);
		});
		_handlers.Add(typeof(ReqInputSkill), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqInputSkill);
		});
		_handlers.Add(typeof(ReqInputTryInteract), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqInputTryInteract);
		});
		_handlers.Add(typeof(ReqInputUseItem), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqInputUseItem);
		});
		_handlers.Add(typeof(ReqInventorySlotToLootingSlot), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqInventorySlotToLootingSlot);
		});
		_handlers.Add(typeof(ReqInventorySlotToSafeSlot), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqInventorySlotToSafeSlot);
		});
		_handlers.Add(typeof(ReqInventorySlotToStashSlot), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqInventorySlotToStashSlot);
		});
		_handlers.Add(typeof(ReqJoinCompleted), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqJoinCompleted);
		});
		_handlers.Add(typeof(ReqJoinGame), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqJoinGame);
		});
		_handlers.Add(typeof(ReqKillCharacter), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqKillCharacter);
		});
		_handlers.Add(typeof(ReqLevelUp), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqLevelUp);
		});
		_handlers.Add(typeof(ReqLootingSlotToInventorySlot), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqLootingSlotToInventorySlot);
		});
		_handlers.Add(typeof(ReqLootingSlotToSafeSlot), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqLootingSlotToSafeSlot);
		});
		_handlers.Add(typeof(ReqMoveAllInventoryToStash), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqMoveAllInventoryToStash);
		});
		_handlers.Add(typeof(ReqOrganizeInventorySafeSlots), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqOrganizeInventorySafeSlots);
		});
		_handlers.Add(typeof(ReqOrganizeInventorySlots), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqOrganizeInventorySlots);
		});
		_handlers.Add(typeof(ReqParadoxLock), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqParadoxLock);
		});
		_handlers.Add(typeof(ReqParadoxPurify), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqParadoxPurify);
		});
		_handlers.Add(typeof(ReqParadoxUnlock), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqParadoxUnlock);
		});
		_handlers.Add(typeof(ReqPing), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqPing);
		});
		_handlers.Add(typeof(ReqPlayCutscene), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqPlayCutscene);
		});
		_handlers.Add(typeof(ReqRegisterItemUsed), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqRegisterItemUsed);
		});
		_handlers.Add(typeof(ReqRemoveInventorySlot), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqRemoveInventorySlot);
		});
		_handlers.Add(typeof(ReqRemoveParadox), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqRemoveParadox);
		});
		_handlers.Add(typeof(ReqRemovePointMarker), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqRemovePointMarker);
		});
		_handlers.Add(typeof(ReqRemoveSafeSlot), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqRemoveSafeSlot);
		});
		_handlers.Add(typeof(ReqRepairAllGoods), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqRepairAllGoods);
		});
		_handlers.Add(typeof(ReqRepairGoods), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqRepairGoods);
		});
		_handlers.Add(typeof(ReqRerollLostItems), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqRerollLostItems);
		});
		_handlers.Add(typeof(ReqRest), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqRest);
		});
		_handlers.Add(typeof(ReqRewardBox), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqRewardBox);
		});
		_handlers.Add(typeof(ReqSafeSlotToInventorySlot), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqSafeSlotToInventorySlot);
		});
		_handlers.Add(typeof(ReqSafeSlotToLootingSlot), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqSafeSlotToLootingSlot);
		});
		_handlers.Add(typeof(ReqSafeSlotToStashSlot), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqSafeSlotToStashSlot);
		});
		_handlers.Add(typeof(ReqScrapGoods), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqScrapGoods);
		});
		_handlers.Add(typeof(ReqSellArtifact), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqSellArtifact);
		});
		_handlers.Add(typeof(ReqSetCharacterCorruption), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqSetCharacterCorruption);
		});
		_handlers.Add(typeof(ReqSetCharacterForward), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqSetCharacterForward);
		});
		_handlers.Add(typeof(ReqSetCharacterHp), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqSetCharacterHp);
		});
		_handlers.Add(typeof(ReqSetCharacterLightFuel), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqSetCharacterLightFuel);
		});
		_handlers.Add(typeof(ReqSetCharacterPosition), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqSetCharacterPosition);
		});
		_handlers.Add(typeof(ReqSetCharacterStress), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqSetCharacterStress);
		});
		_handlers.Add(typeof(ReqSetPet), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqSetPet);
		});
		_handlers.Add(typeof(ReqShopBuyGoods), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqShopBuyGoods);
		});
		_handlers.Add(typeof(ReqShopSellGoods), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqShopSellGoods);
		});
		_handlers.Add(typeof(ReqSpawnDropGoods), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqSpawnDropGoods);
		});
		_handlers.Add(typeof(ReqSpawnExit), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqSpawnExit);
		});
		_handlers.Add(typeof(ReqSpawnMonster), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqSpawnMonster);
		});
		_handlers.Add(typeof(ReqSpawnWaveExecutor), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqSpawnWaveExecutor);
		});
		_handlers.Add(typeof(ReqStageResultPlayerLocalInfo), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqStageResultPlayerLocalInfo);
		});
		_handlers.Add(typeof(ReqStashSlotToInventorySlot), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqStashSlotToInventorySlot);
		});
		_handlers.Add(typeof(ReqStashSlotToSafeSlot), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqStashSlotToSafeSlot);
		});
		_handlers.Add(typeof(ReqSwapEquipmentSlots), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqSwapEquipmentSlots);
		});
		_handlers.Add(typeof(ReqSwapInventorySlots), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqSwapInventorySlots);
		});
		_handlers.Add(typeof(ReqSwapLootingSlots), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqSwapLootingSlots);
		});
		_handlers.Add(typeof(ReqSwapSafeSlots), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqSwapSafeSlots);
		});
		_handlers.Add(typeof(ReqSwapWeapon), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqSwapWeapon);
		});
		_handlers.Add(typeof(ReqSyncInventory), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqSyncInventory);
		});
		_handlers.Add(typeof(ReqSyncSafeInventory), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqSyncSafeInventory);
		});
		_handlers.Add(typeof(ReqSyncShop), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqSyncShop);
		});
		_handlers.Add(typeof(ReqSyncUnitModel), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqSyncUnitModel);
		});
		_handlers.Add(typeof(ReqTakeLostItem), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqTakeLostItem);
		});
		_handlers.Add(typeof(ReqTriggerLuaEvent), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqTriggerLuaEvent);
		});
		_handlers.Add(typeof(ReqTryKeyInteraction), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqTryKeyInteraction);
		});
		_handlers.Add(typeof(ReqTycoonCancel), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqTycoonCancel);
		});
		_handlers.Add(typeof(ReqTycoonFinish), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqTycoonFinish);
		});
		_handlers.Add(typeof(ReqTycoonHire), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqTycoonHire);
		});
		_handlers.Add(typeof(ReqTycoonPurify), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqTycoonPurify);
		});
		_handlers.Add(typeof(ReqTycoonRegisterArtifact), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqTycoonRegisterArtifact);
		});
		_handlers.Add(typeof(ReqUnequipToInventorySlot), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqUnequipToInventorySlot);
		});
		_handlers.Add(typeof(ReqUnequipToLootingSlot), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqUnequipToLootingSlot);
		});
		_handlers.Add(typeof(ReqUnequipToSafeSlot), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqUnequipToSafeSlot);
		});
		_handlers.Add(typeof(ReqUnequipToStashSlot), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqUnequipToStashSlot);
		});
		_handlers.Add(typeof(ReqUnlockTalent), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqUnlockTalent);
		});
		_handlers.Add(typeof(ReqUpdateCampaignState), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqUpdateCampaignState);
		});
		_handlers.Add(typeof(ReqUpdateClientState), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqUpdateClientState);
		});
		_handlers.Add(typeof(ReqUserChat), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqUserChat);
		});
		_handlers.Add(typeof(ReqVoteCampaign), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqVoteCampaign);
		});
		_handlers.Add(typeof(ReqVoteGiveUpCampaign), delegate(BaseMessage _)
		{
			_messageBroker.Publish(_ as ReqVoteGiveUpCampaign);
		});
	}

	private void OnTransportFailure()
	{
		if (_networkManager.IsHost)
		{
			_logger.Debug();
		}
	}

	private void OnClientConnected(ulong clientId)
	{
		if (_networkManager.IsHost)
		{
			if (clientId == _networkManager.LocalClientId)
			{
				OnStarted.OnNext(Unit.Default);
			}
			_logger.Debug(string.Format("ClientId: {0}", clientId));
		}
	}

	private void OnClientDisconnect(ulong clientId)
	{
		if (_networkManager.IsHost)
		{
			TryLeavePlayer(clientId);
			if (_roomContext.ClientStates.Remove(clientId, out var value) && value != EClientState.Playable)
			{
				UpdateClientStatesToPlayable();
			}
			_logger.Debug(string.Format("ClientId: {0}", clientId));
		}
	}

	private void OnServerStarted()
	{
		if (_networkManager.IsHost)
		{
			SnapshotSentClientIds.Add(0uL);
			JoinedClientIds.Add(0uL);
			_logger.Debug();
		}
	}

	private void OnServerStopped(bool _)
	{
		if (_networkManager.IsHost)
		{
			_roomContext.Clear();
			SnapshotSentClientIds.Clear();
			JoinedClientIds.Clear();
			OnStopped.OnNext(Unit.Default);
			_disposable?.Dispose();
			_disposable = null;
			_logger.Debug();
		}
	}

	private void OnConnectionApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
	{
		void SetResponse(int resultCode)
		{
			response.Approved = resultCode == 0;
			response.CreatePlayerObject = resultCode == 0;
			response.Reason = string.Format("{0}", resultCode);
		}
		if (request.ClientNetworkId == _networkManager.LocalClientId)
		{
			SetResponse(0);
			return;
		}
		ConnectionData connectionData = SerializeHelper.Deserialize<ConnectionData>(request.Payload);
		SetResponse(ValidateJoin(connectionData.Version));
	}

	private int ValidateJoin(AppVersion clientVersion)
	{
		if (clientVersion != _appManager.Version)
		{
			return NetworkResultCode.VERSION_MISMATCH;
		}
		if (_roomContext.RoomState == ERoomState.Playing)
		{
			return NetworkResultCode.GAME_ALREADY_STARTED;
		}
		if (_roomContext.RoomState != ERoomState.Ready)
		{
			return NetworkResultCode.GAME_ALREADY_STARTED;
		}
		return (_gameContext.Players.Count < _roomContext.PlayerCountMax) ? NetworkResultCode.OK : NetworkResultCode.PLAYER_FULL;
	}
}
