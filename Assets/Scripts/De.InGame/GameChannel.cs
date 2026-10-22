using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Log;
using NemoLib.Popup;
using NemoLib.Scene;
using Steamworks;
using UniRx;
using Unity.Netcode;
using VContainer;

public class GameChannel : NativeBase, INetworkChannel
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass48_0
	{
		public UniTaskCompletionSource tcs;

		public GameChannel _003C_003E4__this;

		internal void _003CShutdownAsync_003Eb__0(Unit _)
		{
		}

		internal void _003CShutdownAsync_003Eb__1(Unit _)
		{
		}

		internal bool _003CShutdownAsync_003Eb__2()
		{
			return false;
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CConnectAndJoinByAddressAsync_003Ed__68 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<int> _003C_003Et__builder;

		public GameChannel _003C_003E4__this;

		public string address;

		public ushort port;

		public GamePlayerData playerData;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

		private UniTask.Awaiter _003C_003Eu__2;

		private UniTask<int>.Awaiter _003C_003Eu__3;

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
	private struct _003CConnectAndJoinBySteamLobbyAsync_003Ed__67 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<int> _003C_003Et__builder;

		public GameChannel _003C_003E4__this;

		public ulong lobbyId;

		public GamePlayerData playerData;

		private CSteamID _003CsteamLobbyId_003E5__2;

		private MatchingLobby _003CmatchingLobby_003E5__3;

		private UniTask<int>.Awaiter _003C_003Eu__1;

		private UniTask<bool>.Awaiter _003C_003Eu__2;

		private UniTask.Awaiter _003C_003Eu__3;

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
	private struct _003CMonitorConnectionTimeoutAsync_003Ed__72 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public GameChannel _003C_003E4__this;

		private TimeSpan _003Cstep_003E5__2;

		private float _003Celapsed_003E5__3;

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

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CRecreateTransportAsync_003Ed__52 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public GameChannel _003C_003E4__this;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

		private UniTask.Awaiter _003C_003Eu__2;

		private Cysharp.Threading.Tasks.YieldAwaitable.Awaiter _003C_003Eu__3;

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
	private struct _003CRequestAsync_003Ed__57<T> : IAsyncStateMachine where T : BaseMessage
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<T> _003C_003Et__builder;

		public GameChannel _003C_003E4__this;

		public BaseMessage baseMessage;

		private BlockScope _003CblockScope_003E5__2;

		private int _003Csequence_003E5__3;

		private UniTaskCompletionSource<BaseMessage> _003Ctcs_003E5__4;

		private T _003Cmessage_003E5__5;

		private UniTask<BaseMessage>.Awaiter _003C_003Eu__1;

		private Cysharp.Threading.Tasks.YieldAwaitable.Awaiter _003C_003Eu__2;

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
	private struct _003CSendJoinRequestWithRetryAsync_003Ed__73 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public GameChannel _003C_003E4__this;

		private GamePlayerData _003CplayerData_003E5__2;

		private int _003Cattempt_003E5__3;

		private UniTask<ResJoinGame>.Awaiter _003C_003Eu__1;

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
	private struct _003CShutdownAsync_003Ed__48 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public GameChannel _003C_003E4__this;

		private _003C_003Ec__DisplayClass48_0 _003C_003E8__1;

		private IDisposable _003Cdisposable_003E5__2;

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

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CTryJoinGame_003Ed__46 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<int> _003C_003Et__builder;

		public GameChannel _003C_003E4__this;

		public GamePlayerData playerData;

		private BlockScope _003CblockScope_003E5__2;

		private TaskAwaiter<int> _003C_003Eu__1;

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
	private struct _003CWaitNetworkInactiveAsync_003Ed__51 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

		public GameChannel _003C_003E4__this;

		public float timeoutSeconds;

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
	private EnvironmentSettings _settings;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private IRoomMatchmaker _roomMatchmaker;

	[Inject]
	private SceneManager _sceneManager;

	[Inject]
	private AppManager _appManager;

	private readonly MessageBroker _messageBroker;

	private readonly Dictionary<Type, Action<BaseMessage>> _handlers;

	private TaskCompletionSource<int> _joinLobbyTcs;

	private GamePlayerData _pendingPlayerData;

	private CompositeDisposable _disposable;

	private bool _joinResponseReceived;

	private bool _clientConnected;

	private int _requestSequence;

	private readonly Dictionary<int, UniTaskCompletionSource<BaseMessage>> _requests;

	private NetworkTransport Transport => null;

	private ulong HostId => 0uL;

	private ulong LocalClientId => 0uL;

	public bool IsHost => false;

	public Subject<Unit> OnJoined { get; private set; }

	public Subject<Unit> OnStopped { get; private set; }

	public Subject<Unit> OnHostStopped { get; private set; }

	public ReactiveProperty<ERoomConnectState> RxConnectState { get; private set; }

	private bool IsNetworkInactive => false;

	private string BuildNetworkDiagnosticContext()
	{
		return null;
	}

	public void SetUp(string address, ushort port)
	{
	}

	public void SetUp(ulong steamId)
	{
	}

	private void RegisterHandler()
	{
	}

	[AsyncStateMachine(typeof(_003CTryJoinGame_003Ed__46))]
	public UniTask<int> TryJoinGame(GamePlayerData playerData)
	{
		return default;
	}

	public void LeftRoom()
	{
	}

	[AsyncStateMachine(typeof(_003CShutdownAsync_003Ed__48))]
	public UniTask ShutdownAsync()
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CWaitNetworkInactiveAsync_003Ed__51))]
	private UniTask<bool> WaitNetworkInactiveAsync(float timeoutSeconds = 5f)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CRecreateTransportAsync_003Ed__52))]
	public UniTask RecreateTransportAsync()
	{
		return default;
	}

	public void SendToHost(BaseMessage baseMessage)
	{
	}

	public void SendToClient(ulong clientId, BaseMessage baseMessage)
	{
	}

	[AsyncStateMachine(typeof(_003CRequestAsync_003Ed__57<>))]
	public UniTask<T> RequestAsync<T>(BaseMessage baseMessage) where T : BaseMessage
	{
		return default;
	}

	private void OnMessageReceived(ulong clientId, FastBufferReader reader)
	{
	}

	private void LogReceivedMessage(BaseMessage message)
	{
	}

	private void OnJoinGame(ResJoinGame message)
	{
	}

	private void OnPlayerJoined(NtfPlayerJoined message)
	{
	}

	private void OnPlayerLeft(NtfPlayerLeft message)
	{
	}

	private void OnUpdateGamePlayer(NtfUpdateGamePlayer message)
	{
	}

	private void OnRtt(NtfRtt message)
	{
	}

	private void OnUpdateRoomState(NtfUpdateRoomState message)
	{
	}

	public IObservable<T> Receive<T>()
	{
		return null;
	}

	[AsyncStateMachine(typeof(_003CConnectAndJoinBySteamLobbyAsync_003Ed__67))]
	public UniTask<int> ConnectAndJoinBySteamLobbyAsync(ulong lobbyId, GamePlayerData playerData)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CConnectAndJoinByAddressAsync_003Ed__68))]
	public UniTask<int> ConnectAndJoinByAddressAsync(string address, ushort port, GamePlayerData playerData)
	{
		return default;
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

	[AsyncStateMachine(typeof(_003CMonitorConnectionTimeoutAsync_003Ed__72))]
	private UniTask MonitorConnectionTimeoutAsync()
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CSendJoinRequestWithRetryAsync_003Ed__73))]
	private UniTask SendJoinRequestWithRetryAsync()
	{
		return default;
	}

	private bool IsJoinSettled()
	{
		return false;
	}

	private void OnClientDisconnected(ulong clientId)
	{
	}

	private void OnConnectionEvent(NetworkManager networkManager, ConnectionEventData eventData)
	{
	}

	private void OnClientStopped(bool isServer)
	{
	}

	private void OnServerStopped(bool isServer)
	{
	}
}
