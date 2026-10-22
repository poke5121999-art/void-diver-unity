using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UnityEngine.InputSystem;
using VContainer;

public class ScrapPopupPresenter : GamePopupPresenter
{
	private enum EItemPanelType
	{
		Inventory = 0,
		Stash = 1
	}

	private enum EFocusPanel
	{
		Left = 0,
		Scrap = 1
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__36 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public ScrapPopupPresenter _003C_003E4__this;

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
	private struct _003CShowScrapQuantityPopupAsync_003Ed__56 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<long> _003C_003Et__builder;

		public ScrapPopupPresenter _003C_003E4__this;

		public Goods goods;

		public long maxCount;

		private UniTask<GoodsQuantityPopupPresenter>.Awaiter _003C_003Eu__1;

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
	private struct _003CTryScrapAsync_003Ed__58 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public ScrapPopupPresenter _003C_003E4__this;

		private UniTask<ResScrapGoods>.Awaiter _003C_003Eu__1;

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
	private struct _003CTrySetScrapItemAsync_003Ed__55 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public InventoryGoodsSlotModel slotModel;

		public ScrapPopupPresenter _003C_003E4__this;

		public bool showQuantityPopup;

		private Goods _003Cgoods_003E5__2;

		private TScrap _003CscrapData_003E5__3;

		private UniTask<long>.Awaiter _003C_003Eu__1;

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

	[InjectModel]
	private ScrapPopupModel _model;

	[View]
	private ScrapPopupView _view;

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

	private ScrapPanelModel _scrapPanelModel;

	private bool _isDragging;

	private bool _isSelectedSlotLocked;

	private InventoryGoodsSlotModel _selectedSlotModel;

	private InventoryGoodsSlotModel _targetSlotModel;

	private DraggingGoodsSlotModel _draggingSlotModel;

	private EGoodsSlotCategory _scrapSourceSlotCategory;

	private int _scrapSourceSlotIndex;

	private GameInputManager GameInputManager => null;

	public override string OpenSfx => null;

	public override bool IsInputActionPopup => false;

	private InputActions.UiActions UiActions => default;

	private InputActions.UiPadActions UiPadActions => default;

	private PlayerInventory MyInventory => null;

	private bool AllowAccessStash => false;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__36))]
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

	private void OnQuickSubmit()
	{
	}

	private void OnUserDataChanged()
	{
	}

	private void OnSlotFocused(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnScrapSlotEnter(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnScrapSlotExit(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSlotLeftClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSlotRightClick(InventoryGoodsSlotModel slotModel)
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

	private void OnScrapSlotClick()
	{
	}

	private TScrap FindScrapData(Goods goods)
	{
		return null;
	}

	[AsyncStateMachine(typeof(_003CTrySetScrapItemAsync_003Ed__55))]
	private UniTask TrySetScrapItemAsync(InventoryGoodsSlotModel slotModel, bool showQuantityPopup)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CShowScrapQuantityPopupAsync_003Ed__56))]
	private UniTask<long> ShowScrapQuantityPopupAsync(Goods goods, long maxCount)
	{
		return default;
	}

	private bool CanAcceptScrapResults(TScrap scrapData, long count)
	{
		return false;
	}

	[AsyncStateMachine(typeof(_003CTryScrapAsync_003Ed__58))]
	private UniTaskVoid TryScrapAsync()
	{
		return default;
	}

	private void RefreshSelectedSlotModel()
	{
	}

	private void ToggleContainer()
	{
	}

	private void OnOrganizeButtonClick(InputAction.CallbackContext context)
	{
	}

	private void OnBack()
	{
	}
}
