using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine.UI;
using VContainer;

public class WeaponRepairPopupPresenter : GamePopupPresenter
{
	private enum EFocusPanel
	{
		Inventory = 0,
		RepairAll = 1,
		RepairPanel = 2
	}

	[View]
	private WeaponRepairPopupView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private ToastManager _toastManager;

	private PlayerInventoryPanelModel _inventoryPanelModel;

	private WeaponRepairPanelModel _repairPanelModel;

	private readonly ReactiveProperty<InventoryGoodsSlotModel> _rxSelectedSlotModel = new ReactiveProperty<InventoryGoodsSlotModel>();

	private int _totalRepairCost;

	private readonly List<(EGoodsSlotCategory Category, int SlotIndex)> _repairableSlots = new List<(EGoodsSlotCategory, int)>();

	private EFocusPanel _focusPanel;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_inventoryPanelModel = new PlayerInventoryPanelModel
		{
			PlayerInventory = _gameManager.MyInventory,
			AllowedSubTypes = new EGoodsType[2]
			{
				EGoodsType.Weapon,
				EGoodsType.Accessory
			}
		};
		_inventoryPanelModel.OnSlotClick.Subscribe(OnSlotClick).AddTo(this);
		_inventoryPanelModel.OnEquipmentSlotClick.Subscribe(OnEquipmentSlotClick).AddTo(this);
		_view.MyInventory.Inject(_inventoryPanelModel);
		_repairPanelModel = new WeaponRepairPanelModel();
		_view.WeaponRepairPanel.GetComponent<WeaponRepairPanelPresenter>().Inject(_repairPanelModel);
		_repairPanelModel.OnConfirmRepair.Subscribe(OnConfirmRepair).AddTo(this);
		_repairPanelModel.OnGoodsSlotLeftClick.Subscribe(delegate
		{
			ClearSelection();
		}).AddTo(this);
		_rxSelectedSlotModel.Subscribe(OnSelectedSlotChanged).AddTo(this);
		_view.BtnAllFRepair.OnClickWithSfx().Subscribe(delegate
		{
			OnRepairAllClicked();
		}).AddTo(this);
		UpdateRepairAllUi();
		PlayerInventory myInventory = _gameManager.MyInventory;
		if (myInventory != null)
		{
			myInventory.OnInventoryChanged.Subscribe(delegate
			{
				UpdateRepairAllUi();
			}).AddTo(this);
			myInventory.OnSafeInventoryChanged.Subscribe(delegate
			{
				UpdateRepairAllUi();
			}).AddTo(this);
		}
		_playerManager.RxCharacters.ObserveReplace().Subscribe(delegate
		{
			UpdateRepairAllUi();
		}).AddTo(this);
		InitGamePadNav();
	}

	private void InitGamePadNav()
	{
		_inventoryPanelModel.OutOfRight.Subscribe(delegate
		{
			FocusPanel(EFocusPanel.RepairAll);
		}).AddTo(this);
		_repairPanelModel.OutOfLeft.Subscribe(delegate
		{
			FocusPanel(EFocusPanel.Inventory);
		}).AddTo(this);
	}

	private void FocusPanel(EFocusPanel panel)
	{
		_focusPanel = panel;
		_inventoryPanelModel.RxIsInputEnabled.Value = panel == EFocusPanel.Inventory;
		_repairPanelModel.RxIsInputEnabled.Value = panel == EFocusPanel.RepairPanel;
		ClearNav();
		if (panel == EFocusPanel.RepairAll)
		{
			SetNavHorizontal(new Selectable[1] { _view.BtnAllFRepair });
			ResumeNav();
			SetDefaultNav(_view.BtnAllFRepair);
		}
		else
		{
			SuspendNav();
		}
	}

	protected override void OnNavOutOfBounds(ENavDir dir)
	{
		if (dir == ENavDir.Left)
		{
			FocusPanel(EFocusPanel.Inventory);
		}
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx().Subscribe(delegate
		{
			Back();
		}).AddTo(InputDisposable);
		FocusPanel(_focusPanel);
	}

	protected override void OnInputDisabled()
	{
		_inventoryPanelModel.RxIsInputEnabled.Value = false;
		_repairPanelModel.RxIsInputEnabled.Value = false;
		base.OnInputDisabled();
	}

	private void UpdateRepairAllUi()
	{
		_repairableSlots.Clear();
		_totalRepairCost = 0;
		CollectRepairableSlots();
		int count = _repairableSlots.Count;
		_view.BtnAllFRepair_Normal.gameObject.SetActive(count > 0);
		_view.BtnAllFRepair_Gold_Value.text = string.Format("{0}", _totalRepairCost);
		_view.BtnAllFRepair_Disabled.SetActive(count < 1);
	}

	private void CollectRepairableSlots()
	{
		Character selectedCharacter = _playerManager.SelectedCharacter;
		if (selectedCharacter != null)
		{
			GoodsSlot[] weaponSlots = selectedCharacter.WeaponSlots;
			foreach (GoodsSlot goodsSlot in weaponSlots)
			{
				if (goodsSlot.Goods is WeaponGoods weaponGoods && weaponGoods.CanRepair())
				{
					_repairableSlots.Add((EGoodsSlotCategory.WeaponSlot, goodsSlot.SlotIndex));
					_totalRepairCost += weaponGoods.GetRepairCost();
				}
			}
			GoodsSlot[] accessorySlots = selectedCharacter.AccessorySlots;
			foreach (GoodsSlot goodsSlot2 in accessorySlots)
			{
				if (goodsSlot2.Goods is AccessoryGoods accessoryGoods && accessoryGoods.CanRepair())
				{
					_repairableSlots.Add((EGoodsSlotCategory.AccessorySlot, goodsSlot2.SlotIndex));
					_totalRepairCost += accessoryGoods.GetRepairCost();
				}
			}
		}
		foreach (GoodsSlot value in _gameManager.MyInventory.Slots.Values)
		{
			if (value != null && !value.IsEmpty && value.Goods.IsEquipment() && value.Goods.CanRepair())
			{
				_repairableSlots.Add((EGoodsSlotCategory.InventorySlot, value.SlotIndex));
				_totalRepairCost += value.Goods.GetRepairCost();
			}
		}
		foreach (GoodsSlot value2 in _gameManager.MyInventory.SafeSlots.Values)
		{
			if (value2 != null && !value2.IsEmpty && value2.Goods.IsEquipment() && value2.Goods.CanRepair())
			{
				_repairableSlots.Add((EGoodsSlotCategory.SafeSlot, value2.SlotIndex));
				_totalRepairCost += value2.Goods.GetRepairCost();
			}
		}
	}

	private void OnRepairAllClicked()
	{
		OnRepairAllAsync().Forget();
	}

	private async UniTaskVoid OnRepairAllAsync()
	{
		if (_repairableSlots.Count == 0)
		{
			return;
		}
		if (!_playerManager.HasEnoughGold(_totalRepairCost))
		{
			_toastManager.Notice(TLocalizedText.NotEnoughGold.LocalText);
			return;
		}
		RepairConfirmPopupPresenter repairConfirmPopupPresenter = await _popupManager.OpenAsync<RepairConfirmPopupPresenter>(new RepairConfirmPopupModel
		{
			IsRepairAll = true,
			OverrideRepairCost = _totalRepairCost
		});
		object obj = await repairConfirmPopupPresenter.Task;
		if (obj is bool flag && flag)
		{
			ResRepairAllGoods resRepairAllGoods = await _gameChannel.RequestAsync<ResRepairAllGoods>(new ReqRepairAllGoods
			{
				PreventMaxDurabilityLoss = _playerManager.PreventRepairMaxDurabilityLoss
			});
			if (resRepairAllGoods != null && resRepairAllGoods.IsSuccess)
			{
				_soundManager.PlaySfx("Repair_Success");
				ClearSelection();
			}
			else
			{
				_toastManager.Notice(TLocalizedText.NotEnoughGold.LocalText);
			}
		}
	}

	private void OnSelectedSlotChanged(InventoryGoodsSlotModel slotModel)
	{
		if (slotModel != null && !slotModel.GoodsSlot.IsEmpty)
		{
			_repairPanelModel.RxSelectedGoods.Value = slotModel.GoodsSlot.Goods;
		}
		else
		{
			_repairPanelModel.RxSelectedGoods.Value = null;
		}
	}

	private void OnSlotClick(InventoryGoodsSlotModel slotModel)
	{
		if (!slotModel.GoodsSlot.IsEmpty && slotModel.GoodsSlot.Category == EGoodsCategory.Equipment)
		{
			_rxSelectedSlotModel.Value = slotModel;
			if (slotModel.GoodsSlot.Goods.CanRepair())
			{
				_focusPanel = EFocusPanel.RepairPanel;
				_inventoryPanelModel.RxIsInputEnabled.Value = false;
				_repairPanelModel.RxIsInputEnabled.Value = true;
				ClearNav();
				SuspendNav();
			}
		}
	}

	private void OnEquipmentSlotClick(InventoryGoodsSlotModel slotModel)
	{
		if (!slotModel.GoodsSlot.IsEmpty && slotModel.GoodsSlot.Category == EGoodsCategory.Equipment)
		{
			_rxSelectedSlotModel.Value = slotModel;
			if (slotModel.GoodsSlot.Goods.CanRepair())
			{
				_focusPanel = EFocusPanel.RepairPanel;
				_inventoryPanelModel.RxIsInputEnabled.Value = false;
				_repairPanelModel.RxIsInputEnabled.Value = true;
				ClearNav();
				SuspendNav();
			}
		}
	}

	private void OnConfirmRepair(Goods goods)
	{
		OnConfirmRepairAsync(goods).Forget();
	}

	private async UniTaskVoid OnConfirmRepairAsync(Goods goods)
	{
		if (!goods.CanRepair())
		{
			return;
		}
		if (!_playerManager.HasEnoughGold(goods.GetRepairCost()))
		{
			_toastManager.Notice(TLocalizedText.NotEnoughGold.LocalText);
			return;
		}
		RepairConfirmPopupPresenter repairConfirmPopupPresenter = await _popupManager.OpenAsync<RepairConfirmPopupPresenter>(new RepairConfirmPopupModel
		{
			Goods = goods
		});
		object obj = await repairConfirmPopupPresenter.Task;
		if (obj is bool flag && flag)
		{
			InventoryGoodsSlotModel value = _rxSelectedSlotModel.Value;
			ClearSelection();
			ResRepairGoods resRepairGoods = await _gameChannel.RequestAsync<ResRepairGoods>(new ReqRepairGoods
			{
				SlotCategory = value.SlotCategory,
				SlotIndex = value.GoodsSlot.SlotIndex,
				PreventMaxDurabilityLoss = _playerManager.PreventRepairMaxDurabilityLoss
			});
			if (resRepairGoods != null && resRepairGoods.IsSuccess)
			{
				_soundManager.PlaySfx("Repair_Success");
			}
			else
			{
				_toastManager.Notice(TLocalizedText.NotEnoughGold.LocalText);
			}
		}
	}

	private void ClearSelection()
	{
		_rxSelectedSlotModel.Value = null;
		_focusPanel = EFocusPanel.Inventory;
		_inventoryPanelModel.RxIsInputEnabled.Value = true;
		_repairPanelModel.RxIsInputEnabled.Value = false;
		ClearNav();
		SuspendNav();
	}

	private void Back()
	{
		if (_rxSelectedSlotModel.Value == null)
		{
			Close();
		}
		else
		{
			ClearSelection();
		}
	}
}
