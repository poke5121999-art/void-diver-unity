using UnityEngine;
using UniRx;
using GameCommon.Table;
using GameCommon.Enum;
using System.Globalization;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Mvp;
using NemoLib.Popup;
using VContainer;

public class InputNamePopupPresenter : GamePopupPresenter
{
	[View]
	private InputNamePopupView _view;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private PopupManager _popupManager;

	private GameInputManager GameInputManager => base.InputManager as GameInputManager;

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.Confirm_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnConfirmButtonClick();
		}).AddTo(this);
		_view.Cancel_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnCancelButtonClick();
		}).AddTo(this);
		_view.InputField.GetComponent<InputFieldController>().SetDescription(TLocalizedText.EnterUserName.LocalText);
		InitGamePadNav();
	}

	private void InitGamePadNav()
	{
		ConnectNav(_view.InputField, ENavDir.Down, _view.Confirm_Button);
		ConnectNav(_view.Confirm_Button, ENavDir.Up, _view.InputField);
		ConnectNav(_view.Confirm_Button, ENavDir.Right, _view.Cancel_Button);
		ConnectNav(_view.Cancel_Button, ENavDir.Up, _view.InputField);
		ConnectNav(_view.Cancel_Button, ENavDir.Left, _view.Confirm_Button);
		SetDefaultNav(_view.InputField);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		GameInputManager.UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close();
		}).AddTo(base.InputDisposable);
	}

	private async UniTask<bool> ConfirmAsync()
	{
		string text = _view.InputField.text;
		if (string.IsNullOrEmpty(text))
		{
			_toastManager.NoticeFail(TLocalizedText.EnterUserName.LocalText);
			return false;
		}
		if (ContainsSpecialCharacter(text))
		{
			_toastManager.NoticeFail(TLocalizedText.UserNameNoSpecialChars.LocalText);
			return false;
		}
		if (GetTextLength(text) > 10)
		{
			_toastManager.NoticeFail(TLocalizedText.UsernameLengthLimitFormat.Format(10));
			return false;
		}
		if (!UIInputUtility.ValidateName(text))
		{
			_toastManager.NoticeFail(TLocalizedText.ContainsProhibitedWords.LocalText);
			return false;
		}
		if (await _popupManager.ShowConfirmationTextPopupAsync(TLocalizedText.ConfirmPlayerName.Format(text)))
		{
			Close(text);
			return true;
		}
		return false;
	}

	private void OnConfirmButtonClick()
	{
		ConfirmAsync().Forget();
	}

	private void OnCancelButtonClick()
	{
		Close();
	}

	private bool ContainsSpecialCharacter(string text)
	{
		if (!string.IsNullOrEmpty(text))
		{
			for (int i = 0; i < text.Length; i++)
			{
				if (!char.IsLetterOrDigit(text[i]))
				{
					return true;
				}
			}
		}
		return false;
	}

	private int GetTextLength(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return 0;
		}
		return new StringInfo(text).LengthInTextElements;
	}
}
