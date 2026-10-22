using Cysharp.Threading.Tasks;
using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;

public class TextPopupPresenter : GamePopupPresenter
{
	[InjectModel]
	private TextPopupModel _model;

	[View]
	private TextPopupView _view;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override string OpenSfx => "NPC_UI_Open_ETC";

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.Message.text = _model.Message;
		_view.AdditionalMessage.text = _model.AdditionalMessage;
		_view.Cancel.SetActive(_model.CommonPopupType == CommonPopupType.TWO_BUTTON);
		_view.Confirm_Button.OnClickWithSfx().Subscribe(delegate
		{
			Close(true);
		}).AddTo(this);
		_view.Cancel_Button.OnClickWithSfx().Subscribe(delegate
		{
			Close();
		}).AddTo(this);
		InitGamePadNav();
	}

	private void InitGamePadNav()
	{
		if (_model.CommonPopupType == CommonPopupType.TWO_BUTTON)
		{
			SetNavHorizontal(new Selectable[2] { _view.Confirm_Button, _view.Cancel_Button });
		}
		else
		{
			SetNavHorizontal(new Selectable[1] { _view.Confirm_Button });
		}
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

	private void OnCancel()
	{
		Close();
	}

	private void OnConfirm()
	{
		Close(true);
	}
}
