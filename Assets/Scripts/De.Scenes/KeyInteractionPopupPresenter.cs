using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using VContainer;

public class KeyInteractionPopupPresenter : GamePopupPresenter
{
	private enum EFocusPanel
	{
		Left = 0,
		KeyInteraction = 1
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__32 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public KeyInteractionPopupPresenter _003C_003E4__this;

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
	private struct _003CTryPickRegisterAsync_003Ed__38 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public KeyInteractionPopupPresenter _003C_003E4__this;

		public int itemId;

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

	[InjectModel]
	private KeyInteractionPopupModel _model;

	[View]
	private KeyInteractionPopupView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private CameraManager _cameraManager;

	private EFocusPanel _focusPanel;

	private PlayerInventoryPanelModel _myInventoryPanelModel;

	private KeyInteractionPanelModel _keyInteractionPanelModel;

	private bool _isDragging;

	private bool _isSelectedSlotLocked;

	private InventoryGoodsSlotModel _selectedSlotModel;

	private InventoryGoodsSlotModel _targetSlotModel;

	private DraggingGoodsSlotModel _draggingSlotModel;

	private GameInputManager GameInputManager => null;

	public override bool IsInputActionPopup => false;

	public override string OpenSfx => null;

	private InputActions.UiActions UiActions => default;

	private InputActions.UiPadActions UiPadActions => default;

	private PlayerInventory MyInventory => null;

	private ReactiveCollection<int> RxSelectedItemIds => null;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__32))]
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

	private void FocusPanel(EFocusPanel panel)
	{
	}

	private void DisableAllPanelInput()
	{
	}

	private void OnSubmit()
	{
	}

	[AsyncStateMachine(typeof(_003CTryPickRegisterAsync_003Ed__38))]
	private UniTask TryPickRegisterAsync(int itemId)
	{
		return default;
	}

	protected override void Update()
	{
	}

	private void OnConfirm()
	{
	}

	private void OnBack()
	{
	}

	private void OnSlotClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnKeyInteractionSlotLeftClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSlotFocused(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSlotDrag(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSlotEndDrag(InventoryGoodsSlotModel slotModel)
	{
	}

	private void HandleInteraction()
	{
	}

	private bool TryRegisterItem(int itemId)
	{
		return false;
	}

	private bool TryUnregisterItem(int itemId)
	{
		return false;
	}
}
