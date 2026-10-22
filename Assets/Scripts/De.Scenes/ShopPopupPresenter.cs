using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Data;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UnityEngine.InputSystem;
using VContainer;

public class ShopPopupPresenter : GamePopupPresenter
{
	private enum EFocusPanel
	{
		None = 0,
		Inventory = 1,
		Stash = 2,
		ShopProduct = 3
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__39 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public ShopPopupPresenter _003C_003E4__this;

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
	private struct _003CShowBuyQuantityPopupAsync_003Ed__59 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<long> _003C_003Et__builder;

		public ShopPopupPresenter _003C_003E4__this;

		public Goods productGoods;

		public long buyCount;

		public long maxBuyCount;

		public Goods consumeGoods;

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
	private struct _003CShowSellQuantityPopupAsync_003Ed__65 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<long> _003C_003Et__builder;

		public GoodsSlot goodsSlot;

		public ShopPopupPresenter _003C_003E4__this;

		public long sellCount;

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
	private struct _003CTryBuyProductAsync_003Ed__57 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

		public ShopPopupPresenter _003C_003E4__this;

		public int productId;

		public bool isBuyMax;

		public bool showQuantityPopup;

		private TShopProduct _003CproductData_003E5__2;

		private long _003CbuyCount_003E5__3;

		private Goods _003CgainGoods_003E5__4;

		private Goods _003CconsumeGoods_003E5__5;

		private UniTask<long>.Awaiter _003C_003Eu__1;

		private UniTask<ResShopBuyGoods>.Awaiter _003C_003Eu__2;

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
	private struct _003CTrySellGoodsAsync_003Ed__63 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

		public ShopPopupPresenter _003C_003E4__this;

		public InventoryGoodsSlotModel slotModel;

		public bool isSellMax;

		public bool showQuantityPopup;

		private long _003CsellCount_003E5__2;

		private Goods _003CsellGoods_003E5__3;

		private UniTask<long>.Awaiter _003C_003Eu__1;

		private UniTask<ResShopSellGoods>.Awaiter _003C_003Eu__2;

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
	private ShopPopupModel _model;

	[View]
	private ShopPopupView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameShopManager _shopManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private GameChannel _gameChannel;

	private EFocusPanel _activePanel;

	private EFocusPanel _focusPanel;

	private ShopProductPanelModel _shopProductPanelModel;

	private PlayerInventoryPanelModel _inventoryPanelModel;

	private StashPanelModel _stashPanelModel;

	private bool _isDragging;

	private InventoryGoodsSlotModel _selectedSlotModel;

	private InventoryGoodsSlotModel _targetSlotModel;

	private DraggingGoodsSlotModel _draggingSlotModel;

	private GameInputManager GameInputManager => null;

	public override string OpenSfx => null;

	public override bool IsInputActionPopup => false;

	private InputActions.UiActions UiActions => default;

	private InputActions.UiPadActions UiPadActions => default;

	private bool IsMaxQuantityPressed => false;

	private PlayerInventory MyInventory => null;

	private bool AllowAccessStash => false;

	private int SourceSlotIndex => 0;

	private int TargetSlotIndex => 0;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__39))]
	protected override UniTask OnOpenAsync()
	{
		return default;
	}

	private void InitGamePadNav()
	{
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}

	private void OnProductSlotLeftClick(ShopProductSlotModel slotModel)
	{
	}

	private void OnProductSlotRightClick(ShopProductSlotModel slotModel)
	{
	}

	protected override void Update()
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

	private void OnSlotLeftClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSlotRightClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnOrganizeButtonClick(InputAction.CallbackContext context)
	{
	}

	private void OnSubmit()
	{
	}

	private void OnQuickSubmit()
	{
	}

	private void OnBack()
	{
	}

	private void FocusPanel(EFocusPanel panel, ENavDir entryDir = ENavDir.None)
	{
	}

	private void ToggleContainer()
	{
	}

	[AsyncStateMachine(typeof(_003CTryBuyProductAsync_003Ed__57))]
	private UniTask<bool> TryBuyProductAsync(int productId, bool isBuyMax, bool showQuantityPopup)
	{
		return default;
	}

	private bool CheckBuyProduct(int productId)
	{
		return false;
	}

	[AsyncStateMachine(typeof(_003CShowBuyQuantityPopupAsync_003Ed__59))]
	private UniTask<long> ShowBuyQuantityPopupAsync(Goods productGoods, Goods consumeGoods, long buyCount, long maxBuyCount)
	{
		return default;
	}

	private static string GetBuySfx(Goods consumeGoods)
	{
		return null;
	}

	private static string GetConsumeGoodsName(Goods consumeGoods)
	{
		return null;
	}

	private long GetMaxBuyCount(int productId)
	{
		return 0L;
	}

	[AsyncStateMachine(typeof(_003CTrySellGoodsAsync_003Ed__63))]
	private UniTask<bool> TrySellGoodsAsync(InventoryGoodsSlotModel slotModel, bool isSellMax, bool showQuantityPopup)
	{
		return default;
	}

	private bool CheckSellable(GoodsSlot goodsSlot)
	{
		return false;
	}

	[AsyncStateMachine(typeof(_003CShowSellQuantityPopupAsync_003Ed__65))]
	private UniTask<long> ShowSellQuantityPopupAsync(GoodsSlot goodsSlot, long sellCount)
	{
		return default;
	}

	private void HandleInteraction()
	{
	}

	private void SwapInventorySlots()
	{
	}

	private void TransferInventorySlotToSafeSlot()
	{
	}

	private void SwapSafeSlots()
	{
	}

	private void TransferSafeSlotToInventorySlot()
	{
	}

	private void SwapStashSlots()
	{
	}
}
