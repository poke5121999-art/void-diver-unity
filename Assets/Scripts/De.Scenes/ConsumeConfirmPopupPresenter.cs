using System;
using Cysharp.Threading.Tasks;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ConsumeConfirmPopupPresenter : GamePopupPresenter
{
	[View]
	private ConsumeConfirmPopupView _view;

	[InjectModel]
	private ConsumeConfirmPopupModel _model;

	[Inject]
	private PlayerManager _playerManager;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private InputActions.UiKeyboardActions UiKeyboardActions => GameInputManager.UiKeyboardActions;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.Confirm_Button.enabled = _model.GoldAmount <= _playerManager.Gold;
		_view.Title_Text.text = _model.TitleText;
		_view.Title_SubText.gameObject.SetActive(_model.SubText.HasValue());
		_view.Title_SubText.text = _model.SubText;
		_view.Desc.text = _model.DescText;
		_view.Gold.SetActive(_model.GoldAmount > 0);
		_view.Gold_Text.text = _model.GoldAmount.ToNumberString();
		_view.Coin.SetActive(_model.CoinAmount > 0);
		_view.Coin_Text.text = _model.CoinAmount.ToNumberString();
		_view.Confirm_Button.OnClickAsObservable().Subscribe(delegate
		{
			OnConfirm();
		}).AddTo(this);
		_view.Cancel_Button.OnClickAsObservable().Subscribe(delegate
		{
			OnCancel();
		}).AddTo(this);
		InitGamePadNav();
	}

	private void InitGamePadNav()
	{
		Selectable[] items = new Selectable[2] { _view.Confirm_Button, _view.Cancel_Button };
		SetNavHorizontal(items);
		SetDefaultNav(_view.Confirm_Button);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		if (GameInputManager != null)
		{
			UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				OnCancel();
			}).AddTo(InputDisposable);
			UiKeyboardActions.Confirm.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				OnConfirm();
			}).AddTo(InputDisposable);
			UiKeyboardActions.Cancel.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				OnCancel();
			}).AddTo(InputDisposable);
		}
	}

	private void OnCancel()
	{
		Close(false);
	}

	private void OnConfirm()
	{
		Close(true);
	}
}
