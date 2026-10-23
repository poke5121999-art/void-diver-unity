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

public class KeyInteractionPopupPresenter : GamePopupPresenter
{
	private enum EFocusPanel
	{
		Left = 0,
		KeyInteraction = 1
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

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override bool IsInputActionPopup => true;

	public override string OpenSfx => "InventoryPopupOpen";

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private InputActions.UiPadActions UiPadActions => GameInputManager.UiPadActions;

	private PlayerInventory MyInventory => _gameManager.MyInventory;

	private ReactiveCollection<int> RxSelectedItemIds => _keyInteractionPanelModel.RxSelectedItemIds;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_focusPanel = EFocusPanel.Left;
		_isDragging = false;
		_isSelectedSlotLocked = false;
		_selectedSlotModel = null;
		_targetSlotModel = null;
		_draggingSlotModel = null;
		_view.DraggingGoodsSlot.gameObject.SetActive(false);
		_myInventoryPanelModel = new PlayerInventoryPanelModel
		{
			PlayerInventory = _gameManager.MyInventory,
			KeyItemFilterTypes = _model.KeyItemTypes,
			DisplayScope = EGoodsDisplayScope.AllOwned
		};
		_myInventoryPanelModel.RxFocusedSlotModel.Subscribe(OnSlotFocused).AddTo(OpenDisposable);
		_myInventoryPanelModel.OnSlotLeftClick.Subscribe(OnSlotClick).AddTo(OpenDisposable);
		_myInventoryPanelModel.OnSlotDrag.Subscribe(OnSlotDrag).AddTo(OpenDisposable);
		_myInventoryPanelModel.OnSlotEndDrag.Subscribe(OnSlotEndDrag).AddTo(OpenDisposable);
		_myInventoryPanelModel.RxFocusedSafeSlotModel.Subscribe(OnSlotFocused).AddTo(OpenDisposable);
		_view.MyInventory.Inject(_myInventoryPanelModel);
		_keyInteractionPanelModel = new KeyInteractionPanelModel
		{
			RequiredKeyCount = _model.KeyItemIds.Length
		};
		_keyInteractionPanelModel.OnSlotClick.Subscribe(OnKeyInteractionSlotLeftClick).AddTo(OpenDisposable);
		_keyInteractionPanelModel.OnConfirmClick.Subscribe(delegate
		{
			OnConfirm();
		}).AddTo(OpenDisposable);
		_keyInteractionPanelModel.RxFocusedSlotModel.Subscribe(OnSlotFocused).AddTo(OpenDisposable);
		_keyInteractionPanelModel.OnSlotDrag.Subscribe(OnSlotDrag).AddTo(OpenDisposable);
		_keyInteractionPanelModel.OnSlotEndDrag.Subscribe(OnSlotEndDrag).AddTo(OpenDisposable);
		_view.KeyInteractionPanel.Inject(_keyInteractionPanelModel);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.CloseMenu.OnClickAsObservable().Subscribe(delegate
		{
			Close();
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
		switch (panel)
		{
		case EFocusPanel.Left:
			_myInventoryPanelModel.RxIsInputEnabled.Value = true;
			break;
		case EFocusPanel.KeyInteraction:
			_keyInteractionPanelModel.RxIsInputEnabled.Value = true;
			break;
		}
	}

	private void DisableAllPanelInput()
	{
		_myInventoryPanelModel.RxIsInputEnabled.Value = false;
		_keyInteractionPanelModel.RxIsInputEnabled.Value = false;
	}

	private void OnSubmit()
	{
		if (_focusPanel != EFocusPanel.KeyInteraction && _selectedSlotModel != null && !_selectedSlotModel.IsSlotEmpty)
		{
			TryPickRegisterAsync(_selectedSlotModel.GoodsSlot.Id).Forget();
		}
	}

	private async UniTask TryPickRegisterAsync(int itemId)
	{
		if (TryRegisterItem(itemId) && RxSelectedItemIds.Count >= _model.KeyItemIds.Length && IsGamePad)
		{
			await UniTask.NextFrame();
			FocusPanel(EFocusPanel.KeyInteraction);
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

	private void OnConfirm()
	{
		int[] array = RxSelectedItemIds.ToArray();
		if (array.Length == 0)
		{
			_soundManager.PlaySfx("Fail");
			_toastManager.Notice(TLocalizedText.KeyInteractionOnEmptySlotClick.LocalText);
			return;
		}
		if (_model.KeyItemIds.Length != array.Length || _model.KeyItemIds.Except(array).Any() || array.Except(_model.KeyItemIds).Any())
		{
			_soundManager.PlaySfx("Fail");
			_toastManager.Notice(TLocalizedText.WrongKeyMessage.LocalText);
			_cameraManager.Shake(0.25f, 0.15f, 0.1f);
			return;
		}
		_gameChannel.SendToHost(new ReqTryKeyInteraction
		{
			ClientId = _gameManager.LocalClientId,
			InteractiveObjectId = _model.InteractiveObject.NetworkObjectId,
			KeyItemIds = array
		});
		Close(true);
	}

	private void OnBack()
	{
		if (_focusPanel == EFocusPanel.KeyInteraction)
		{
			RxSelectedItemIds.Clear();
			FocusPanel(EFocusPanel.Left);
		}
		else if (RxSelectedItemIds.Count < 1)
		{
			Close();
		}
		else
		{
			RxSelectedItemIds.Clear();
		}
	}

	private void OnSlotClick(InventoryGoodsSlotModel slotModel)
	{
		if (!_isDragging && slotModel != null && !slotModel.IsSlotEmpty)
		{
			TryRegisterItem(slotModel.GoodsSlot.Id);
		}
	}

	private void OnKeyInteractionSlotLeftClick(InventoryGoodsSlotModel slotModel)
	{
		if (slotModel != null && !slotModel.IsSlotEmpty)
		{
			TryUnregisterItem(slotModel.GoodsSlot.Id);
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

	private void OnSlotDrag(InventoryGoodsSlotModel slotModel)
	{
		if (slotModel != null && !slotModel.IsSlotEmpty)
		{
			_selectedSlotModel = slotModel;
			_isDragging = true;
			_isSelectedSlotLocked = true;
			Goods goods = GoodsCreator.MakeGoods(slotModel.GoodsSlot.Type, slotModel.GoodsSlot.Id, 1L);
			_draggingSlotModel = new DraggingGoodsSlotModel(goods);
			_view.DraggingGoodsSlot.gameObject.SetActive(true);
			_view.DraggingGoodsSlot.Inject(_draggingSlotModel);
		}
	}

	private void OnSlotEndDrag(InventoryGoodsSlotModel slotModel)
	{
		if (_isDragging)
		{
			_isDragging = false;
			_view.DraggingGoodsSlot.gameObject.SetActive(false);
			_isSelectedSlotLocked = false;
			_myInventoryPanelModel.RxHighlightCategory.Value = EGoodsSlotCategory.None;
			HandleInteraction();
		}
	}

	private void HandleInteraction()
	{
		if (_selectedSlotModel == _targetSlotModel || _selectedSlotModel == null || _targetSlotModel == null)
		{
			_selectedSlotModel = null;
		}
		else
		{
			EGoodsSlotCategory slotCategory = _selectedSlotModel.SlotCategory;
			if (slotCategory == EGoodsSlotCategory.KeySlot)
			{
				TryUnregisterItem(_selectedSlotModel.GoodsId);
			}
			else if (slotCategory == EGoodsSlotCategory.InventorySlot && _targetSlotModel.SlotCategory == EGoodsSlotCategory.KeySlot)
			{
				TryRegisterItem(_selectedSlotModel.GoodsId);
			}
			_selectedSlotModel = null;
		}
		_targetSlotModel = null;
	}

	private bool TryRegisterItem(int itemId)
	{
		if (RxSelectedItemIds.Contains(itemId))
		{
			_toastManager.Notice(TLocalizedText.KeyInteractionOnSameItemClick.LocalText);
			return false;
		}
		if (RxSelectedItemIds.Count < _model.KeyItemIds.Length)
		{
			RxSelectedItemIds.Add(itemId);
			_soundManager.PlaySfx("ItemRelease");
			return true;
		}
		_toastManager.Notice(TLocalizedText.KeyInteractionOnNomoreSlotsToClick.LocalText);
		return false;
	}

	private bool TryUnregisterItem(int itemId)
	{
		if (!RxSelectedItemIds.Contains(itemId))
		{
			_toastManager.Notice(TLocalizedText.KeyInteractionOnEmptySlotClick.LocalText);
			return false;
		}
		RxSelectedItemIds.Remove(itemId);
		_soundManager.PlaySfx("ItemRelease");
		return true;
	}
}
