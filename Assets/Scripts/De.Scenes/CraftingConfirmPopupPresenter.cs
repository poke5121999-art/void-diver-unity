using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;

public class CraftingConfirmPopupPresenter : GamePopupPresenter
{
	[View]
	private CraftingConfirmPopupView _view;

	[InjectModel]
	private CraftingConfirmPopupModel _model;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private GameManager _gameManager;

	private long _maxCraftableCount;

	private long _craftingCount;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override string OpenSfx => "InventoryPopupOpen";

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private InputActions.UiKeyboardActions UiKeyboardActions => GameInputManager.UiKeyboardActions;

	private PlayerInventory MyInventory => _gameManager.MyInventory;

	private bool AllowAccessStash => _gameManager.IsInLounge;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		Goods[] array = _model.Materials ?? Array.Empty<Goods>();
		if (_model.NeedGold > 0)
		{
			array = array.Append(new GoldGoods(0L, _model.NeedGold)).ToArray();
		}
		if (_model.NeedCoin > 0)
		{
			array = array.Append(new CoinGoods(0L, _model.NeedCoin)).ToArray();
		}
		if (_model.NeedRealCoin > 0)
		{
			array = array.Append(GoodsCreator.MakeItemGoods(10001, _model.NeedRealCoin)).ToArray();
		}
		_maxCraftableCount = (_model.CraftingGoods.IsItem() ? _playerManager.CalculateEnoughCraftingCount(array) : 1);
		if (_model.CraftingGoods.IsItem() && !(AllowAccessStash && _playerManager.CanAddGoodsToStash(_model.CraftingGoods)))
		{
			_maxCraftableCount = Math.Min(_maxCraftableCount, MyInventory.GetAddableItemCountWithBags(_model.CraftingGoods.Id));
		}
		_craftingCount = 1L;
		InventoryGoodsSlotModel inventoryGoodsSlotModel = new InventoryGoodsSlotModel(_model.CraftingGoods, InventoryGoodsSlotPresenter.ELayout.Normal);
		inventoryGoodsSlotModel.UseTooltip = true;
		inventoryGoodsSlotModel.ShowItemCount = true;
		_view.CraftingItem_GoodsSlot.Inject(inventoryGoodsSlotModel);
		_view.CraftingItem_Name.text = _model.CraftingGoods.GetName();
		_view.Cancel_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close();
		}).AddTo(this);
		_view.Confirm_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close(_craftingCount);
		}).AddTo(this);
		_view.Quantity_IncreaseButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnIncrease();
		}).AddTo(this);
		_view.Quantity_DecreaseButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnDecrease();
		}).AddTo(this);
		_view.Quantity.SetActive(_model.CraftingGoods.IsItem());
		_view.Quantity_Slider.maxValue = _maxCraftableCount;
		_view.Quantity_Slider.minValue = 0f;
		_view.Quantity_Slider.SetValueWithoutNotify(_craftingCount);
		_view.Quantity_Slider.OnValueChangedAsObservable().Subscribe(OnQuantitySliderValueChanged).AddTo(this);
		UpdateUis();
		InitGamePadNav();
	}

	private void InitGamePadNav()
	{
		ClearNav();
		bool activeSelf = _view.Quantity.activeSelf;
		IReadOnlyList<Selectable> navigableSlots = _view.CraftingMaterialPanel.GetNavigableSlots();
		int count = navigableSlots.Count;
		ConnectNav(_view.Confirm_Button, ENavDir.Right, _view.Cancel_Button);
		ConnectNav(_view.Cancel_Button, ENavDir.Left, _view.Confirm_Button);
		if (count > 0)
		{
			SetNavGrid(navigableSlots, _view.CraftingMaterialPanel.NavigableColumnCount);
		}
		if (!activeSelf)
		{
			if (count > 0)
			{
				Selectable to = LastRowHead(navigableSlots);
				ConnectGridBottomTo(navigableSlots, _view.Confirm_Button);
				ConnectNav(_view.Confirm_Button, ENavDir.Up, to);
				ConnectNav(_view.Cancel_Button, ENavDir.Up, to);
			}
		}
		else
		{
			ConnectNav(_view.Confirm_Button, ENavDir.Up, _view.Quantity_Slider);
			ConnectNav(_view.Cancel_Button, ENavDir.Up, _view.Quantity_Slider);
			ConnectNav(_view.Quantity_Slider, ENavDir.Down, _view.Confirm_Button);
			if (count > 0)
			{
				ConnectGridBottomTo(navigableSlots, _view.Quantity_Slider);
				ConnectNav(_view.Quantity_Slider, ENavDir.Up, LastRowHead(navigableSlots));
			}
		}
		SetDefaultNav(_view.Confirm_Button);
	}

	private void ConnectGridBottomTo(IReadOnlyList<Selectable> slots, Selectable target)
	{
		int navigableColumnCount = _view.CraftingMaterialPanel.NavigableColumnCount;
		for (int i = 0; i < slots.Count; i++)
		{
			if (slots.Count <= i + navigableColumnCount)
			{
				ConnectNav(slots[i], ENavDir.Down, target);
			}
		}
	}

	private Selectable LastRowHead(IReadOnlyList<Selectable> slots)
	{
		int navigableColumnCount = _view.CraftingMaterialPanel.NavigableColumnCount;
		return slots[(slots.Count - 1) / navigableColumnCount * navigableColumnCount];
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
		UiKeyboardActions.Confirm.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close(_craftingCount);
		}).AddTo(InputDisposable);
		UiKeyboardActions.Cancel.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
		if (_model.CraftingGoods.IsItem())
		{
			UiKeyboardActions.Slider.OnClickAsObservable().Subscribe(OnScrollInput).AddTo(InputDisposable);
		}
	}

	private void OnScrollInput(InputAction.CallbackContext ctx)
	{
		float y = ctx.ReadValue<Vector2>().y;
		if (y > 0f && _craftingCount < _maxCraftableCount)
		{
			OnIncrease();
		}
		else if (y < 0f && _craftingCount > 1)
		{
			OnDecrease();
		}
	}

	private void UpdateUis()
	{
		_view.CraftingMaterialPanel.Inject(new CraftingMaterialPanelModel
		{
			CraftingId = _model.CraftingId,
			CraftingCount = _craftingCount
		});
		_view.Quantity_Value.text = string.Format("{0}/{1}", _craftingCount, _maxCraftableCount);
	}

	private void SetCount(long count)
	{
		_craftingCount = Math.Clamp(count, 1L, _maxCraftableCount);
		_view.Quantity_Slider.SetValueWithoutNotify(_craftingCount);
		UpdateUis();
	}

	private void OnQuantitySliderValueChanged(float value)
	{
		if (!Mathf.Approximately(value, _craftingCount))
		{
			SetCount((int)value);
		}
	}

	private void OnDecrease()
	{
		if (_craftingCount > 1)
		{
			SetCount(_craftingCount - 1);
		}
		else
		{
			_toastManager.Notice(TLocalizedText.MinCountOne.LocalText);
		}
	}

	private void OnIncrease()
	{
		if (_craftingCount < _maxCraftableCount)
		{
			SetCount(_craftingCount + 1);
		}
		else
		{
			_toastManager.Notice(TLocalizedText.MaxItemCountFormat.Format(_maxCraftableCount));
		}
	}
}
