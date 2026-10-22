using System.Collections.Generic;
using System.Linq;
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

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override string OpenSfx => "NPC_UI_Open_ETC";

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private InputActions.UiPadActions UiPadActions => GameInputManager.UiPadActions;

	private PlayerInventory MyInventory => _gameManager.MyInventory;

	private bool AllowAccessStash => _gameManager.IsInLounge;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_scrapPanelModel = new ScrapPanelModel();
		_scrapPanelModel.OnScrapSlotEnter.Subscribe(OnScrapSlotEnter).AddTo(this);
		_scrapPanelModel.OnScrapSlotExit.Subscribe(OnScrapSlotExit).AddTo(this);
		_scrapPanelModel.OnScrapButtonClick.Subscribe(delegate
		{
			TryScrapAsync().Forget();
		}).AddTo(this);
		_scrapPanelModel.OnScrapSlotClick.Subscribe(delegate
		{
			OnScrapSlotClick();
		}).AddTo(this);
		_view.ScrapPanel.Inject(_scrapPanelModel);
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
		_view.MyInventory.Inject(_inventoryPanelModel);
		if (_gameManager.IsInLounge)
		{
			_view.StashPanel.gameObject.SetActive(value: true);
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
		}
		_itemPanelType = EItemPanelType.Inventory;
		_view.MyInventory.gameObject.SetActive(value: true);
		_view.StashPanel.gameObject.SetActive(value: false);
		_view.KeyGuide_LShift.SetActive(_gameManager.IsInLounge);
		_playerManager.RxProfile.Subscribe(delegate
		{
			OnUserDataChanged();
		}).AddTo(this);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Switch.OnClickWithSfx().Subscribe(delegate
		{
			ToggleContainer();
		}).AddTo(InputDisposable);
		UiActions.Organize.OnClickWithSfx().Subscribe(OnOrganizeButtonClick).AddTo(InputDisposable);
		UiActions.Escape.OnClickWithSfx().Subscribe(delegate
		{
			OnBack();
		}).AddTo(InputDisposable);
		UiPadActions.Submit.OnClickWithSfx().Subscribe(delegate
		{
			OnSubmit();
		}).AddTo(InputDisposable);
		UiPadActions.QuickSubmit.OnClickWithSfx().Subscribe(delegate
		{
			OnQuickSubmit();
		}).AddTo(InputDisposable);
		FocusPanel(_focusPanel);
	}

	protected override void OnInputDisabled()
	{
		DisableAllPanelInput();
		base.OnInputDisabled();
	}

	private void FocusPanel(EFocusPanel panel)
	{
		DisableAllPanelInput();
		GameInputManager.ClearSelectedPosition();
		_focusPanel = panel;
		BasePanelModel basePanelModel;
		switch (panel)
		{
		case EFocusPanel.Left:
			basePanelModel = ((_itemPanelType == EItemPanelType.Inventory && _inventoryPanelModel != null) ? _inventoryPanelModel : _stashPanelModel);
			break;
		case EFocusPanel.Scrap:
			basePanelModel = _scrapPanelModel;
			break;
		default:
			return;
		}
		basePanelModel.RxIsInputEnabled.Value = true;
	}

	private void DisableAllPanelInput()
	{
		if (_inventoryPanelModel != null)
		{
			_inventoryPanelModel.RxIsInputEnabled.Value = false;
		}
		if (_stashPanelModel != null)
		{
			_stashPanelModel.RxIsInputEnabled.Value = false;
		}
		_scrapPanelModel.RxIsInputEnabled.Value = false;
	}

	private void OnSubmit()
	{
		if (_focusPanel != EFocusPanel.Scrap)
		{
			TrySetScrapItemAsync(_selectedSlotModel, showQuantityPopup: true).Forget();
		}
	}

	private void OnQuickSubmit()
	{
		if (_focusPanel != EFocusPanel.Scrap)
		{
			TrySetScrapItemAsync(_selectedSlotModel, showQuantityPopup: false).Forget();
		}
	}

	private void OnUserDataChanged()
	{
		_view.Gold_Text.GetOrAddComponent<ValueTween>().SetValue(_playerManager.Gold);
	}

	private void OnSlotFocused(InventoryGoodsSlotModel slotModel)
	{
		if (!_isSelectedSlotLocked)
		{
			_selectedSlotModel = slotModel;
		}
		else
		{
			_targetSlotModel = slotModel;
		}
	}

	private void OnScrapSlotEnter(InventoryGoodsSlotModel slotModel)
	{
		if (_isSelectedSlotLocked)
		{
			_targetSlotModel = slotModel;
		}
	}

	private void OnScrapSlotExit(InventoryGoodsSlotModel slotModel)
	{
		if (_isSelectedSlotLocked && _targetSlotModel == slotModel)
		{
			_targetSlotModel = null;
		}
	}

	private void OnSlotLeftClick(InventoryGoodsSlotModel slotModel)
	{
		TrySetScrapItemAsync(slotModel, showQuantityPopup: true).Forget();
	}

	private void OnSlotRightClick(InventoryGoodsSlotModel slotModel)
	{
		if (!_isDragging)
		{
			TrySetScrapItemAsync(slotModel, showQuantityPopup: false).Forget();
		}
	}

	private void OnSlotDrag(InventoryGoodsSlotModel slotModel)
	{
		if (slotModel != null && !slotModel.IsSlotEmpty)
		{
			_selectedSlotModel = slotModel;
			_isDragging = true;
			_isSelectedSlotLocked = true;
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
			_isSelectedSlotLocked = false;
			HandleInteraction();
			_targetSlotModel = null;
		}
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

	private void HandleInteraction()
	{
		if (_selectedSlotModel != null && _targetSlotModel != null && _targetSlotModel == _scrapPanelModel.ScrapSlotModel)
		{
			TrySetScrapItemAsync(_selectedSlotModel, showQuantityPopup: true).Forget();
		}
		_selectedSlotModel = null;
	}

	private void OnScrapSlotClick()
	{
		if (_scrapPanelModel.RxScrapData.Value != null)
		{
			_scrapPanelModel.Clear();
			FocusPanel(EFocusPanel.Left);
		}
	}

	private TScrap FindScrapData(Goods goods)
	{
		return TScrap.Table.Values.FirstOrDefault((TScrap s) => s.InsertGoodsData.Type == goods.Type && s.InsertGoodsData.Id == goods.Id);
	}

	private async UniTask TrySetScrapItemAsync(InventoryGoodsSlotModel slotModel, bool showQuantityPopup)
	{
		if (slotModel == null || slotModel.IsSlotEmpty)
		{
			return;
		}
		Goods goods = slotModel.GoodsSlot.Goods;
		if (goods is BagGoods bagGoods && bagGoods.OccupiedSlotCount > 0)
		{
			_toastManager.NoticeFail(TLocalizedText.CannotScrapOccupiedBag.LocalText);
			return;
		}
		TScrap scrapData = FindScrapData(goods);
		if (scrapData == null)
		{
			_toastManager.NoticeFail(TLocalizedText.InvalidScrapType.LocalText);
			return;
		}
		long maxCount = goods.Count / scrapData.InsertGoodsData.Count;
		if (maxCount <= 0)
		{
			_toastManager.NoticeFail(TLocalizedText.NotEnoughScrapGoods.LocalText);
			return;
		}
		long count = 1L;
		if (showQuantityPopup && maxCount > 1)
		{
			count = await ShowScrapQuantityPopupAsync(goods, maxCount);
		}
		_scrapPanelModel.SetScrapInfo(scrapData, count, goods);
		_scrapPanelModel.RxIsInvalid.Value = !CanAcceptScrapResults(scrapData, count);
		_scrapSourceSlotCategory = slotModel.SlotCategory;
		_scrapSourceSlotIndex = slotModel.SlotIndex;
		if (InputManager.IsGamePad)
		{
			await UniTask.NextFrame();
		}
		FocusPanel(EFocusPanel.Scrap);
	}

	private async UniTask<long> ShowScrapQuantityPopupAsync(Goods goods, long maxCount)
	{
		GoodsQuantityPopupPresenter goodsQuantityPopupPresenter = await _popupManager.OpenAsync<GoodsQuantityPopupPresenter>(new GoodsQuantityPopupModel
		{
			Message = TLocalizedText.SelecScrapQuantity.LocalText,
			Goods = goods,
			Count = 1L,
			CountMax = maxCount
		});
		if (goodsQuantityPopupPresenter == null)
		{
			return 0L;
		}
		object obj = await goodsQuantityPopupPresenter.Task;
		return (obj is long num) ? num : 0;
	}

	private bool CanAcceptScrapResults(TScrap scrapData, long count)
	{
		int inventoryEmptySlots = _gameManager.MyInventory.GetEmptySlotCount();
		int stashEmptySlots = (_gameManager.IsInLounge ? _playerManager.GetStashEmptySlotCount() : 0);
		Dictionary<int, long> dictionary = new Dictionary<int, long>();
		long num = 0L;
		if (scrapData != null && scrapData.ResultGoodsData != null)
		{
			GoodsData[] resultGoodsData = scrapData.ResultGoodsData;
			for (int i = 0; i < resultGoodsData.Length; i++)
			{
				GoodsData goodsData = resultGoodsData[i];
				long num2 = goodsData.Count * count;
				switch (goodsData.Category)
				{
				case EGoodsCategory.Currency:
					continue;
				case EGoodsCategory.Item:
					dictionary.TryGetValue(goodsData.Id, out var value);
					dictionary[goodsData.Id] = value + num2;
					continue;
				}
				num += num2;
			}
		}
		foreach (KeyValuePair<int, long> item in dictionary)
		{
			int key = item.Key;
			long num3 = item.Value;
			long addableItemCount = _gameManager.MyInventory.GetAddableItemCount(key);
			bool flag = _gameManager.MyInventory.Slots.HasItem(key);
			if (addableItemCount > 0)
			{
				if (!flag)
				{
					if (inventoryEmptySlots < 1)
					{
						goto IL_remaining;
					}
					inventoryEmptySlots--;
				}
				long num4 = System.Math.Min(num3, addableItemCount);
				num3 -= num4;
			}
			goto IL_remaining;
		IL_remaining:
			if (num3 < 1)
			{
				continue;
			}
			if (_gameManager.IsInLounge && _playerManager.StashSlots.HasItem(key))
			{
				continue;
			}
			if (stashEmptySlots < 1)
			{
				return false;
			}
			stashEmptySlots--;
		}
		return TryConsumeEmptySlots(num);
		bool TryConsumeEmptySlots(long slotCount)
		{
			long num5 = System.Math.Min(slotCount, inventoryEmptySlots);
			inventoryEmptySlots -= (int)num5;
			long num6 = System.Math.Min(slotCount - num5, stashEmptySlots);
			stashEmptySlots -= (int)num6;
			return slotCount - num5 - num6 < 1;
		}
	}

	private async UniTaskVoid TryScrapAsync()
	{
		TScrap value = _scrapPanelModel.RxScrapData.Value;
		long value2 = _scrapPanelModel.RxScrapCount.Value;
		if (!CanAcceptScrapResults(value, value2))
		{
			_toastManager.NoticeFail(TLocalizedText.InvalidScrapSpace.LocalText);
			return;
		}
		ResScrapGoods resScrapGoods = await _gameChannel.RequestAsync<ResScrapGoods>(new ReqScrapGoods
		{
			ScrapId = value.Id,
			Count = (int)value2,
			SlotCategory = _scrapSourceSlotCategory,
			SlotIndex = _scrapSourceSlotIndex
		});
		if (resScrapGoods == null || !resScrapGoods.IsSuccess)
		{
			_toastManager.NoticeFail(TLocalizedText.ScrapFail.LocalText);
			return;
		}
		_soundManager.PlaySfx("Scraping");
		_toastManager.Notice(TLocalizedText.ScrapSuccess.LocalText);
		_scrapPanelModel.Clear();
		if (InputManager.IsGamePad)
		{
			await UniTask.DelayFrame(2);
		}
		RefreshSelectedSlotModel();
		FocusPanel(EFocusPanel.Left);
	}

	private void RefreshSelectedSlotModel()
	{
		InventoryGoodsSlotModel value = ((_itemPanelType == EItemPanelType.Inventory) ? _inventoryPanelModel.RxFocusedSlotModel : _stashPanelModel.RxFocusedSlotModel).Value;
		if (value != null)
		{
			_selectedSlotModel = value;
		}
	}

	private void ToggleContainer()
	{
		if (!_gameManager.IsInLounge || _focusPanel != EFocusPanel.Left)
		{
			return;
		}
		if (_isDragging)
		{
			_isDragging = false;
			_view.DraggingGoodsSlot.gameObject.SetActive(value: false);
			_isSelectedSlotLocked = false;
			_selectedSlotModel = null;
			_targetSlotModel = null;
		}
		_itemPanelType = ((_itemPanelType == EItemPanelType.Inventory) ? EItemPanelType.Stash : EItemPanelType.Inventory);
		_view.MyInventory.gameObject.SetActive(_itemPanelType == EItemPanelType.Inventory);
		_view.StashPanel.gameObject.SetActive(_itemPanelType == EItemPanelType.Stash);
		FocusPanel(_focusPanel);
	}

	private void OnOrganizeButtonClick(InputAction.CallbackContext context)
	{
		BaseMessage message;
		switch (_selectedSlotModel.SlotCategory)
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

	private void OnBack()
	{
		if (_focusPanel != EFocusPanel.Scrap)
		{
			Close();
			return;
		}
		_scrapPanelModel.Clear();
		FocusPanel(EFocusPanel.Left);
	}
}
