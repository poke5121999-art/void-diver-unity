using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using VContainer;

public class TycoonArtifactSettingPopupPresenter : GamePopupPresenter
{
	private enum EItemPanelType
	{
		Inventory = 0,
		Stash = 1
	}

	[View]
	private TycoonArtifactSettingPopupView _view;

	[InjectModel]
	private TycoonArtifactSettingPopupModel _model;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private PopupManager _popupManager;

	private PlayerInventoryPanelModel _inventoryPanelModel;

	private StashPanelModel _stashPanelModel;

	private InventoryGoodsSlotModel _selectedSlotModel;

	private EItemPanelType _itemPanelType;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	public override bool IsInputActionPopup => true;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_itemPanelType = EItemPanelType.Inventory;
		_view.InventoryPanel.gameObject.SetActive(value: true);
		_view.StashPanel.gameObject.SetActive(value: false);
		InitInventoryPanel();
		InitStashPanel();
		if (ShouldAutoSwitchToStash())
		{
			SwapItemPanel();
		}
		_view.BtnEmptyArtifact.OnClickWithSfx().Subscribe(delegate
		{
			ClearSelectedArtifact();
		}).AddTo(this);
		if (_model.CurrentSlotModel == null)
		{
			ClearSelectedArtifact();
			await UniTask.Yield();
		}
		else
		{
			SetSelectedArtifact(_model.CurrentSlotModel);
		}
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx().Subscribe(delegate
		{
			Close(_selectedSlotModel);
		}).AddTo(InputDisposable);
		UiActions.Switch.OnClickWithSfx().Subscribe(delegate
		{
			SwapItemPanel();
		}).AddTo(InputDisposable);
		BasePanelModel basePanelModel = ((_itemPanelType == EItemPanelType.Inventory) ? ((BasePanelModel)_inventoryPanelModel) : ((BasePanelModel)_stashPanelModel));
		if (basePanelModel != null)
		{
			basePanelModel.RxIsInputEnabled.Value = true;
		}
	}

	protected override void OnInputDisabled()
	{
		_inventoryPanelModel.RxIsInputEnabled.Value = false;
		_stashPanelModel.RxIsInputEnabled.Value = false;
		base.OnInputDisabled();
	}

	private void InitInventoryPanel()
	{
		_inventoryPanelModel = new PlayerInventoryPanelModel
		{
			PlayerInventory = _gameManager.MyInventory,
			AllowedSubTypes = new EGoodsType[1] { EGoodsType.Artifact }
		};
		_inventoryPanelModel.OnSlotClick.Subscribe(OnInventorySlotClick).AddTo(this);
		_view.InventoryPanel.Inject(_inventoryPanelModel);
	}

	private void InitStashPanel()
	{
		_view.StashPanel.gameObject.SetActive(value: true);
		_stashPanelModel = new StashPanelModel
		{
			AllowedSubtypes = new EGoodsType[1] { EGoodsType.Artifact },
			EnableFilterButtons = false
		};
		_stashPanelModel.OnSlotClick.Subscribe(OnStashSlotClick).AddTo(this);
		_view.StashPanel.Inject(_stashPanelModel);
		_view.StashPanel.gameObject.SetActive(value: false);
	}

	private void OnInventorySlotClick(InventoryGoodsSlotModel slotModel)
	{
		if (slotModel.GoodsSlot != null && !slotModel.GoodsSlot.IsEmpty && slotModel.GoodsSlot.Type == EGoodsType.Artifact)
		{
			ConfirmAndSelectArtifactAsync(slotModel).Forget();
		}
	}

	private void OnStashSlotClick(InventoryGoodsSlotModel slotModel)
	{
		if (slotModel.GoodsSlot != null && !slotModel.GoodsSlot.IsEmpty && slotModel.GoodsSlot.Type == EGoodsType.Artifact)
		{
			ConfirmAndSelectArtifactAsync(slotModel).Forget();
		}
	}

	private async UniTaskVoid ConfirmAndSelectArtifactAsync(InventoryGoodsSlotModel slotModel)
	{
		await UniTask.NextFrame();
		if (await _popupManager.ShowConfirmationTextPopupAsync(TLocalizedText.TycoonArtifactSettingConfirm.LocalText))
		{
			Close(slotModel);
		}
	}

	private void SetSelectedArtifact(InventoryGoodsSlotModel slotModel)
	{
		_selectedSlotModel = slotModel;
		_view.ImgArtifact.gameObject.SetActive(value: true);
		_view.ImgArtifact.sprite = _assetManager.GetIconEquipment(slotModel.GoodsId);
		_view.BtnEmptyArtifact.gameObject.SetActive(value: true);
	}

	private void ClearSelectedArtifact()
	{
		_selectedSlotModel = null;
		_view.ImgArtifact.gameObject.SetActive(value: false);
		_view.BtnEmptyArtifact.gameObject.SetActive(value: false);
	}

	private bool ShouldAutoSwitchToStash()
	{
		int num = 0;
		foreach (GoodsSlot value in _gameManager.MyInventory.Slots.Values)
		{
			if (value.Type == EGoodsType.Artifact)
			{
				num++;
			}
		}
		int num2 = 0;
		foreach (GoodsSlot value2 in _playerManager.StashSlots.Values)
		{
			if (value2.Type == EGoodsType.Artifact)
			{
				num2++;
			}
		}
		return num == 0 && num2 > 0;
	}

	private void SwapItemPanel()
	{
		if (_selectedSlotModel != null)
		{
			return;
		}
		if (base.IsInputEnable)
		{
			_inventoryPanelModel.RxIsInputEnabled.Value = false;
			_stashPanelModel.RxIsInputEnabled.Value = false;
		}
		_itemPanelType = ((_itemPanelType == EItemPanelType.Inventory) ? EItemPanelType.Stash : EItemPanelType.Inventory);
		_view.InventoryPanel.gameObject.SetActive(_itemPanelType == EItemPanelType.Inventory);
		_view.StashPanel.gameObject.SetActive(_itemPanelType == EItemPanelType.Stash);
		if (base.IsInputEnable)
		{
			BasePanelModel basePanelModel = ((_itemPanelType == EItemPanelType.Inventory) ? ((BasePanelModel)_inventoryPanelModel) : ((BasePanelModel)_stashPanelModel));
			if (basePanelModel != null)
			{
				basePanelModel.RxIsInputEnabled.Value = true;
			}
		}
	}
}
