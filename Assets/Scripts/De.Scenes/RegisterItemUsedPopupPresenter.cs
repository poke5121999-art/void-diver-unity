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

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override string OpenSfx => "InventoryPopupOpen";

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private InputActions.UiPadActions UiPadActions => GameInputManager.UiPadActions;

	private PlayerInventory MyInventory => _gameManager.MyInventory;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		string text = TLocalizedText.RegisterFormat.Format(_model.RegisterableType.ToLocalText());
		_view.Title_TitleText.text = text;
		_panelModel = new RegisterItemUsedPanelModel
		{
			Title = text
		};
		_panelModel.OnSlotEnter.Subscribe(OnRegisterSlotEnter).AddTo(OpenDisposable);
		_panelModel.OnSlotExit.Subscribe(OnRegisterSlotExit).AddTo(OpenDisposable);
		_panelModel.OnConfirmButtonClick.Subscribe(delegate
		{
			TryRegisterItemUsedAsync().Forget();
		}).AddTo(OpenDisposable);
		_panelModel.OnSlotClick.Subscribe(delegate
		{
			OnRegisterSlotClicked();
		}).AddTo(OpenDisposable);
		_view.RegisterItemUsedPanel.Inject(_panelModel);
		_inventoryPanelModel = new PlayerInventoryPanelModel
		{
			PlayerInventory = _gameManager.MyInventory,
			AllowedSubTypes = new EGoodsType[1] { _model.RegisterableType }
		};
		_inventoryPanelModel.OnSlotLeftClick.Subscribe(OnSlotClick).AddTo(OpenDisposable);
		_inventoryPanelModel.RxFocusedSlotModel.Subscribe(OnSlotFocused).AddTo(OpenDisposable);
		_inventoryPanelModel.OnSlotDrag.Subscribe(OnSlotDrag).AddTo(OpenDisposable);
		_inventoryPanelModel.OnSlotEndDrag.Subscribe(OnSlotEndDrag).AddTo(OpenDisposable);
		_inventoryPanelModel.OnSafeSlotLeftClick.Subscribe(OnSlotClick).AddTo(OpenDisposable);
		_inventoryPanelModel.RxFocusedSafeSlotModel.Subscribe(OnSlotFocused).AddTo(OpenDisposable);
		_inventoryPanelModel.OnSafeSlotDrag.Subscribe(OnSlotDrag).AddTo(OpenDisposable);
		_inventoryPanelModel.OnSafeSlotEndDrag.Subscribe(OnSlotEndDrag).AddTo(OpenDisposable);
		_view.MyInventory.Inject(_inventoryPanelModel);
		_view.StashPanel.gameObject.SetActive(value: true);
		_stashPanelModel = new StashPanelModel
		{
			EnableFilterButtons = false,
			AllowedSubtypes = new EGoodsType[1] { _model.RegisterableType }
		};
		_stashPanelModel.OnSlotLeftClick.Subscribe(OnSlotClick).AddTo(OpenDisposable);
		_stashPanelModel.RxFocusedSlotModel.Subscribe(OnSlotFocused).AddTo(OpenDisposable);
		_stashPanelModel.OnSlotDrag.Subscribe(OnSlotDrag).AddTo(OpenDisposable);
		_stashPanelModel.OnSlotEndDrag.Subscribe(OnSlotEndDrag).AddTo(OpenDisposable);
		_view.StashPanel.Inject(_stashPanelModel);
		_itemPanelType = EItemPanelType.Inventory;
		_view.MyInventory.gameObject.SetActive(value: true);
		_view.StashPanel.gameObject.SetActive(value: false);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Switch.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			ToggleContainer();
		}).AddTo(InputDisposable);
		UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnBack();
		}).AddTo(InputDisposable);
		UiPadActions.Submit.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnSubmit();
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
		case EFocusPanel.Register:
			basePanelModel = _panelModel;
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
		_panelModel.RxIsInputEnabled.Value = false;
	}

	private void OnSubmit()
	{
		if (_focusPanel != EFocusPanel.Register)
		{
			TrySetGoodsAsync(_selectedSlotModel).Forget();
		}
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

	private void OnRegisterSlotEnter(GoodsSlotModel slotModel)
	{
		if (_isSelectedSlotLocked)
		{
			_isHoveringRegisterSlot = true;
		}
	}

	private void OnRegisterSlotExit(GoodsSlotModel slotModel)
	{
		if (_isSelectedSlotLocked)
		{
			_isHoveringRegisterSlot = false;
		}
	}

	private void OnSlotClick(InventoryGoodsSlotModel slotModel)
	{
		if (!_isDragging)
		{
			TrySetGoodsAsync(slotModel).Forget();
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
			if (_selectedSlotModel != null && _isHoveringRegisterSlot)
			{
				TrySetGoodsAsync(_selectedSlotModel).Forget();
			}
			_selectedSlotModel = null;
			_targetSlotModel = null;
			_isHoveringRegisterSlot = false;
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
		if (_selectedSlotModel != null && _isHoveringRegisterSlot)
		{
			TrySetGoodsAsync(_selectedSlotModel).Forget();
		}
		_selectedSlotModel = null;
		_targetSlotModel = null;
		_isHoveringRegisterSlot = false;
	}

	private void OnRegisterSlotClicked()
	{
		if (_panelModel.RxGoods.Value != null)
		{
			_soundManager.PlaySfx("ItemRelease");
			_panelModel.Clear();
		}
	}

	private async UniTask TrySetGoodsAsync(InventoryGoodsSlotModel slotModel)
	{
		if (slotModel == null || slotModel.IsSlotEmpty)
		{
			return;
		}
		Goods goods = slotModel.GoodsSlot.Goods;
		if (goods.Type == _model.RegisterableType)
		{
			_soundManager.PlaySfx("ButtonClick2");
			_panelModel.SetGoods(goods);
			_panelModel.RxIsInvalid.Value = _playerManager.IsItemUsed(goods.Id);
			_sourceSlotCategory = slotModel.SlotCategory;
			_sourceSlotIndex = slotModel.SlotIndex;
			if (InputManager.IsGamePad)
			{
				await UniTask.NextFrame();
			}
			FocusPanel(EFocusPanel.Register);
		}
		else if (goods.Type != EGoodsType.None)
		{
			_soundManager.PlaySfx("Fail");
			_toastManager.NoticeFail(TLocalizedText.RegisterInvalidTypeToastFormat.Format(_model.RegisterableType.ToLocalText()));
		}
	}

	private async UniTaskVoid TryRegisterItemUsedAsync()
	{
		Goods goods = _panelModel.RxGoods.Value;
		if (goods == null)
		{
			return;
		}
		if (!_playerManager.IsItemUsed(goods.Id))
		{
			ResRegisterItemUsed resRegisterItemUsed = await _gameChannel.RequestAsync<ResRegisterItemUsed>(new ReqRegisterItemUsed
			{
				ItemId = goods.Id,
				SlotCategory = _sourceSlotCategory,
				SlotIndex = _sourceSlotIndex,
				ExpectedType = _model.RegisterableType
			});
			if (resRegisterItemUsed == null || !resRegisterItemUsed.IsSuccess)
			{
				_toastManager.NoticeFail(TLocalizedText.RegisterFailToast.LocalText);
				return;
			}
			_soundManager.PlaySfx("Recipe_Success");
			_toastManager.Notice(TLocalizedText.RegisterSuccessToastFormat.Format(TItem.Table[goods.Id].Name));
			_panelModel.Clear();
			if (InputManager.IsGamePad)
			{
				await UniTask.DelayFrame(2);
			}
			FocusPanel(EFocusPanel.Left);
			RefreshSelectedSlotModel();
		}
		else
		{
			_soundManager.PlaySfx("Fail");
			_toastManager.NoticeFail(TLocalizedText.RegisterFailAlready.LocalText);
		}
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
		if (_focusPanel != EFocusPanel.Left)
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
			_isHoveringRegisterSlot = false;
		}
		_itemPanelType = ((_itemPanelType == EItemPanelType.Inventory) ? EItemPanelType.Stash : EItemPanelType.Inventory);
		_view.MyInventory.gameObject.SetActive(_itemPanelType == EItemPanelType.Inventory);
		_view.StashPanel.gameObject.SetActive(_itemPanelType == EItemPanelType.Stash);
		FocusPanel(_focusPanel);
	}

	private void OnBack()
	{
		if (_focusPanel != EFocusPanel.Register)
		{
			Close();
			return;
		}
		_panelModel.Clear();
		FocusPanel(EFocusPanel.Left);
	}
}
