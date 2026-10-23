using Cysharp.Threading.Tasks;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class RoomCodeDisplayPopupPresenter : GamePopupPresenter
{
	[View]
	private RoomCodeDisplayPopupView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private IRoomMatchmaker _roomMatchmaker;

	[Inject]
	private ToastManager _toastManager;

	private bool _isCodeVisible;

	public override bool IsInputActionPopup => true;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.Confirm_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close();
		}).AddTo(this);
		_view.CopyButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnCopyButtonClick();
		}).AddTo(this);
		_view.OnOffButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnOnOffButtonClick();
		}).AddTo(this);
		_view.LockIcon.gameObject.SetActive(_roomMatchmaker.IsPrivateLobby);
		UpdateCodeVisibility();
		InitGamePadNav();
	}

	private void InitGamePadNav()
	{
		ConnectNav(_view.OnOffButton, ENavDir.Right, _view.CopyButton);
		ConnectNav(_view.OnOffButton, ENavDir.Down, _view.Confirm_Button);
		ConnectNav(_view.CopyButton, ENavDir.Left, _view.OnOffButton);
		ConnectNav(_view.CopyButton, ENavDir.Down, _view.Confirm_Button);
		ConnectNav(_view.Confirm_Button, ENavDir.Up, _view.CopyButton);
		SetDefaultNav(_view.Confirm_Button);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
	}

	private void OnCopyButtonClick()
	{
		string loungeCode = _roomMatchmaker.LoungeCode;
		_toastManager.Notice(TLocalizedText.CopyLoungeCode.LocalText);
		GUIUtility.systemCopyBuffer = loungeCode;
	}

	private void OnOnOffButtonClick()
	{
		_isCodeVisible = !_isCodeVisible;
		UpdateCodeVisibility();
	}

	private void UpdateCodeVisibility()
	{
		string loungeCode = _roomMatchmaker.LoungeCode;
		_view.OnOffButton_On.gameObject.SetActive(_isCodeVisible);
		_view.OnOffButton_Off.gameObject.SetActive(!_isCodeVisible);
		_view.CodeNum.text = (_isCodeVisible ? loungeCode : new string('*', loungeCode.Length));
	}
}
