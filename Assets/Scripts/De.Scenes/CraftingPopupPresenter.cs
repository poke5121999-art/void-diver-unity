using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class CraftingPopupPresenter : GamePopupPresenter
{
	private enum EToggleGoodsSubType
	{
		All = 0,
		Weapon = 1,
		Accessory = 2,
		Consumable = 3,
		Misc = 4
	}

	[View]
	private CraftingPopupView _view;

	[InjectModel]
	private CraftingPopupModel _model;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameChannel _gameChannel;

	private CraftingGoodsScrollerModel _scrollerModel;

	private CraftingMaterialPanelModel _materialPanelModel;

	private CraftingGoodsSlotModel _selectedSlot;

	private CraftingGoodsSlotModel _focusedSlot;

	private readonly ReactiveProperty<int> _rxSelectedCraftingId = new ReactiveProperty<int>();

	private int _currentToggleIndex;

	private readonly Dictionary<int, (int FocusedIndex, float ScrollPosition)> _tabFocusCache = new Dictionary<int, (int, float)>();

	private readonly Subject<Goods> _craftingNavigateSubject = new Subject<Goods>();

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override string OpenSfx => "InventoryPopupOpen";

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private InputActions.UiPadActions UiPadActions => GameInputManager.UiPadActions;

	private CraftingGoodsScrollerPresenter CraftingGoodsScrollerPresenter => _view.Scroller.GetComponent<CraftingGoodsScrollerPresenter>();

	private PlayerInventory MyInventory => _gameManager.MyInventory;

	private bool AllowAccessStash => _gameManager.IsInLounge;

	private bool HasEnoughMaterials
	{
		get
		{
			if (TCrafting.Table.TryGetValue(_rxSelectedCraftingId.Value, out var value))
			{
				return _playerManager.HasEnoughGoods(value.MaterialGoodsDatas);
			}
			return false;
		}
	}

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.CurrencyPanel.gameObject.SetActive(_gameManager.IsInLounge);
		_materialPanelModel = new CraftingMaterialPanelModel
		{
			ShowConfirm = true,
			OnCraftingNavigate = _craftingNavigateSubject
		};
		_materialPanelModel.OnConfirm.Subscribe(delegate
		{
			OnMaterialConfirm();
		}).AddTo(this);
		_materialPanelModel.OutOfLeft.Subscribe(delegate
		{
			OnBack();
		}).AddTo(this);
		_rxSelectedCraftingId.Value = TCrafting.GroupTable[_model.CraftingGroupId].Where((TCrafting _) => _playerManager.CheckStateConditions(_.UnlockConditions)).Min((TCrafting _) => _.Id);
		_rxSelectedCraftingId.Subscribe(OnSelectedCraftingIdChanged).AddTo(this);
		_scrollerModel = new CraftingGoodsScrollerModel
		{
			CraftingGroupId = _model.CraftingGroupId,
			FilterType = EGoodsType.None
		};
		_scrollerModel.OnSlotEnter.Subscribe(OnSlotEnter).AddTo(this);
		_scrollerModel.OnSlotExit.Subscribe(OnSlotExit).AddTo(this);
		_scrollerModel.OnSlotClick.Subscribe(OnSlotClick).AddTo(this);
		CraftingGoodsScrollerPresenter.Inject(_scrollerModel);
		_craftingNavigateSubject.Subscribe(OnCraftingNavigate).AddTo(this);
		_playerManager.RxProfile.Subscribe(delegate
		{
			UpdateAvailable();
		}).AddTo(this);
		_playerManager.OnOwnedGoodsChanged.Subscribe(delegate
		{
			UpdateAvailable();
		}).AddTo(this);
		_currentToggleIndex = 0;
		InitFilterToggles();
		_view.Craftable_Toggle.SetIsOnWithoutNotify(false);
		_view.Craftable_Toggle.OnValueChangedWithSfx().Subscribe(OnCraftableToggleChanged).AddTo(this);
	}

	private void OnCraftableToggleChanged(bool isOn)
	{
		_scrollerModel.RxCraftableOnly.Value = isOn;
	}

	private void UpdateAvailable()
	{
		_view.Available.SetActive(HasEnoughMaterials);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		if (GameInputManager != null)
		{
			UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				OnBack();
			}).AddTo(InputDisposable);
			UiPadActions.TabLeft.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				OnTabLeft();
			}).AddTo(InputDisposable);
			UiPadActions.TabRight.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				OnTabRight();
			}).AddTo(InputDisposable);
			_scrollerModel.RxIsInputEnabled.Value = _selectedSlot == null;
			_materialPanelModel.RxIsInputEnabled.Value = _selectedSlot != null;
		}
	}

	protected override void OnInputDisabled()
	{
		_scrollerModel.RxIsInputEnabled.Value = false;
		_materialPanelModel.RxIsInputEnabled.Value = false;
		base.OnInputDisabled();
	}

	private void OnBack()
	{
		if (_selectedSlot == null)
		{
			Close();
			return;
		}
		UpdateSelectedObject(null);
		if (_focusedSlot != null)
		{
			_rxSelectedCraftingId.Value = _focusedSlot.CraftingId;
		}
	}

	private void UpdateSelectedObject(CraftingGoodsSlotModel slot)
	{
		if (_selectedSlot != null)
		{
			_selectedSlot.RxIsSelected.Value = false;
		}
		if (slot != null)
		{
			slot.RxIsSelected.Value = true;
		}
		_selectedSlot = slot;
		if (IsInputEnable)
		{
			_materialPanelModel.EntryDirection = ENavDir.None;
			_scrollerModel.RxIsInputEnabled.Value = _selectedSlot == null;
			_materialPanelModel.RxIsInputEnabled.Value = _selectedSlot != null;
		}
	}

	private void UpdateInfo(CraftingGoodsSlotModel slot)
	{
		if (slot != null)
		{
			_rxSelectedCraftingId.Value = slot.CraftingId;
		}
	}

	private void OnMaterialConfirm()
	{
		TryCraftAsync().Forget();
	}

	private void OnTabLeft()
	{
		if (_selectedSlot == null)
		{
			ChangeToggle((_currentToggleIndex - 1 + _view.Toggles.Length) % _view.Toggles.Length);
		}
	}

	private void OnTabRight()
	{
		if (_selectedSlot == null)
		{
			ChangeToggle((_currentToggleIndex + 1) % _view.Toggles.Length);
		}
	}

	private void ChangeToggle(int index)
	{
		_tabFocusCache[_currentToggleIndex] = (_scrollerModel.FocusedIndex, _scrollerModel.ScrollPosition);
		_currentToggleIndex = index;
		_scrollerModel.RxIsInputEnabled.Value = false;
		_tabFocusCache.TryGetValue(index, out var value);
		_scrollerModel.FocusedIndex = value.FocusedIndex;
		_scrollerModel.ScrollPosition = value.ScrollPosition;
		_view.Toggles[index].GetComponent<Toggle>().isOn = true;
		_scrollerModel.RxIsInputEnabled.Value = true;
	}

	private void InitFilterToggles()
	{
		int length = Enum.GetValues(typeof(EToggleGoodsSubType)).Length;
		for (int i = 0; i < length; i++)
		{
			EGoodsType goodsSubType = (EToggleGoodsSubType)i switch
			{
				EToggleGoodsSubType.All => EGoodsType.None,
				EToggleGoodsSubType.Weapon => EGoodsType.Weapon,
				EToggleGoodsSubType.Accessory => EGoodsType.Accessory,
				EToggleGoodsSubType.Consumable => EGoodsType.Consumable,
				EToggleGoodsSubType.Misc => EGoodsType.Misc,
				_ => throw new ArgumentOutOfRangeException(),
			};
			_view.Toggles[i].GetComponent<Toggle>().OnValueTrueWithSfx().Subscribe(delegate
			{
				CraftingGoodsScrollerPresenter.ChangeFilter(goodsSubType);
			}).AddTo(this);
			if (goodsSubType == EGoodsType.None)
			{
				_view.Toggles[i].FindChildComponent<TextMeshProUGUI>("Text").text = TLocalizedText.GoodsFilterTypeAll.LocalText;
			}
		}
	}

	private void OnSelectedCraftingIdChanged(int craftingId)
	{
		Goods goods = TCrafting.Table[craftingId].ResultGoodsData.ToGoodsAsBase();
		bool flag = goods.IsItem() || goods.IsEquipment() || goods.IsBag();
		_view.Info.SetActive(flag);
		UpdateAvailable();
		if (flag)
		{
			_view.ItemTooltip.gameObject.SetActive(goods.IsItem());
			_view.EquipmentTooltip.gameObject.SetActive(goods.IsEquipment());
			_view.BagTooltip.gameObject.SetActive(goods.IsBag());
			if (goods.IsItem())
			{
				_view.ItemTooltip.Inject(new ItemTooltipModel
				{
					Goods = goods
				});
			}
			else if (goods.IsEquipment())
			{
				_view.EquipmentTooltip.Inject(new EquipmentTooltipModel
				{
					Goods = goods
				});
			}
			else
			{
				_view.BagTooltip.Inject(new BagTooltipModel
				{
					Goods = goods
				});
			}
			_materialPanelModel.CraftingId = craftingId;
			_view.CraftingMaterialPanel.Inject(_materialPanelModel);
		}
	}

	private void OnCraftingNavigate(Goods goods)
	{
		TCrafting tCrafting = TCrafting.GroupTable[_model.CraftingGroupId].Where((TCrafting _) => _playerManager.CheckStateConditions(_.UnlockConditions)).FirstOrDefault((TCrafting _) => _.ResultGoodsData.Type == goods.Type && _.ResultGoodsData.Id == goods.Id);
		if (tCrafting != null)
		{
			if (_currentToggleIndex != 0)
			{
				_tabFocusCache[_currentToggleIndex] = (_scrollerModel.FocusedIndex, _scrollerModel.ScrollPosition);
				_currentToggleIndex = 0;
				_view.Toggles[0].GetComponent<Toggle>().SetIsOnWithoutNotify(value: true);
			}
			_view.Craftable_Toggle.SetIsOnWithoutNotify(value: false);
			_view.Scroller.GetComponent<CraftingGoodsScrollerPresenter>().NavigateToItem(tCrafting.Id);
			CraftingGoodsSlotModel craftingGoodsSlotModel = _view.Scroller.GetComponent<CraftingGoodsScrollerPresenter>().FindSlotModel(tCrafting.Id);
			UpdateSelectedObject(craftingGoodsSlotModel);
			if (craftingGoodsSlotModel != null)
			{
				_rxSelectedCraftingId.Value = craftingGoodsSlotModel.CraftingId;
			}
		}
	}

	private void OnSlotEnter(CraftingGoodsSlotModel slot)
	{
		_focusedSlot = slot;
		if (_selectedSlot == null && _focusedSlot != null)
		{
			_rxSelectedCraftingId.Value = _focusedSlot.CraftingId;
		}
	}

	private void OnSlotExit(CraftingGoodsSlotModel slot)
	{
		_focusedSlot = null;
	}

	private void OnSlotClick(CraftingGoodsSlotModel slot)
	{
		UpdateSelectedObject(slot);
		if (slot != null)
		{
			_rxSelectedCraftingId.Value = slot.CraftingId;
		}
	}

	private bool CheckCraftable(int craftingId)
	{
		TCrafting tCrafting = TCrafting.Table[craftingId];
		if (!_playerManager.HasEnoughGoods(tCrafting.MaterialGoodsDatas))
		{
			_soundManager.PlaySfx("Fail");
			_toastManager.NoticeFail(TLocalizedText.NotEnoughCraftMaterials.LocalText);
			return false;
		}
		Goods goods = tCrafting.ResultGoodsData.ToGoodsAsBase();
		if (!MyInventory.CanAddGoods(goods, out var reason) && !(AllowAccessStash && _playerManager.CanAddGoodsToStash(goods)))
		{
			_toastManager.NoticeFail(reason);
			return false;
		}
		if (!_playerManager.CheckStateConditions(tCrafting.UnlockConditions))
		{
			_soundManager.PlaySfx("Fail");
			_toastManager.NoticeFail(TLocalizedText.CraftLocked.LocalText);
			return false;
		}
		return true;
	}

	private async UniTask<bool> TryCraftAsync()
	{
		int craftingId = _rxSelectedCraftingId.Value;
		if (!CheckCraftable(craftingId))
		{
			return false;
		}
		CraftingConfirmPopupPresenter craftingConfirmPopupPresenter = await _popupManager.OpenAsync<CraftingConfirmPopupPresenter>(new CraftingConfirmPopupModel
		{
			CraftingId = craftingId
		});
		object obj = await craftingConfirmPopupPresenter.Task;
		if (!(obj is long count) || count < 1)
		{
			return false;
		}
		Goods[] consumeGoods = TCrafting.Table[craftingId].MaterialGoodsDatas.Select((GoodsData _) => GoodsCreator.MakeFromGoodsData(_, count * _.Count, 0L)).ToArray();
		string bagConsumeFailToast = _playerManager.GetBagConsumeFailToast(consumeGoods);
		if (bagConsumeFailToast != null)
		{
			_toastManager.NoticeFail(bagConsumeFailToast);
			return false;
		}
		ResCraftingGoods resCraftingGoods = await _gameChannel.RequestAsync<ResCraftingGoods>(new ReqCraftingGoods
		{
			CraftingId = craftingId,
			Count = count
		});
		if (resCraftingGoods == null || !resCraftingGoods.IsSuccess)
		{
			return false;
		}
		string name = TCrafting.Table[craftingId].ResultGoodsData.GetName();
		string text = ((resCraftingGoods.RemainGoods != null && resCraftingGoods.RemainGoods.Length > 0) ? TLocalizedText.GoodsCraftedWithStashFormat.Format(name) : TLocalizedText.GoodsCraftedFormat.Format(name));
		_toastManager.Notice(text);
		_soundManager.PlaySfx("EquipmentProduction");
		return true;
	}
}
