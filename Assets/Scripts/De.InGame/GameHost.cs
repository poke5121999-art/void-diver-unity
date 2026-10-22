using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.Log;
using UniRx;
using Unity.Netcode;
using VContainer;

public class GameHost : NativeBase, INetworkHost
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CPublishRoomAsync_003Ed__32 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<ECreateLobbyResult> _003C_003Et__builder;

		public GameHost _003C_003E4__this;

		public ERoomDifficulty difficulty;

		public ERoomPublishType publishType;

		public int playerCountMax;

		public string roomTitle;

		public string playerName;

		public int playerLevel;

		private UniTask<ECreateLobbyResult>.Awaiter _003C_003Eu__1;

		private void MoveNext()
		{
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CSendPlayersReadyAsync_003Ed__57 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public GameHost _003C_003E4__this;

		private UniTask.Awaiter _003C_003Eu__1;

		private void MoveNext()
		{
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

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

	private readonly Dictionary<Type, Action<BaseMessage>> _handlers;

	private readonly MessageBroker _messageBroker;

	private CompositeDisposable _disposable;

	private NetworkTransport Transport => null;

	private ulong HostId => 0uL;

	private ulong LocalClientId => 0uL;

	public bool IsAllPlayersReady => false;

	public Subject<Unit> OnStarted { get; }

	public Subject<Unit> OnStopped { get; }

	public Subject<(string address, ushort port)> OnConnectionDataSet { get; }

	public HashSet<ulong> JoinedClientIds { get; }

	public HashSet<ulong> SnapshotSentClientIds { get; }

	public Subject<ulong> OnJoinCompleted { get; }

	public Subject<ulong> OnClientCaughtUp { get; }

	public ulong GetCurrentRtt(ulong clientId)
	{
		return 0uL;
	}

	public void SetUp(string address, ushort port)
	{
	}

	public void CreateRoom(ERoomDifficulty difficulty)
	{
	}

	[AsyncStateMachine(typeof(_003CPublishRoomAsync_003Ed__32))]
	public UniTask<ECreateLobbyResult> PublishRoomAsync(string roomTitle, int playerCountMax, string playerName, int playerLevel, ERoomPublishType publishType, ERoomDifficulty difficulty)
	{
		return default;
	}

	public void CloseRoom()
	{
	}

	public void LoadScene(string sceneName)
	{
	}

	public void SetState(ERoomState state)
	{
	}

	public void SendToClient(ulong clientId, BaseMessage baseMessage)
	{
	}

	public void SendToAll(BaseMessage baseMessage, HashSet<ulong> excludeIds = null)
	{
	}

	public void SendToSnapshotApplied(BaseMessage baseMessage, HashSet<ulong> excludeIds = null)
	{
	}

	public void SendToJoined(BaseMessage baseMessage, HashSet<ulong> excludeIds = null)
	{
	}

	private void SendToGated(BaseMessage baseMessage, HashSet<ulong> gate, HashSet<ulong> excludeIds)
	{
	}

	public void SendToAllExceptHost(BaseMessage baseMessage, HashSet<ulong> excludeIds = null)
	{
	}

	private void OnMessageReceived(ulong clientID, FastBufferReader reader)
	{
	}

	private int TryJoinPlayer(ulong clientId, GamePlayerData playerData, int sequence)
	{
		return 0;
	}

	private void ResendJoinResult(ulong clientId, int sequence)
	{
	}

	private void FlushPlayerChanges(ulong excludeClientId)
	{
	}

	private bool TryLeavePlayer(ulong clientId)
	{
		return false;
	}

	public void SendNoticeToastToAll(string textKey, long fontSize = 24L, HashSet<ulong> excludeIds = null)
	{
	}

	public void SendNoticeToast(string textKey, ulong targetClientId, long fontSize = 24L)
	{
	}

	public void SendPlayerKilledToastToAll(NtfPlayerKilledToast notice, HashSet<ulong> excludeIds = null)
	{
	}

	private void BroadcastRtt()
	{
	}

	private void LogReceivedMessage(BaseMessage message)
	{
	}

	private void OnJoinGame(ReqJoinGame message)
	{
	}

	private void OnReqJoinCompleted(ReqJoinCompleted message)
	{
	}

	private void OnUpdateClientState(ReqUpdateClientState message)
	{
	}

	private bool IsLateJoinDuringLoungeCampaign(ulong senderId)
	{
		return false;
	}

	private void UpdateClientStatesToPlayable()
	{
	}

	[AsyncStateMachine(typeof(_003CSendPlayersReadyAsync_003Ed__57))]
	private UniTask SendPlayersReadyAsync()
	{
		return default;
	}

	public IObservable<T> Receive<T>()
	{
		return null;
	}

	private void RegisterHandlers()
	{
	}

	private void OnTransportFailure()
	{
	}

	private void OnClientConnected(ulong clientId)
	{
	}

	private void OnClientDisconnect(ulong clientId)
	{
	}

	private void OnServerStarted()
	{
	}

	private void OnServerStopped(bool _)
	{
	}

	private void OnConnectionApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
	{
	}

	private int ValidateJoin(AppVersion clientVersion)
	{
		return 0;
	}
}
