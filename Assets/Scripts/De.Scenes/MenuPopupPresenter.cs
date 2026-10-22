using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class MenuPopupPresenter : GamePopupPresenter
{
	public enum EPageCategory
	{
		Quest = 0,
		InventoryManagement = 1,
		Character = 2,
		Archive = 3,
		Squad = 4,
		Option = 5,
		System = 6
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__24 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public MenuPopupPresenter _003C_003E4__this;

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
	private MenuPopupView _view;

	[InjectModel]
	private MenuPopupModel _model;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private RoomContext _roomContext;

	[Inject]
	private IRoomMatchmaker _roomMatchmaker;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private SoundManager _soundManager;

	private List<MenuTabModel> _tabModels;

	private CompositeDisposable _pageDisposable;

	private BasePanelModel _selectedPageModel;

	private GameInputManager GameInputManager => null;

	public override bool IsInputActionPopup => false;

	private InputActions.UiActions UiActions => default;

	private InputActions.UiPadActions UiPadActions => default;

	public override string OpenSfx => null;

	public override string CloseSfx => null;

	private bool IsSquadTabLocked => false;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__24))]
	protected override UniTask OnOpenAsync()
	{
		return default;
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}

	protected override void OnDestroy()
	{
	}

	private void UpdatePage(EPageCategory category)
	{
	}

	private void SetTab(EPageCategory category)
	{
	}

	private void SetPage(EPageCategory category)
	{
	}

	private void OnTabLeft()
	{
	}

	private void OnTabRight()
	{
	}

	private void OnTabButtonClick(MenuTabModel tabModel)
	{
	}
}
