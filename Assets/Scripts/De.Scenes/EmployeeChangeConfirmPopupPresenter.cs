using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;

public class EmployeeChangeConfirmPopupPresenter : GamePopupPresenter
{
	[View]
	private EmployeeChangeConfirmPopupView _view;

	[InjectModel]
	private EmployeeChangeConfirmPopupModel _model;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private InputActions.UiKeyboardActions UiKeyboardActions => GameInputManager.UiKeyboardActions;

	public override bool IsInputActionPopup => true;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		EmployeeCardModel model = new EmployeeCardModel
		{
			EmployeeId = _model.BeforeEmployeeId,
			Level = _model.BeforeLevel,
			Exp = _model.BeforeExp,
			Stress = _model.BeforeStress,
			IsPanic = _model.BeforeIsPanic,
			SlotType = EEmployeeSlotType.Viewing,
			LearnedSkillIds = _model.BeforeLearnedSkillIds
		};
		_view.BeforeEmployeeCard.Inject(model);
		EmployeeCardModel model2 = new EmployeeCardModel
		{
			EmployeeId = _model.AfterEmployeeId,
			Level = _model.AfterLevel,
			Exp = _model.AfterExp,
			Stress = _model.AfterStress,
			IsPanic = _model.AfterIsPanic,
			SlotType = EEmployeeSlotType.Viewing,
			LearnedSkillIds = _model.AfterLearnedSkillIds
		};
		_view.AfterEmployeeCard.Inject(model2);
		_view.Confirm_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close(true);
		}).AddTo(this);
		_view.Cancel_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close(false);
		}).AddTo(this);
		InitGamePadNav();
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		if (GameInputManager != null)
		{
			UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				Close(false);
			}).AddTo(InputDisposable);
			UiKeyboardActions.Confirm.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				Close(true);
			}).AddTo(InputDisposable);
			UiKeyboardActions.Cancel.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				Close(false);
			}).AddTo(InputDisposable);
		}
	}

	private void InitGamePadNav()
	{
		Selectable[] items = new Selectable[2] { _view.Confirm_Button, _view.Cancel_Button };
		SetNavHorizontal(items);
		SetDefaultNav(_view.Confirm_Button);
	}
}
