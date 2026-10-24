using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
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

	private EFocusPanel _activePanel = EFocusPanel.Inventory;

	private EFocusPanel _focusPanel = EFocusPanel.Inventory;

	private ShopProductPanelModel _shopProductPanelModel;

	private PlayerInventoryPanelModel _inventoryPanelModel;

	private StashPanelModel _stashPanelModel;

	private bool _isDragging;

	private InventoryGoodsSlotModel _selectedSlotModel;

	private InventoryGoodsSlotModel _targetSlotModel;

	private DraggingGoodsSlotModel _draggingSlotModel;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override string OpenSfx => "InventoryPopupOpen";

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private InputActions.UiPadActions UiPadActions => GameInputManager.UiPadActions;

	private bool IsMaxQuantityPressed => GameInputManager.UiActions.ShopMaxQuantity.IsPressed();

	private PlayerInventory MyInventory => _gameManager.MyInventory;

	private bool AllowAccessStash => _gameManager.IsInLounge;

	private int SourceSlotIndex => _selectedSlotModel?.GoodsSlot?.SlotIndex ?? (-1);

	private int TargetSlotIndex => _targetSlotModel?.GoodsSlot?.SlotIndex ?? (-1);

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.Title_Text.text = _model.Title;
		_view.CurrencyLayout.SetActive(_gameManager.IsInLounge);
		_view.PopupKeyGuide_LShift.SetActive(AllowAccessStash);
		_shopProductPanelModel = new ShopProductPanelModel();
		_shopProductPanelModel.ShopProductGroupId = _model.ShopProductGroupId;
		_shopProductPanelModel.OnSlotLeftClick.Subscribe(OnProductSlotLeftClick).AddTo(this);
		_shopProductPanelModel.OnSlotRightClick.Subscribe(OnProductSlotRightClick).AddTo(this);
		_view.ShopProductPanel.Inject(_shopProductPanelModel);
		_inventoryPanelModel = new PlayerInventoryPanelModel
		{
			PlayerInventory = _gameManager.MyInventory
		};
		_inventoryPanelModel.OnSlotLeftClick.Subscribe(OnSlotLeftClick).AddTo(this);
		_inventoryPanelModel.OnSlotRightClick.Subscribe(OnSlotRightClick).AddTo(this);
		_inventoryPanelModel.RxFocusedSlotModel.Subscribe(OnSlotFocused).AddTo(this);
		_inventoryPanelModel.OnSlotDrag.Subscribe(OnSlotDrag).AddTo(this);
		_inventoryPanelModel.OnSlotEndDrag.Subscribe(OnSlotEndDrag).AddTo(this);
		_inventoryPanelModel.OnSafeSlotLeftClick.Subscribe(OnSlotLeftClick).AddTo(this);
		_inventoryPanelModel.OnSafeSlotRightClick.Subscribe(OnSlotRightClick).AddTo(this);
		_inventoryPanelModel.RxFocusedSafeSlotModel.Subscribe(OnSlotFocused).AddTo(this);
		_inventoryPanelModel.OnSafeSlotDrag.Subscribe(OnSlotDrag).AddTo(this);
		_inventoryPanelModel.OnSafeSlotEndDrag.Subscribe(OnSlotEndDrag).AddTo(this);
		_view.InventoryPanel.Inject(_inventoryPanelModel);
		_stashPanelModel = new StashPanelModel
		{
			EnableFilterButtons = true
		};
		_stashPanelModel.OnSlotLeftClick.Subscribe(OnSlotLeftClick).AddTo(this);
		_stashPanelModel.OnSlotRightClick.Subscribe(OnSlotRightClick).AddTo(this);
		_stashPanelModel.RxFocusedSlotModel.Subscribe(OnSlotFocused).AddTo(this);
		_stashPanelModel.OnSlotDrag.Subscribe(OnSlotDrag).AddTo(this);
		_stashPanelModel.OnSlotEndDrag.Subscribe(OnSlotEndDrag).AddTo(this);
		_view.StashPanel.Inject(_stashPanelModel);
		_view.InventoryPanel.gameObject.SetActive(_activePanel == EFocusPanel.Inventory);
		_view.StashPanel.gameObject.SetActive(_activePanel == EFocusPanel.Stash);
		InitGamePadNav();
	}

	private void InitGamePadNav()
	{
		_inventoryPanelModel.OutOfRight.Subscribe(delegate
		{
			FocusPanel(EFocusPanel.ShopProduct, ENavDir.Right);
		}).AddTo(this);
		_stashPanelModel.OutOfRight.Subscribe(delegate
		{
			FocusPanel(EFocusPanel.ShopProduct, ENavDir.Right);
		}).AddTo(this);
		_shopProductPanelModel.OutOfLeft.Subscribe(delegate
		{
			FocusPanel(_activePanel, ENavDir.Left);
		}).AddTo(this);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		GameInputManager.UiActions.Switch.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			ToggleContainer();
		}).AddTo(InputDisposable);
		GameInputManager.UiActions.Organize.OnClickWithSfx("ButtonClick").Subscribe(OnOrganizeButtonClick).AddTo(InputDisposable);
		GameInputManager.UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnBack();
		}).AddTo(InputDisposable);
		GameInputManager.UiPadActions.Submit.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnSubmit();
		}).AddTo(InputDisposable);
		GameInputManager.UiPadActions.QuickSubmit.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnQuickSubmit();
		}).AddTo(InputDisposable);
		FocusPanel(_focusPanel);
	}

	protected override void OnInputDisabled()
	{
		_inventoryPanelModel.RxIsInputEnabled.Value = false;
		_stashPanelModel.RxIsInputEnabled.Value = false;
		_shopProductPanelModel.RxIsInputEnabled.Value = false;
		base.OnInputDisabled();
	}

	private void OnProductSlotLeftClick(ShopProductSlotModel slotModel)
	{
		TryBuyProductAsync(slotModel.Id, IsMaxQuantityPressed, showQuantityPopup: true).Forget();
	}

	private void OnProductSlotRightClick(ShopProductSlotModel slotModel)
	{
		TryBuyProductAsync(slotModel.Id, IsMaxQuantityPressed, showQuantityPopup: false).Forget();
	}

	protected override void Update()
	{
		base.Update();
		if (_isDragging)
		{
			RectTransform component = _view.DraggingGoodsSlot.GetComponent<RectTransform>();
			RectTransformUtility.ScreenPointToLocalPointInRectangle(_popupManager.OverlayCanvas.transform as RectTransform, Mouse.current.position.ReadValue(), null, out var localPoint);
			component.anchoredPosition = localPoint;
		}
	}

	private void OnSlotFocused(InventoryGoodsSlotModel slotModel)
	{
		if (!_isDragging)
		{
			_selectedSlotModel = slotModel;
		}
		else
		{
			_targetSlotModel = slotModel;
		}
	}

	private void OnSlotDrag(InventoryGoodsSlotModel slotModel)
	{
		if (slotModel != null && !slotModel.IsSlotEmpty)
		{
			_selectedSlotModel = slotModel;
			_isDragging = true;
			_draggingSlotModel = new DraggingGoodsSlotModel(slotModel.GoodsSlot.Goods.Clone());
			_view.DraggingGoodsSlot.gameObject.SetActive(value: true);
			_view.DraggingGoodsSlot.Inject(_draggingSlotModel);
		}
	}

	private void OnSlotEndDrag(InventoryGoodsSlotModel slotModel)
	{
		if (_isDragging)
		{
			_isDragging = false;
			_view.DraggingGoodsSlot.gameObject.SetActive(value: false);
			HandleInteraction();
		}
	}

	private void OnSlotLeftClick(InventoryGoodsSlotModel slotModel)
	{
		TrySellGoodsAsync(slotModel, IsMaxQuantityPressed, showQuantityPopup: true).Forget();
	}

	private void OnSlotRightClick(InventoryGoodsSlotModel slotModel)
	{
		TrySellGoodsAsync(slotModel, IsMaxQuantityPressed, showQuantityPopup: false).Forget();
	}

	private void OnOrganizeButtonClick(InputAction.CallbackContext context)
	{
		BaseMessage message;
		switch (_selectedSlotModel?.SlotCategory ?? EGoodsSlotCategory.None)
		{
		case EGoodsSlotCategory.InventorySlot:
			message = new ReqOrganizeInventorySlots();
			break;
		case EGoodsSlotCategory.SafeSlot:
			message = new ReqOrganizeInventorySafeSlots();
			break;
		case EGoodsSlotCategory.StashSlot:
			_playerManager.OrganizeStash();
			return;
		default:
			return;
		}
		_gameChannel.SendToHost(message);
	}

	private void OnSubmit()
	{
		if (_focusPanel != EFocusPanel.ShopProduct)
		{
			if (_focusPanel == EFocusPanel.Inventory || _focusPanel == EFocusPanel.Stash)
			{
				if (_selectedSlotModel != null)
				{
					TrySellGoodsAsync(_selectedSlotModel, IsMaxQuantityPressed, showQuantityPopup: true).Forget();
				}
			}
			return;
		}
		ShopProductSlotModel focusedSlotModel = _shopProductPanelModel.FocusedSlotModel;
		if (focusedSlotModel != null)
		{
			TryBuyProductAsync(focusedSlotModel.Id, IsMaxQuantityPressed, showQuantityPopup: true).Forget();
		}
	}

	private void OnQuickSubmit()
	{
		if (_focusPanel != EFocusPanel.ShopProduct)
		{
			if (_focusPanel == EFocusPanel.Inventory || _focusPanel == EFocusPanel.Stash)
			{
				if (_selectedSlotModel != null)
				{
					TrySellGoodsAsync(_selectedSlotModel, IsMaxQuantityPressed, showQuantityPopup: false).Forget();
				}
			}
			return;
		}
		ShopProductSlotModel focusedSlotModel = _shopProductPanelModel.FocusedSlotModel;
		if (focusedSlotModel != null)
		{
			TryBuyProductAsync(focusedSlotModel.Id, IsMaxQuantityPressed, showQuantityPopup: false).Forget();
		}
	}

	private void OnBack()
	{
		if (!_isDragging)
		{
			Close();
			return;
		}
		_isDragging = false;
		_view.DraggingGoodsSlot.gameObject.SetActive(value: false);
		if (_targetSlotModel != null)
		{
			_selectedSlotModel = _targetSlotModel;
		}
		_targetSlotModel = null;
	}

	private void FocusPanel(EFocusPanel panel, ENavDir entryDir = ENavDir.None)
	{
		if (panel != EFocusPanel.None)
		{
			_focusPanel = panel;
			BasePanelModel basePanelModel;
			switch (panel)
			{
			case EFocusPanel.Inventory:
				basePanelModel = _inventoryPanelModel;
				break;
			case EFocusPanel.Stash:
				basePanelModel = _stashPanelModel;
				break;
			case EFocusPanel.ShopProduct:
				basePanelModel = _shopProductPanelModel;
				break;
			default:
				basePanelModel = null;
				break;
			}
			if (basePanelModel != null)
			{
				basePanelModel.EntryDirection = entryDir;
			}
		}
		_inventoryPanelModel.RxIsInputEnabled.Value = panel == EFocusPanel.Inventory;
		_stashPanelModel.RxIsInputEnabled.Value = panel == EFocusPanel.Stash;
		_shopProductPanelModel.RxIsInputEnabled.Value = panel == EFocusPanel.ShopProduct;
	}

	private void ToggleContainer()
	{
		if (!_gameManager.IsInLounge)
		{
			return;
		}
		if (_isDragging)
		{
			_isDragging = false;
			_view.DraggingGoodsSlot.gameObject.SetActive(value: false);
			_selectedSlotModel = null;
			_targetSlotModel = null;
		}
		_activePanel = ((_activePanel == EFocusPanel.Inventory) ? EFocusPanel.Stash : EFocusPanel.Inventory);
		_view.InventoryPanel.gameObject.SetActive(_activePanel == EFocusPanel.Inventory);
		_view.StashPanel.gameObject.SetActive(_activePanel == EFocusPanel.Stash);
		if (_focusPanel != EFocusPanel.ShopProduct)
		{
			FocusPanel(_activePanel);
		}
	}

	private async UniTask<bool> TryBuyProductAsync(int productId, bool isBuyMax, bool showQuantityPopup)
	{
		if (!CheckBuyProduct(productId))
		{
			return false;
		}
		TShopProduct productData = TShopProduct.Table[productId];
		long maxBuyCount = GetMaxBuyCount(productId);
		long buyCount = (isBuyMax ? maxBuyCount : 1);
		Goods gainGoods = productData.GainGoodsData.ToGoodsAsBase();
		Goods consumeGoods = productData.ConsumeGoodsData.ToGoodsAsBase();
		if (showQuantityPopup)
		{
			buyCount = await ShowBuyQuantityPopupAsync(gainGoods, consumeGoods, buyCount, maxBuyCount);
		}
		if (buyCount < 1)
		{
			return false;
		}
		if (!_shopManager.HasEnoughShopProductStock(productId, (int)buyCount))
		{
			_toastManager.NoticeFail(TLocalizedText.NotEnoughStockCount.LocalText);
			return false;
		}
		string bagConsumeFailToast = _playerManager.GetBagConsumeFailToast(new Goods[1] { GoodsCreator.MakeFromGoodsData(productData.ConsumeGoodsData, productData.ConsumeGoodsData.Count * buyCount, 0L) });
		if (bagConsumeFailToast != null)
		{
			_toastManager.NoticeFail(bagConsumeFailToast);
			return false;
		}
		ResShopBuyGoods resShopBuyGoods = await _gameChannel.RequestAsync<ResShopBuyGoods>(new ReqShopBuyGoods
		{
			ProductId = productId,
			BuyCount = (int)buyCount,
			DestinationSlotCategory = ((_activePanel == EFocusPanel.Stash) ? EGoodsSlotCategory.StashSlot : EGoodsSlotCategory.InventorySlot)
		});
		if (resShopBuyGoods == null || !resShopBuyGoods.IsSuccess)
		{
			_toastManager.NoticeFail(TLocalizedText.FailedBuyShopProudct.LocalText);
			return false;
		}
		string name = gainGoods.GetName();
		long count = consumeGoods.Count;
		_soundManager.PlaySfx(GetBuySfx(consumeGoods));
		_toastManager.Notice(TLocalizedText.ItemPurchasedFormat.Format(name, buyCount, GetConsumeGoodsName(consumeGoods), count * buyCount));
		return true;
	}

	private bool CheckBuyProduct(int productId)
	{
		string failMessage = null;
		TShopProduct tShopProduct = TShopProduct.Table[productId];
		Goods goods = tShopProduct.GainGoodsData.ToGoodsAsBase();
		Goods goods2 = tShopProduct.ConsumeGoodsData.ToGoodsAsBase();
		if (!_shopManager.HasEnoughShopProductStock(productId, 1))
		{
			_toastManager.NoticeFail(TLocalizedText.NotEnoughStockCount.LocalText);
			return false;
		}
		if (!_playerManager.HasEnoughGoods(goods2))
		{
			_toastManager.NoticeFail(TLocalizedText.NotEnoughFunds.LocalText);
			return false;
		}
		if (_activePanel != EFocusPanel.Stash)
		{
			if (!_gameManager.MyInventory.CanAddGoods(goods, out failMessage))
			{
				_toastManager.NoticeFail(failMessage);
				return false;
			}
			return true;
		}
		if (!_playerManager.CanAddGoodsToStash(goods))
		{
			_toastManager.NoticeFail(TLocalizedText.NotEnoughStashSlots.LocalText);
			return false;
		}
		return true;
	}

	private async UniTask<long> ShowBuyQuantityPopupAsync(Goods productGoods, Goods consumeGoods, long buyCount, long maxBuyCount)
	{
		GoodsQuantityPopupPresenter goodsQuantityPopupPresenter = await _popupManager.OpenAsync<GoodsQuantityPopupPresenter>(new GoodsQuantityPopupModel
		{
			Message = TLocalizedText.SelectPurchaseQuantity.LocalText,
			Goods = productGoods,
			Count = buyCount,
			CountMax = maxBuyCount,
			GoldPrice = consumeGoods.Count,
			ShowGoldPrice = true
		});
		if (goodsQuantityPopupPresenter == null)
		{
			return 0L;
		}
		object obj = await goodsQuantityPopupPresenter.Task;
		if (obj is long num && num > 0)
		{
			return num;
		}
		return 0L;
	}

	private static string GetBuySfx(Goods consumeGoods)
	{
		switch (consumeGoods.Type)
		{
		case EGoodsType.Gold:
			return "Casher";
		case EGoodsType.Coin:
			return "Casher_Coin_Online";
		case EGoodsType.Misc:
			if (consumeGoods.Id == 10001)
			{
				return "Casher_Coin_Actual";
			}
			return "Casher_Etc";
		default:
			return "Casher_Etc";
		}
	}

	private static string GetConsumeGoodsName(Goods consumeGoods)
	{
		EGoodsCategory eGoodsCategory = consumeGoods.Type.GoodsCategory();
		switch (eGoodsCategory)
		{
		case EGoodsCategory.Item:
			return TItem.Table[consumeGoods.Id].Name;
		case EGoodsCategory.Equipment:
			return LocalizationUtility.GetLocalizedText(string.Format("{0}_{1}_{2}", "TEquipment", "Name", TEquipment.Table[consumeGoods.Id].Id));
		case EGoodsCategory.Bag:
			return LocalizationUtility.GetLocalizedText(string.Format("{0}_{1}_{2}", "TBag", "Name", TBag.Table[consumeGoods.Id].Id));
		default:
			return consumeGoods.Type.ToLocalText();
		}
	}

	private long GetMaxBuyCount(int productId)
	{
		TShopProduct tShopProduct = TShopProduct.Table[productId];
		Goods goods = tShopProduct.ConsumeGoodsData.ToGoodsAsBase();
		long totalGoodsCount = _playerManager.GetTotalGoodsCount(goods);
		int num = ((goods.Count > 0) ? System.Math.Min((int)(totalGoodsCount / goods.Count), 99) : 99);
		int shopProductStock = _shopManager.GetShopProductStock(productId);
		if (shopProductStock >= 0)
		{
			num = System.Math.Min(num, shopProductStock);
		}
		EGoodsCategory category = tShopProduct.GainGoodsData.Category;
		int num2 = ((category != EGoodsCategory.Equipment && category != EGoodsCategory.Bag) ? ((int)_gameManager.MyInventory.GetAddableItemCountWithBags(tShopProduct.GainGoodsData.Id)) : 1);
		return System.Math.Min(num, num2);
	}

	private async UniTask<bool> TrySellGoodsAsync(InventoryGoodsSlotModel slotModel, bool isSellMax, bool showQuantityPopup)
	{
		if (!CheckSellable(slotModel.GoodsSlot))
		{
			return false;
		}
		long sellCount = (isSellMax ? slotModel.GoodsSlot.Count : 1);
		if (showQuantityPopup)
		{
			sellCount = await ShowSellQuantityPopupAsync(slotModel.GoodsSlot, sellCount);
		}
		if (sellCount < 1)
		{
			return false;
		}
		Goods sellGoods = GoodsCreator.MakeGoods(slotModel.GoodsType, slotModel.GoodsId, sellCount, 0L);
		ResShopSellGoods resShopSellGoods = await _gameChannel.RequestAsync<ResShopSellGoods>(new ReqShopSellGoods
		{
			SlotCategory = slotModel.SlotCategory,
			SlotIndex = slotModel.SlotIndex,
			SellGoods = sellGoods
		});
		if (resShopSellGoods == null || !resShopSellGoods.IsSuccess)
		{
			return false;
		}
		_soundManager.PlaySfx("Casher");
		_toastManager.Notice(TLocalizedText.ItemSoldFormat.Format(sellGoods.GetName(), sellCount, TLocalizedText.EGoodsTypeGold.LocalText, resShopSellGoods.GainGold));
		return true;
	}

	private bool CheckSellable(GoodsSlot goodsSlot)
	{
		if (goodsSlot != null && !goodsSlot.IsEmpty)
		{
			if (goodsSlot.Goods.IsArtifact())
			{
				_toastManager.NoticeFail(TLocalizedText.CannotSellArtifact.LocalText);
				return false;
			}
			if (goodsSlot.Goods is BagGoods { OccupiedSlotCount: >0 })
			{
				_toastManager.NoticeFail(TLocalizedText.CannotSellOccupiedBag.LocalText);
				return false;
			}
			if (goodsSlot.Count > 0)
			{
				return true;
			}
			_toastManager.NoticeFail(TLocalizedText.NoItemsToSell.LocalText);
			return false;
		}
		_toastManager.NoticeFail(TLocalizedText.SelectItem.LocalText);
		return false;
	}

	private async UniTask<long> ShowSellQuantityPopupAsync(GoodsSlot goodsSlot, long sellCount)
	{
		int sellPriceEach = goodsSlot.Goods.SellPriceEach;
		Goods goods = GoodsCreator.MakeGoods(goodsSlot.Goods.Type, goodsSlot.Id, 0L, 0L);
		GoodsQuantityPopupPresenter goodsQuantityPopupPresenter = await _popupManager.OpenAsync<GoodsQuantityPopupPresenter>(new GoodsQuantityPopupModel
		{
			Message = TLocalizedText.SelectQuantityToSell.LocalText,
			Goods = goods,
			Count = sellCount,
			CountMax = goodsSlot.Count,
			GoldPrice = sellPriceEach,
			ShowGoldPrice = true
		});
		if (goodsQuantityPopupPresenter == null)
		{
			return 0L;
		}
		object obj = await goodsQuantityPopupPresenter.Task;
		if (obj is long num && num > 0)
		{
			return num;
		}
		return 0L;
	}

	private void HandleInteraction()
	{
		if (_selectedSlotModel == _targetSlotModel || _selectedSlotModel == null || _targetSlotModel == null)
		{
			_selectedSlotModel = null;
			return;
		}
		EGoodsSlotCategory slotCategory = _targetSlotModel.SlotCategory;
		switch (_selectedSlotModel.SlotCategory)
		{
		case EGoodsSlotCategory.InventorySlot:
			switch (slotCategory)
			{
			case EGoodsSlotCategory.InventorySlot:
				SwapInventorySlots();
				break;
			case EGoodsSlotCategory.SafeSlot:
				TransferInventorySlotToSafeSlot();
				break;
			}
			break;
		case EGoodsSlotCategory.SafeSlot:
			switch (slotCategory)
			{
			case EGoodsSlotCategory.SafeSlot:
				SwapSafeSlots();
				break;
			case EGoodsSlotCategory.InventorySlot:
				TransferSafeSlotToInventorySlot();
				break;
			}
			break;
		case EGoodsSlotCategory.StashSlot:
			if (slotCategory == EGoodsSlotCategory.StashSlot)
			{
				SwapStashSlots();
			}
			break;
		}
		_selectedSlotModel = null;
		_targetSlotModel = null;
	}

	private void SwapInventorySlots()
	{
		if (_selectedSlotModel != null && _selectedSlotModel.GoodsSlot != null && !_selectedSlotModel.GoodsSlot.IsEmpty)
		{
			_gameChannel.SendToHost(new ReqSwapInventorySlots
			{
				SourceSlotIndex = SourceSlotIndex,
				TargetSlotIndex = TargetSlotIndex
			});
			_soundManager.PlaySfx("ItemRelease");
		}
	}

	private void TransferInventorySlotToSafeSlot()
	{
		if (_selectedSlotModel != null && _selectedSlotModel.GoodsSlot != null && !_selectedSlotModel.GoodsSlot.IsEmpty && _targetSlotModel != null)
		{
			_gameChannel.SendToHost(new ReqInventorySlotToSafeSlot
			{
				InventorySlotIndex = SourceSlotIndex,
				SafeSlotIndex = TargetSlotIndex
			});
			_soundManager.PlaySfx("ItemRelease");
		}
	}

	private void SwapSafeSlots()
	{
		if (_selectedSlotModel != null && _selectedSlotModel.GoodsSlot != null && !_selectedSlotModel.GoodsSlot.IsEmpty)
		{
			_gameChannel.SendToHost(new ReqSwapSafeSlots
			{
				SourceSlotIndex = SourceSlotIndex,
				TargetSlotIndex = TargetSlotIndex
			});
			_soundManager.PlaySfx("ItemRelease");
		}
	}

	private void TransferSafeSlotToInventorySlot()
	{
		if (_selectedSlotModel != null && _selectedSlotModel.GoodsSlot != null && !_selectedSlotModel.GoodsSlot.IsEmpty && _targetSlotModel != null)
		{
			_gameChannel.SendToHost(new ReqSafeSlotToInventorySlot
			{
				SafeSlotIndex = SourceSlotIndex,
				InventorySlotIndex = TargetSlotIndex
			});
			_soundManager.PlaySfx("ItemRelease");
		}
	}

	private void SwapStashSlots()
	{
		if (_selectedSlotModel != null && _selectedSlotModel.GoodsSlot != null && !_selectedSlotModel.GoodsSlot.IsEmpty)
		{
			_playerManager.StashSlots.SwapIndex(SourceSlotIndex, TargetSlotIndex);
			_soundManager.PlaySfx("ItemRelease");
		}
	}
}
