using System;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using NemoLib.Mvp;
using NemoLib.Popup;
using GameCommon.Data;
using GameCommon.Table;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;

public class CoinExchangePopupPresenter : GamePopupPresenter
{
	[InjectModel]
	private CoinExchangePopupModel _model;

	[View]
	private CoinExchangePopupView _view;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private SoundManager _soundManager;

	private ECoinExchangeDirection _direction;

	private long _quantity = 1L;

	private long _quantityMax;

	private bool _isRequesting;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private InputActions.UiPadActions UiPadActions => GameInputManager.UiPadActions;

	private InputActions.UiKeyboardActions UiKeyboardActions => GameInputManager.UiKeyboardActions;

	public override bool IsInputActionPopup => true;

	public override string OpenSfx => "PopUpOpen";

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_direction = _model.Direction;
		_view.ToggleGroup_ToggleIn.isOn = _direction == ECoinExchangeDirection.CoinToItem;
		_view.ToggleGroup_ToggleOut.isOn = _direction == ECoinExchangeDirection.ItemToCoin;
		_view.ToggleGroup_ToggleIn.OnValueTrueWithSfx().Subscribe(delegate
		{
			OnDirectionChanged(ECoinExchangeDirection.CoinToItem);
		}).AddTo(this);
		_view.ToggleGroup_ToggleOut.OnValueTrueWithSfx().Subscribe(delegate
		{
			OnDirectionChanged(ECoinExchangeDirection.ItemToCoin);
		}).AddTo(this);
		_view.Quantity_DecreaseButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnDecrease();
		}).AddTo(this);
		_view.Quantity_IncreaseButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnIncrease();
		}).AddTo(this);
		_view.Quantity_Slider.OnValueChangedAsObservable().Subscribe(OnSliderValueChanged).AddTo(this);
		_view.Confirm_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnConfirmAsync().Forget();
		}).AddTo(this);
		_view.Cancel_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnCancel();
		}).AddTo(this);
		_playerManager.RxProfile.Skip(1).Where((Profile _) => _direction == ECoinExchangeDirection.CoinToItem).Subscribe(delegate
		{
			OnUserUpdated();
		}).AddTo(this);
		_playerManager.OnOwnedGoodsChanged.Where((Unit _) => _direction == ECoinExchangeDirection.ItemToCoin).Subscribe(delegate
		{
			OnUserUpdated();
		}).AddTo(this);
		UpdateDirectionUi();
		UpdateQuantityUi();
		ConnectNav(_view.Quantity_Slider, ENavDir.Down, _view.Confirm_Button);
		ConnectNav(_view.Confirm_Button, ENavDir.Up, _view.Quantity_Slider);
		ConnectNav(_view.Confirm_Button, ENavDir.Right, _view.Cancel_Button);
		ConnectNav(_view.Cancel_Button, ENavDir.Up, _view.Quantity_Slider);
		ConnectNav(_view.Cancel_Button, ENavDir.Left, _view.Confirm_Button);
		SetDefaultNav(_view.Confirm_Button);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		GameInputManager gameInputManager = GameInputManager;
		if (gameInputManager != null)
		{
			UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				Close();
			}).AddTo(InputDisposable);
			UiPadActions.TabLeft.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				OnTabLeft();
			}).AddTo(InputDisposable);
			UiPadActions.TabRight.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				OnTabRight();
			}).AddTo(InputDisposable);
			UiPadActions.DecreaseQuantityLarge.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				OnDecreaseQuantityLarge();
			}).AddTo(InputDisposable);
			UiPadActions.IncreaseQuantityLarge.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				OnIncreaseQuantityLarge();
			}).AddTo(InputDisposable);
			UiKeyboardActions.Slider.OnClickAsObservable().Subscribe(OnScrollInput).AddTo(InputDisposable);
		}
	}

	private void OnScrollInput(InputAction.CallbackContext ctx)
	{
		float y = ctx.ReadValue<Vector2>().y;
		if (y > 0f)
		{
			if (_quantityMax > _quantity)
			{
				SetQuantity(_quantity + 1);
			}
		}
		else if (y < 0f && _quantity >= 2)
		{
			SetQuantity(_quantity - 1);
		}
	}

	private void OnDirectionChanged(ECoinExchangeDirection direction)
	{
		if (_direction != direction)
		{
			_direction = direction;
			_quantity = 1L;
			UpdateDirectionUi();
			UpdateQuantityUi();
		}
	}

	private void UpdateUi(bool applyImmediately = false)
	{
		UpdateDirectionUi();
		UpdateQuantityUi();
	}

	private void UpdateDirectionUi()
	{
		int direction = (int)_direction;
		_view.Change_ImgCoinLeft.gameObject.SetActive(direction == 0);
		_view.Change_ImgRealCoinRight.gameObject.SetActive(direction == 0);
		_view.Change_ImgRealCoinLeft.gameObject.SetActive(direction != 0);
		_view.Change_ImgCoinRight.gameObject.SetActive(direction != 0);
		TLocalizedText tLocalizedText = ((direction == 0) ? TLocalizedText.UCoinExchangePopupIssuanceText : TLocalizedText.UCoinExchangePopupRegistrationText);
		_view.Desc.text = tLocalizedText.LocalText;
	}

	private void UpdateQuantityUi(long? overrideMax = null)
	{
		long num = overrideMax ?? ((_direction == ECoinExchangeDirection.CoinToItem) ? _playerManager.Coin : GetRealCoinCount());
		_quantityMax = num;
		_view.Quantity_Slider.wholeNumbers = true;
		long quantity;
		if (_quantityMax < 1)
		{
			_view.Quantity_Slider.minValue = 0f;
			_view.Quantity_Slider.maxValue = 0f;
			quantity = 0L;
		}
		else
		{
			_view.Quantity_Slider.maxValue = _quantityMax;
			_view.Quantity_Slider.minValue = 0f;
			quantity = _quantity;
		}
		SetQuantity(quantity);
	}

	private void OnDecrease()
	{
		if (_quantity >= 2)
		{
			SetQuantity(_quantity - 1);
		}
	}

	private void OnIncrease()
	{
		if (_quantityMax > _quantity)
		{
			SetQuantity(_quantity + 1);
		}
	}

	private void OnSliderValueChanged(float value)
	{
		if (!Mathf.Approximately(value, _quantity))
		{
			SetQuantity((long)value);
		}
	}

	private void SetQuantity(long quantity)
	{
		_quantity = ((_quantityMax < 1) ? 0 : Math.Clamp(quantity, 1L, _quantityMax));
		_view.Quantity_Slider.SetValueWithoutNotify(_quantity);
		_view.Quantity_Value.text = _quantity.ToString("N0");
	}

	private long GetRealCoinCount()
	{
		return _playerManager.GetTotalGoodsCount(GoodsCreator.MakeItemGoods(10001));
	}

	private async UniTask OnConfirmAsync()
	{
		if (_isRequesting || _quantity < 1 || _quantityMax < 1)
		{
			return;
		}
		long num = ((_direction == ECoinExchangeDirection.CoinToItem) ? _playerManager.Coin : GetRealCoinCount());
		if (num < _quantity)
		{
			return;
		}
		_isRequesting = true;
		long sentQuantity = _quantity;
		long maxBeforeSend = _quantityMax;
		bool succeeded = false;
		try
		{
			ResExchangeCoin resExchangeCoin = await _gameChannel.RequestAsync<ResExchangeCoin>(new ReqExchangeCoin
			{
				Direction = _direction,
				Quantity = sentQuantity
			});
			if (resExchangeCoin == null || !resExchangeCoin.IsSuccess)
			{
				return;
			}
			succeeded = true;
			_soundManager.PlaySfx((resExchangeCoin.Direction == ECoinExchangeDirection.CoinToItem) ? "Casher_Coin_Actual" : "Casher_Coin_Online");
			string text = ((resExchangeCoin.Direction == ECoinExchangeDirection.CoinToItem) ? TLocalizedText.CoinExchangeReal.LocalText : TLocalizedText.CoinExchangeOnline.LocalText);
			_toastManager.Notice(string.Format(text, sentQuantity));
		}
		finally
		{
			// finally funclet @0x180950680 (not a stub: it reads the hoisted locals through pointers): UpdateQuantityUi((long?)(succeeded ? Math.Max(0L, maxBeforeSend - sentQuantity) : maxBeforeSend)) on every exit, then _isRequesting = false
			UpdateQuantityUi(succeeded ? Math.Max(0L, maxBeforeSend - sentQuantity) : maxBeforeSend);
			_isRequesting = false;
		}
	}

	private void OnCancel()
	{
		Close();
	}

	private void OnUserUpdated()
	{
		UpdateQuantityUi();
	}

	private void OnTabLeft()
	{
		if (_direction != ECoinExchangeDirection.CoinToItem)
		{
			_view.ToggleGroup_ToggleIn.isOn = true;
		}
	}

	private void OnTabRight()
	{
		if (_direction != ECoinExchangeDirection.ItemToCoin)
		{
			_view.ToggleGroup_ToggleOut.isOn = true;
		}
	}

	private void OnDecreaseQuantityLarge()
	{
		SetQuantity(_quantity - Math.Max(1L, _quantityMax / 10));
	}

	private void OnIncreaseQuantityLarge()
	{
		SetQuantity(_quantity + Math.Max(1L, _quantityMax / 10));
	}

	private void InitGamePadNav()
	{
		ConnectNav(_view.Quantity_Slider, ENavDir.Down, _view.Confirm_Button);
		ConnectNav(_view.Confirm_Button, ENavDir.Up, _view.Quantity_Slider);
		ConnectNav(_view.Confirm_Button, ENavDir.Right, _view.Cancel_Button);
		ConnectNav(_view.Cancel_Button, ENavDir.Up, _view.Quantity_Slider);
		ConnectNav(_view.Cancel_Button, ENavDir.Left, _view.Confirm_Button);
		SetDefaultNav(_view.Confirm_Button);
	}
}
