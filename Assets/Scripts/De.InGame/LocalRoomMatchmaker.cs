using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Enum;
using UniRx;
using Unity.Netcode;
using VContainer;

public class LocalRoomMatchmaker : MonoBase, IRoomMatchmaker
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CIRoomMatchmaker_002DCreateLobby_003Ed__15 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<ECreateLobbyResult> _003C_003Et__builder;

		public LocalRoomMatchmaker _003C_003E4__this;

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
	private struct _003CIRoomMatchmaker_002DGetLobbyAsync_003Ed__24 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<MatchingLobby> _003C_003Et__builder;

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
	private struct _003CIRoomMatchmaker_002DGetLobbyListAsync_003Ed__25 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<MatchingLobby[]> _003C_003Et__builder;

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
	private struct _003CIRoomMatchmaker_002DJoinLobby_003Ed__16 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<int> _003C_003Et__builder;

		public LocalRoomMatchmaker _003C_003E4__this;

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

	[Inject]
	private NetworkManager _networkManager;

	private IRoomMatchmaker _roomMatchmakerImplementation;

	public Subject<Unit> OnCreateLobby { get; }

	public Subject<bool> OnJoinLobby { get; }

	public Subject<Unit> OnLeaveLobby { get; }

	public Subject<MatchingLobby> OnCurrentLobbyDataUpdated { get; }

	public string LoungeCode => null;

	public MatchingLobby JoinedLobby => null;

	public bool IsAlreadyJoined => false;

	public bool IsPrivateLobby => false;

	protected override void Awake()
	{
	}

	[AsyncStateMachine(typeof(_003CIRoomMatchmaker_002DCreateLobby_003Ed__15))]
	UniTask<ECreateLobbyResult> IRoomMatchmaker.CreateLobby(MatchingLobbyOptions options)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CIRoomMatchmaker_002DJoinLobby_003Ed__16))]
	UniTask<int> IRoomMatchmaker.JoinLobby(ulong lobbyId)
	{
		return default;
	}

	void IRoomMatchmaker.LeaveLobby()
	{
	}

	public void SetJoinable(bool isJoinable)
	{
	}

	public void UpdateGameStarted(bool isStarted)
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

	[AsyncStateMachine(typeof(_003CIRoomMatchmaker_002DGetLobbyAsync_003Ed__24))]
	UniTask<MatchingLobby> IRoomMatchmaker.GetLobbyAsync(ulong lobbyId)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CIRoomMatchmaker_002DGetLobbyListAsync_003Ed__25))]
	UniTask<MatchingLobby[]> IRoomMatchmaker.GetLobbyListAsync(bool blockOtherAreas, bool showPlayingLounge)
	{
		return default;
	}
}
