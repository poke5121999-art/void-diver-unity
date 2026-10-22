using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine.UI;
using VContainer;

public class TycoonHireConfirmPopupPresenter : GamePopupPresenter
{
	[View]
	private TycoonHireConfirmPopupView _view;

	[InjectModel]
	private TycoonHireConfirmPopupModel _model;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private TycoonManager _tycoonManager;

	[Inject]
	private ToastManager _toastManager;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private InputActions.UiKeyboardActions UiKeyboardActions => GameInputManager.UiKeyboardActions;

	public override bool IsInputActionPopup => true;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		EmployeeCardModel model = new EmployeeCardModel
		{
			EmployeeId = _model.EmployeeId,
			Level = _model.Level,
			SlotType = EEmployeeSlotType.Hire,
			HireCost = _model.HireCost,
			LearnedSkillIds = _model.LearnedSkillIds
		};
		_view.EmployeeInfoCard.Inject(model);
		_view.Confirm_Button.OnClickWithSfx().Subscribe(delegate
		{
			OnConfirm().Forget();
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
		UiKeyboardActions.Confirm.OnClickWithSfx().Subscribe(delegate
		{
			OnConfirm().Forget();
		}).AddTo(InputDisposable);
		UiKeyboardActions.Cancel.OnClickWithSfx().Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
	}

	private async UniTaskVoid OnConfirm()
	{
		if (_playerManager.Employees.Count >= TConst.MaxEmployeeCountBase.IntValue)
		{
			_toastManager.NoticeFail(TLocalizedText.AlreadyMaxEmployeeCount.LocalText);
			return;
		}
		if (_playerManager.Gold < _model.HireCost)
		{
			_toastManager.Notice(TLocalizedText.NotEnoughGold.LocalText);
			return;
		}
		bool flag = await _tycoonManager.TryHireEmployee(_model.EmployeeId);
		if (flag)
		{
			_toastManager.Notice(TLocalizedText.TycoonEmployeeHired.LocalText, "Employee_Stamp");
		}
		Close(flag);
	}

	private void InitGamePadNav()
	{
		SetNavHorizontal(new Selectable[2] { _view.Confirm_Button, _view.Cancel_Button });
		SetDefaultNav(_view.Confirm_Button);
	}
}
