using Cysharp.Threading.Tasks;
using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;

public class SamplePopupPresenter : GamePopupPresenter
{
	[InjectModel]
	private SamplePopupModel _model;

	[View]
	private SamplePopupView _view;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	public override bool IsInputActionPopup => true;

	public override string OpenSfx => "PopUpOpen";

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.Confirm_Button.OnClickWithSfx().Subscribe(delegate
		{
			Close();
		}).AddTo(this);
		_view.Cancel_Button.OnClickWithSfx().Subscribe(delegate
		{
			Close();
		}).AddTo(this);
		SetNavHorizontal(new Selectable[2] { _view.Confirm_Button, _view.Cancel_Button });
		SetDefaultNav(_view.Confirm_Button);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx().Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
	}

	private void InitGamePadNav()
	{
		SetNavHorizontal(new Selectable[2] { _view.Confirm_Button, _view.Cancel_Button });
		SetDefaultNav(_view.Confirm_Button);
	}

	private void OnConfirm()
	{
		Close();
	}

	private void OnCancel()
	{
		Close();
	}
}
