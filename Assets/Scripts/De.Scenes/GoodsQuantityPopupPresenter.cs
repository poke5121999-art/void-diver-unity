using System;
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

public class GoodsQuantityPopupPresenter : GamePopupPresenter
{
	[InjectModel]
	private GoodsQuantityPopupModel _model;

	[View]
	private GoodsQuantityPopupView _view;

	[Inject]
	private ToastManager _toastManager;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private InputActions.UiKeyboardActions UiKeyboardActions => GameInputManager.UiKeyboardActions;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.Message.text = _model.Message;
		InventoryGoodsSlotModel inventoryGoodsSlotModel = new InventoryGoodsSlotModel(_model.Goods, InventoryGoodsSlotPresenter.ELayout.Normal);
		inventoryGoodsSlotModel.UseTooltip = true;
		inventoryGoodsSlotModel.ShowItemCount = false;
		_view.GoodsSlot.Inject(inventoryGoodsSlotModel);
		_view.Gold.SetActive(_model.ShowGoldPrice);
		_view.GoodsName.text = _model.Goods.GetName();
		_view.Slider.maxValue = _model.CountMax;
		_view.Slider.minValue = 0f;
		_view.Slider.SetValueWithoutNotify(_model.Count);
		UpdateValueUi();
		_view.Cancel_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close();
		}).AddTo(this);
		_view.Confirm_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnConfirm();
		}).AddTo(this);
		_view.DecreaseButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnDecrease();
		}).AddTo(this);
		_view.IncreaseButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnIncrease();
		}).AddTo(this);
		_view.Slider.OnValueChangedAsObservable().Subscribe(OnSliderValueChange).AddTo(this);
		InitGamePadNav();
	}

	private void InitGamePadNav()
	{
		ConnectNav(_view.Slider, ENavDir.Down, _view.Confirm_Button);
		SetNavHorizontal(new Selectable[2] { _view.Confirm_Button, _view.Cancel_Button });
		ConnectNav(_view.Confirm_Button, ENavDir.Up, _view.Slider);
		ConnectNav(_view.Cancel_Button, ENavDir.Up, _view.Slider);
		SetDefaultNav(_view.Confirm_Button);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		GameInputManager.UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
		GameInputManager.UiKeyboardActions.Confirm.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnConfirm();
		}).AddTo(InputDisposable);
		GameInputManager.UiKeyboardActions.Cancel.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
		GameInputManager.UiKeyboardActions.Slider.OnClickAsObservable().Subscribe(OnScrollInput).AddTo(InputDisposable);
	}

	private void OnScrollInput(InputAction.CallbackContext ctx)
	{
		float y = ctx.ReadValue<Vector2>().y;
		if (y > 0f && _model.Count < _model.CountMax)
		{
			OnIncrease();
		}
		else if (y < 0f && _model.Count > 1)
		{
			OnDecrease();
		}
	}

	private void OnConfirm()
	{
		Close(_model.Count);
	}

	private void OnDecrease()
	{
		if (_model.Count > 1)
		{
			SetCount(_model.Count - 1);
		}
		else
		{
			_toastManager.Notice(TLocalizedText.MinCountOne.LocalText);
		}
	}

	private void OnIncrease()
	{
		if (_model.Count >= _model.CountMax)
		{
			_toastManager.Notice(TLocalizedText.MaxItemCountFormat.Format(_model.CountMax));
		}
		else
		{
			SetCount(_model.Count + 1);
		}
	}

	private void OnSliderValueChange(float value)
	{
		if (!Mathf.Approximately(_model.Count, value))
		{
			SetCount((int)value);
		}
	}

	private void SetCount(long count)
	{
		_model.Count = Math.Clamp(count, 1L, _model.CountMax);
		_view.Slider.SetValueWithoutNotify(_model.Count);
		UpdateValueUi();
	}

	private void UpdateValueUi()
	{
		_view.QuantityValue.text = string.Format("{0} / {1}", _model.Count, _model.CountMax);
		ValueTween component = _view.Gold_Text.GetComponent<ValueTween>();
		component.SetValue(_model.GoldPrice * _model.Count, component.GetValue() == 0);
	}
}
