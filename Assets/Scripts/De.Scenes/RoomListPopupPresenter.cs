using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Mvp;
using NemoLib.Popup;
using UnityEngine.UI;
using VContainer;

public class RoomListPopupPresenter : GamePopupPresenter
{
	private enum EFocusPanel
	{
		Top = 0,
		Scroller = 1,
		Bottom = 2
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnEnterButtonClickAsync_003Ed__38 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public RoomListPopupPresenter _003C_003E4__this;

		private UniTask<int>.Awaiter _003C_003Eu__1;

		private UniTask.Awaiter _003C_003Eu__2;

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
	private struct _003COnFilterButtonClickAsync_003Ed__36 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public RoomListPopupPresenter _003C_003E4__this;

		private UniTask<LoungeFilterPopupPresenter>.Awaiter _003C_003Eu__1;

		private UniTask<object>.Awaiter _003C_003Eu__2;

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
	private struct _003COnOpenAsync_003Ed__26 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public RoomListPopupPresenter _003C_003E4__this;

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

	[View]
	private RoomListPopupView _view;

	[Inject]
	private AppManager _appManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameHost _gameHost;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private EnvironmentSettings _settings;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private IRoomMatchmaker _roomMatchmaker;

	private RoomScrollerModel _scrollerModel;

	private MatchingLobby _selectedLobby;

	private LoungeFilterPopupModel _filterModel;

	private EFocusPanel _focusPanel;

	private Selectable _lastFocusedBeforeScroller;

	private GameInputManager GameInputManager => null;

	public override string OpenSfx => null;

	public override bool IsInputActionPopup => false;

	private InputActions.UiActions UiActions => default;

	private RoomScrollerPresenter _scrollerPresenter => null;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__26))]
	protected override UniTask OnOpenAsync()
	{
		return default;
	}

	private void FocusTop()
	{
	}

	private void FocusScroller(ENavDir entryDir)
	{
	}

	private void FocusBottom()
	{
	}

	private Selectable MapToTop(Selectable source)
	{
		return null;
	}

	private Selectable MapToBottom(Selectable source)
	{
		return null;
	}

	protected override void OnNavOutOfBounds(ENavDir dir)
	{
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}

	private void OnRefreshButtonClick()
	{
	}

	[AsyncStateMachine(typeof(_003COnFilterButtonClickAsync_003Ed__36))]
	private UniTask OnFilterButtonClickAsync()
	{
		return default;
	}

	private void OnGameRoomClick(MatchingLobby matchingLobby)
	{
	}

	[AsyncStateMachine(typeof(_003COnEnterButtonClickAsync_003Ed__38))]
	private UniTask OnEnterButtonClickAsync()
	{
		return default;
	}

	private void OnEnterByCodeClick()
	{
	}

	private void OnConnectionFailed(int resultCode)
	{
	}
}
