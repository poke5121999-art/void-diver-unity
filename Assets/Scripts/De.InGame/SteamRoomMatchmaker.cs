using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Enum;
using NemoLib.Log;
using Steamworks;
using UniRx;
using VContainer;

public class SteamRoomMatchmaker : MonoBase, IRoomMatchmaker
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CGetLobbyAsync_003Ed__44 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<MatchingLobby> _003C_003Et__builder;

		public ulong lobbyId;

		public SteamRoomMatchmaker _003C_003E4__this;

		private TaskAwaiter<MatchingLobby> _003C_003Eu__1;

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
	private struct _003CIRoomMatchmaker_002DCreateLobby_003Ed__38 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<ECreateLobbyResult> _003C_003Et__builder;

		public SteamRoomMatchmaker _003C_003E4__this;

		public MatchingLobbyOptions options;

		private TaskAwaiter<LobbyCreated_t> _003C_003Eu__1;

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
	private struct _003CIRoomMatchmaker_002DGetLobbyListAsync_003Ed__45 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<MatchingLobby[]> _003C_003Et__builder;

		public SteamRoomMatchmaker _003C_003E4__this;

		public bool blockOtherAreas;

		public bool showPlayingLounge;

		private TaskAwaiter<MatchingLobby[]> _003C_003Eu__1;

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
	private struct _003CIRoomMatchmaker_002DJoinLobby_003Ed__40 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<int> _003C_003Et__builder;

		public ulong lobbyId;

		public SteamRoomMatchmaker _003C_003E4__this;

		private CSteamID _003CsteamLobbyId_003E5__2;

		private MatchingLobby _003Clobby_003E5__3;

		private UniTask<MatchingLobby>.Awaiter _003C_003Eu__1;

		private TaskAwaiter<ulong> _003C_003Eu__2;

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

	[Inject]
	private SeriLogger _logger;

	protected Callback<LobbyCreated_t> CallbackLobbyCreated;

	protected Callback<LobbyMatchList_t> CallbackLobbyMatchList;

	protected Callback<LobbyEnter_t> CallbackLobbyEnter;

	protected Callback<LobbyDataUpdate_t> CallbackLobbyDataUpdate;

	protected Callback<LobbyKicked_t> CallbackLobbyKicked;

	private TaskCompletionSource<ulong> _joinLobbyTcs;

	private TaskCompletionSource<MatchingLobby[]> _getLobbyListTcs;

	private TaskCompletionSource<MatchingLobby> _requestLobbyDataTcs;

	private CSteamID _requestLobbyId;

	private readonly Dictionary<CSteamID, MatchingLobby> _cachedLobbies;

	private TaskCompletionSource<LobbyCreated_t> _createLobbyTcs;

	public MatchingLobby JoinedLobby { get; private set; }

	private CSteamID JoinedLobbyId => default;

	public Subject<Unit> OnCreateLobby { get; }

	public Subject<bool> OnJoinLobby { get; }

	public Subject<Unit> OnLeaveLobby { get; }

	public Subject<MatchingLobby> OnCurrentLobbyDataUpdated { get; }

	public string LoungeCode => null;

	public bool IsAlreadyJoined => false;

	public bool IsPrivateLobby => false;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	[AsyncStateMachine(typeof(_003CIRoomMatchmaker_002DCreateLobby_003Ed__38))]
	UniTask<ECreateLobbyResult> IRoomMatchmaker.CreateLobby(MatchingLobbyOptions options)
	{
		return default;
	}

	private void OnLobbyCreated(LobbyCreated_t result)
	{
	}

	[AsyncStateMachine(typeof(_003CIRoomMatchmaker_002DJoinLobby_003Ed__40))]
	UniTask<int> IRoomMatchmaker.JoinLobby(ulong lobbyId)
	{
		return default;
	}

	private void OnLobbyEntered(LobbyEnter_t result)
	{
	}

	void IRoomMatchmaker.LeaveLobby()
	{
	}

	private void OnLobbyKicked(LobbyKicked_t pCallback)
	{
	}

	[AsyncStateMachine(typeof(_003CGetLobbyAsync_003Ed__44))]
	public UniTask<MatchingLobby> GetLobbyAsync(ulong lobbyId)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CIRoomMatchmaker_002DGetLobbyListAsync_003Ed__45))]
	UniTask<MatchingLobby[]> IRoomMatchmaker.GetLobbyListAsync(bool blockOtherAreas, bool showPlayingLounge)
	{
		return default;
	}

	private void OnLobbyList(LobbyMatchList_t result)
	{
	}

	private void OnLobbyUpdated(LobbyDataUpdate_t result)
	{
	}

	public void SetJoinable(bool isJoinable)
	{
	}

	public void UpdateLobbyType(ERoomPublishType publishType)
	{
	}

	public void UpdateLobbyTitle(string title)
	{
	}

	public void UpdateLobbyDifficulty(ERoomDifficulty difficulty)
	{
	}

	public void UpdateLobbySettings(string title, ERoomPublishType publishType, ERoomDifficulty difficulty)
	{
	}

	public void UpdateGameStarted(bool isStarted)
	{
	}

	private int GetPingTime(string pingLocationString)
	{
		return 0;
	}
}
