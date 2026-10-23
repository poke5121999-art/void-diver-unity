using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using Spine.Unity;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;

public class EmployeeSettingPopupPresenter : GamePopupPresenter
{
	[View]
	private EmployeeSettingPopupView _view;

	[InjectModel]
	private EmployeeSettingPopupModel _model;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private TycoonManager _tycoonManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private ToastManager _toastManager;

	private EmployeeScrollerModel _scrollerModel;

	private int _originalEmployeeId;

	private Employee _selectedEmployee;

	private CancellationTokenSource _skeletonCts;

	private bool _isButtonMode;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	public override bool IsInputActionPopup => true;

	private EmployeeScrollerPresenter _scrollerPresenter => _view.ScrollView.GetComponent<EmployeeScrollerPresenter>();

	private SalesSlot SalesSlot => _playerManager.SalesSlots[_model.SalesSlotIndex];

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_originalEmployeeId = (_model.IsStarted ? SalesSlot.EmployeeId : _model.CachedEmployeeId);
		_scrollerModel = new EmployeeScrollerModel
		{
			Mode = EEmployeePopupMode.Setting
		};
		_scrollerModel.OnSlotClick.Subscribe(OnSlotClick).AddTo(this);
		_scrollerModel.OnSlotEnter.Subscribe(OnSlotHover).AddTo(this);
		_scrollerModel.OutOfRight.Subscribe(delegate
		{
			OnScrollerOutOfRight();
		}).AddTo(this);
		_scrollerPresenter.Inject(_scrollerModel);
		SelectInitialEmployee();
		UpdateEmployeeLayout();
		_view.Background.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close();
		}).AddTo(this);
		_view.BtnEmptyEmployee.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnEmptyEmployee();
		}).AddTo(this);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
		GameInputManager.UiPadActions.MoveScroll.OnPressStartAsObservable().Subscribe(OnMoveScrollStarted).AddTo(InputDisposable);
		_isButtonMode = false;
		_scrollerModel.RxIsInputEnabled.Value = true;
	}

	protected override void OnInputDisabled()
	{
		_scrollerModel.RxIsInputEnabled.Value = false;
		_isButtonMode = false;
		base.OnInputDisabled();
	}

	public override void Close(object result = null)
	{
		CancelSkeletonLoad();
		if (!_model.IsStarted)
		{
			result = _selectedEmployee?.Id ?? 0;
		}
		base.Close(result);
	}

	private void RefreshSkeletonToken()
	{
		_skeletonCts?.Cancel();
		_skeletonCts?.Dispose();
		_skeletonCts = null;
		_skeletonCts = new CancellationTokenSource();
	}

	private void CancelSkeletonLoad()
	{
		_skeletonCts?.Cancel();
		_skeletonCts?.Dispose();
		_skeletonCts = null;
	}

	private void OnSlotClick(EmployeeCardModel cardModel)
	{
		if (_playerManager.Employees.TryGetValue(cardModel.EmployeeId, out var employee) && !employee.IsPanic && !_playerManager.SalesSlots.Values.Any((SalesSlot s) => s.EmployeeId == cardModel.EmployeeId && s.Id != _model.SalesSlotIndex))
		{
			if (!_model.IsStarted)
			{
				ConfirmAndSelectEmployeeAsync(employee);
			}
			else
			{
				ConfirmAndChangeEmployeeAsync(employee);
			}
		}
	}

	private async UniTaskVoid ConfirmAndSelectEmployeeAsync(Employee employee)
	{
		if (await _popupManager.ShowConfirmationTextPopupAsync(TLocalizedText.TycoonEmployeeSettingConfirm.LocalText))
		{
			_selectedEmployee = employee;
			Close();
		}
	}

	private async UniTaskVoid ConfirmAndChangeEmployeeAsync(Employee newEmployee)
	{
		_playerManager.Employees.TryGetValue(_originalEmployeeId, out var value);
		EmployeeChangeConfirmPopupPresenter employeeChangeConfirmPopupPresenter = await _popupManager.OpenAsync<EmployeeChangeConfirmPopupPresenter>(new EmployeeChangeConfirmPopupModel
		{
			BeforeEmployeeId = value.Id,
			BeforeLevel = value.Level,
			BeforeExp = value.Exp,
			BeforeStress = value.Stress,
			BeforeIsPanic = value.IsPanic,
			BeforeLearnedSkillIds = value.LearnedSkillIds,
			AfterEmployeeId = newEmployee.Id,
			AfterLevel = newEmployee.Level,
			AfterExp = newEmployee.Exp,
			AfterStress = newEmployee.Stress,
			AfterIsPanic = newEmployee.IsPanic,
			AfterLearnedSkillIds = newEmployee.LearnedSkillIds
		});
		if ((bool)await employeeChangeConfirmPopupPresenter.Task)
		{
			ApplyEmployeeChange(newEmployee);
		}
	}

	private void ApplyEmployeeChange(Employee newEmployee)
	{
		_tycoonManager.SwapSalesSlotEmployee(SalesSlot, newEmployee, _model.Artifact);
		_toastManager.Notice(TLocalizedText.TycoonEmployeeChanged.LocalText);
		Close();
	}

	private void OnSlotHover(EmployeeCardModel cardModel)
	{
		if (!_playerManager.Employees.TryGetValue(cardModel.EmployeeId, out var value))
		{
			return;
		}
		RefreshSkeletonToken();
		bool flag = _model.IsStarted && _originalEmployeeId != 0 && value.Id != _originalEmployeeId;
		_view.EmployeeLayout_LeftEmployee.SetActive(flag);
		_view.EmployeeLayout_Arrow.SetActive(flag);
		if (flag && _playerManager.Employees.TryGetValue(_originalEmployeeId, out var value2))
		{
			SetEmployeeDisplay(value2, _view.EmployeeLayout_LeftEmployee_TextName, _view.EmployeeLayout_LeftEmployee_TextLv, _view.EmployeeLayout_LeftEmployee_ExpSlider, _view.EmployeeLayout_LeftEmployee_PanicSlider, _view.EmployeeLayout_LeftEmployee_PanicSlider_Gauge, _view.EmployeeLayout_LeftEmployee_PanicSlider_ConditionFace, _view.EmployeeLayout_LeftEmployee_Skeleton);
		}
		_view.EmployeeLayout_RightEmployee.SetActive(true);
		SetEmployeeDisplay(value, _view.EmployeeLayout_RightEmployee_TextName, _view.EmployeeLayout_RightEmployee_TextLv, _view.EmployeeLayout_RightEmployee_ExpSlider, _view.EmployeeLayout_RightEmployee_PanicSlider, _view.EmployeeLayout_RightEmployee_PanicSlider_Gauge, _view.EmployeeLayout_RightEmployee_PanicSlider_ConditionFace, _view.EmployeeLayout_RightEmployee_Skeleton);
	}

	private void SelectInitialEmployee()
	{
		if (_originalEmployeeId != 0 && _playerManager.Employees.TryGetValue(_originalEmployeeId, out var value))
		{
			_selectedEmployee = value;
		}
	}

	private void OnEmptyEmployee()
	{
		if (!_model.IsStarted)
		{
			_tycoonManager.ClearEmployee(SalesSlot);
		}
		else
		{
			_tycoonManager.UnregisterSalesSlot(SalesSlot);
		}
		_selectedEmployee = null;
		UpdateEmployeeLayout();
		_scrollerModel.Mode = EEmployeePopupMode.Setting;
		_scrollerPresenter.Inject(_scrollerModel);
	}

	private void UpdateEmployeeLayout()
	{
		RefreshSkeletonToken();
		Employee selectedEmployee = _selectedEmployee;
		bool flag = selectedEmployee != null;
		bool flag2 = flag && _model.IsStarted && _originalEmployeeId != 0 && selectedEmployee.Id != _originalEmployeeId;
		_view.EmployeeLayout_LeftEmployee.SetActive(flag2);
		_view.EmployeeLayout_Arrow.SetActive(flag2);
		if (flag2 && _playerManager.Employees.TryGetValue(_originalEmployeeId, out var value))
		{
			SetEmployeeDisplay(value, _view.EmployeeLayout_LeftEmployee_TextName, _view.EmployeeLayout_LeftEmployee_TextLv, _view.EmployeeLayout_LeftEmployee_ExpSlider, _view.EmployeeLayout_LeftEmployee_PanicSlider, _view.EmployeeLayout_LeftEmployee_PanicSlider_Gauge, _view.EmployeeLayout_LeftEmployee_PanicSlider_ConditionFace, _view.EmployeeLayout_LeftEmployee_Skeleton);
		}
		_view.EmployeeLayout_RightEmployee.SetActive(flag);
		if (selectedEmployee != null)
		{
			SetEmployeeDisplay(_selectedEmployee, _view.EmployeeLayout_RightEmployee_TextName, _view.EmployeeLayout_RightEmployee_TextLv, _view.EmployeeLayout_RightEmployee_ExpSlider, _view.EmployeeLayout_RightEmployee_PanicSlider, _view.EmployeeLayout_RightEmployee_PanicSlider_Gauge, _view.EmployeeLayout_RightEmployee_PanicSlider_ConditionFace, _view.EmployeeLayout_RightEmployee_Skeleton);
		}
		_view.BtnEmptyEmployee.gameObject.SetActive(selectedEmployee != null && !_model.IsStarted);
		if (_isButtonMode && !_view.BtnEmptyEmployee.gameObject.activeSelf)
		{
			ExitButtonMode();
		}
	}

	private void SetEmployeeDisplay(Employee employee, TextMeshProUGUI nameText, TextMeshProUGUI lvText, Slider expSlider, Slider panicSlider, Image panicGauge, Image conditionFace, SkeletonGraphic skeleton)
	{
		TEmployee tEmployee = TEmployee.Table[employee.Id];
		nameText.text = tEmployee.Name;
		lvText.text = TLocalizedText.LevelFormat.Format(employee.Level);
		EmployeeExpUtility.Apply(expSlider, employee.Level, employee.Exp);
		int intValue = TConst.StressMax.IntValue;
		panicSlider.value = 1f - ((intValue > 0) ? ((float)employee.Stress / (float)intValue) : 0f);
		conditionFace.sprite = _assetManager.GetIconEmployeeCondition(employee.Stress, employee.IsPanic);
		SetEmployeeSkeletonAsync(skeleton, tEmployee.NpcId, employee.Id, _skeletonCts.Token);
	}

	private async UniTaskVoid SetEmployeeSkeletonAsync(SkeletonGraphic skeleton, int npcId, int employeeId, CancellationToken ct)
	{
		UnitSkeletonData unitSkeletonData = await _assetManager.GetNpcSkeletonDataAsync(npcId);
		if (!ct.IsCancellationRequested && !(unitSkeletonData == null) && !(skeleton == null))
		{
			skeleton.AnimationState?.ClearTracks();
			skeleton.skeletonDataAsset = unitSkeletonData.Nw;
			skeleton.Initialize(overwrite: true);
			skeleton.PhysicsPositionInheritanceFactor = Vector2.zero;
			skeleton.Skeleton.SetSkin(employeeId.ToString());
			skeleton.Skeleton.SetSlotsToSetupPose();
			skeleton.Skeleton.UpdateCache();
			skeleton.AnimationState.SetAnimation(0, "idle", loop: true);
		}
	}

	private void RefreshScroller()
	{
		_scrollerModel.Mode = EEmployeePopupMode.Setting;
		_scrollerPresenter.Inject(_scrollerModel);
	}

	private void OnScrollerOutOfRight()
	{
		if (_view.BtnEmptyEmployee.gameObject.activeSelf)
		{
			_isButtonMode = true;
			_scrollerModel.RxIsInputEnabled.Value = false;
			SetDefaultNav(_view.BtnEmptyEmployee);
		}
	}

	private void OnMoveScrollStarted(InputAction.CallbackContext ctx)
	{
		if (_isButtonMode && ctx.ReadValue<Vector2>().x < -0.5f)
		{
			ExitButtonMode();
		}
	}

	private void ExitButtonMode()
	{
		if (_isButtonMode)
		{
			_isButtonMode = false;
			ClearNav();
			_scrollerModel.RxIsInputEnabled.Value = true;
		}
	}
}
