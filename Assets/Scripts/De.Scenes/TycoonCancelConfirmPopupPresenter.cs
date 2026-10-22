using Cysharp.Threading.Tasks;
using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;

public class TycoonCancelConfirmPopupPresenter : GamePopupPresenter
{
	[View]
	private TycoonCancelConfirmPopupView _view;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private InputActions.UiKeyboardActions UiKeyboardActions => GameInputManager.UiKeyboardActions;

	public override bool IsInputActionPopup => true;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.Confirm_Button.OnClickWithSfx().Subscribe(delegate
		{
			Close(true);
		}).AddTo(this);
		_view.Cancel_Button.OnClickWithSfx().Subscribe(delegate
		{
			Close(false);
		}).AddTo(this);
		_view.Background.OnClickWithSfx().Subscribe(delegate
		{
			Close(false);
		}).AddTo(this);
		InitGamePadNav();
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx().Subscribe(delegate
		{
			Close(false);
		}).AddTo(InputDisposable);
		UiKeyboardActions.Confirm.OnClickWithSfx().Subscribe(delegate
		{
			Close(true);
		}).AddTo(InputDisposable);
		UiKeyboardActions.Cancel.OnClickWithSfx().Subscribe(delegate
		{
			Close(false);
		}).AddTo(InputDisposable);
	}

	private void InitGamePadNav()
	{
		SetNavHorizontal(new Selectable[2] { _view.Confirm_Button, _view.Cancel_Button });
		SetDefaultNav(_view.Confirm_Button);
	}
}
