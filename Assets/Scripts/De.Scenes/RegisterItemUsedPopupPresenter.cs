using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.Mvp;
using NemoLib.Popup;
using VContainer;

public class RegisterItemUsedPopupPresenter : GamePopupPresenter
{
	private enum EItemPanelType
	{
		Inventory = 0,
		Stash = 1
	}

	private enum EFocusPanel
	{
		Left = 0,
		Register = 1
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__35 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public RegisterItemUsedPopupPresenter _003C_003E4__this;

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
	private struct _003CTryRegisterItemUsedAsync_003Ed__51 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public RegisterItemUsedPopupPresenter _003C_003E4__this;

		private Goods _003Cgoods_003E5__2;

		private UniTask<ResRegisterItemUsed>.Awaiter _003C_003Eu__1;

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
	private struct _003CTrySetGoodsAsync_003Ed__50 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public InventoryGoodsSlotModel slotModel;

		public RegisterItemUsedPopupPresenter _003C_003E4__this;

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
	private RegisterItemUsedPopupView _view;

	[InjectModel]
	private RegisterItemUsedPopupModel _model;

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

	private EItemPanelType _itemPanelType;

	private EFocusPanel _focusPanel;

	private PlayerInventoryPanelModel _inventoryPanelModel;

	private StashPanelModel _stashPanelModel;

	private RegisterItemUsedPanelModel _panelModel;

	private bool _isDragging;

	private bool _isSelectedSlotLocked;

	private InventoryGoodsSlotModel _selectedSlotModel;

	private InventoryGoodsSlotModel _targetSlotModel;

	private DraggingGoodsSlotModel _draggingSlotModel;

	private bool _isHoveringRegisterSlot;

	private EGoodsSlotCategory _sourceSlotCategory;

	private int _sourceSlotIndex;

	private GameInputManager GameInputManager => null;

	public override string OpenSfx => null;

	public override bool IsInputActionPopup => false;

	private InputActions.UiActions UiActions => default;

	private InputActions.UiPadActions UiPadActions => default;

	private PlayerInventory MyInventory => null;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__35))]
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

	private void OnSlotFocused(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnRegisterSlotEnter(GoodsSlotModel slotModel)
	{
	}

	private void OnRegisterSlotExit(GoodsSlotModel slotModel)
	{
	}

	private void OnSlotClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSlotDrag(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSlotEndDrag(InventoryGoodsSlotModel slotModel)
	{
	}

	protected override void Update()
	{
	}

	private void HandleInteraction()
	{
	}

	private void OnRegisterSlotClicked()
	{
	}

	[AsyncStateMachine(typeof(_003CTrySetGoodsAsync_003Ed__50))]
	private UniTask TrySetGoodsAsync(InventoryGoodsSlotModel slotModel)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CTryRegisterItemUsedAsync_003Ed__51))]
	private UniTaskVoid TryRegisterItemUsedAsync()
	{
		return default;
	}

	private void RefreshSelectedSlotModel()
	{
	}

	private void ToggleContainer()
	{
	}

	private void OnBack()
	{
	}
}
