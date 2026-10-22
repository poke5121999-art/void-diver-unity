using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using Spine.Unity;
using TMPro;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using VContainer;

public class EmployeeInfoPagePresenter : BasePanelPresenter
{

	[View]
	private EmployeeInfoPageView _view;

	[InjectModel]
	private EmployeeInfoPageModel _model;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private TycoonManager _tycoonManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private SoundManager _soundManager;

	private EmployeeScrollerModel _scrollerModel;

	private CompositeDisposable _disposable;

	private CompositeDisposable _inputDisposable;

	private CancellationTokenSource _skeletonCts;

	private EmployeeCardModel _selectedCardModel;

	private bool _isButtonMode;

	protected override BasePanelModel PanelModel => _model;

	private EmployeeScrollerPresenter _scrollerPresenter => _view.ScrollView;

	private void OnInject(EmployeeInfoPageModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_skeletonCts?.Cancel();
		_skeletonCts?.Dispose();
		_skeletonCts = null;
		_selectedCardModel = null;
		_view.EmployeeSpine.SetActive(false);
		_scrollerModel = new EmployeeScrollerModel
		{
			Mode = EEmployeePopupMode.Info
		};
		_scrollerModel.OnSlotClick.Subscribe(OnSlotClick).AddTo(_disposable);
		_scrollerModel.OnSlotEnter.Subscribe(OnSlotHover).AddTo(_disposable);
		_scrollerModel.OnCleansingClick.Subscribe(OnCleansingBtnClicked).AddTo(_disposable);
		_scrollerModel.OnFireClick.Subscribe(OnFireBtnClicked).AddTo(_disposable);
		_view.ScrollView.Inject(_scrollerModel);
		_tycoonManager.OnEmployeesChanged.Subscribe(delegate
		{
			RefreshScroller();
			RefreshCountText();
		}).AddTo(_disposable);
		_tycoonManager.OnSalesSlotsChanged.Subscribe(delegate
		{
			RefreshScroller();
		}).AddTo(_disposable);
		RefreshCountText();
		model.RxIsInputEnabled.Where((bool x) => x).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool x) => !x).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
		_popupManager.OnBeforeOpenPopup.Subscribe(delegate
		{
			ResetSpinePhysics();
		}).AddTo(_disposable);
		_popupManager.OnPopupClosed.Subscribe(delegate
		{
			ResetSpinePhysics();
		}).AddTo(_disposable);
	}

	private void ResetSpinePhysics()
	{
		Spine.Skeleton skeleton = _view.EmployeeSpine_Skeleton.Skeleton;
		if (skeleton == null)
		{
			return;
		}
		Spine.ExposedList<Spine.PhysicsConstraint> physicsConstraints = skeleton.PhysicsConstraints;
		if (physicsConstraints == null)
		{
			return;
		}
		for (int i = 0; i < physicsConstraints.Count; i++)
		{
			physicsConstraints.Items[i].Reset();
		}
	}

	protected override void OnDestroy()
	{
		_inputDisposable?.Dispose();
		_disposable?.Dispose();
		_skeletonCts?.Cancel();
		_skeletonCts?.Dispose();
		_skeletonCts = null;
		base.OnDestroy();
	}

	private void OnSlotHover(EmployeeCardModel cardModel)
	{
		if (_selectedCardModel == null)
		{
			_view.EmployeeSpine.SetActive(true);
			UpdateEmployeeDisplay(cardModel);
			RequestEmployeeSkeleton(cardModel.EmployeeId);
		}
	}

	private void OnSlotClick(EmployeeCardModel cardModel)
	{
		if (_selectedCardModel != null && _selectedCardModel.EmployeeId == cardModel.EmployeeId)
		{
			_selectedCardModel = null;
			ExitButtonMode();
			return;
		}
		_selectedCardModel = cardModel;
		_model.OnEmployeeClick.OnNext(cardModel.EmployeeId);
		_view.EmployeeSpine.SetActive(true);
		UpdateEmployeeDisplay(cardModel);
		RequestEmployeeSkeleton(cardModel.EmployeeId);
		EnterButtonMode(cardModel);
	}

	private void UpdateEmployeeDisplay(EmployeeCardModel cardModel)
	{
		TEmployee tEmployee = TEmployee.Table[cardModel.EmployeeId];
		bool flag = _playerManager.SalesSlots.Values.Any((SalesSlot s) => s.EmployeeId == cardModel.EmployeeId);
		bool flag2 = flag && IsNearPanic(cardModel.EmployeeId);
		string[] texts = (cardModel.IsPanic ? tEmployee.BubbleTextsOnInfoPanic : (!flag ? tEmployee.BubbleTextsOnInfo : (flag2 ? tEmployee.BubbleTextsOnSellingHighStress : tEmployee.BubbleTextsOnSelling)));
		string randomText = GetRandomText(texts);
		_view.EmployeeSpine_TextBubble.SetActive(!string.IsNullOrEmpty(randomText));
		TMP_Text componentInChildren = _view.EmployeeSpine_TextBubble.GetComponentInChildren<TMP_Text>();
		if (componentInChildren != null)
		{
			componentInChildren.text = randomText ?? string.Empty;
		}
		if (!string.IsNullOrEmpty(randomText))
		{
			_soundManager.PlaySfx(GetRandomSaySfx());
		}
	}

	private bool IsNearPanic(int employeeId)
	{
		if (!_playerManager.Employees.TryGetValue(employeeId, out var value))
		{
			return false;
		}
		SalesSlot salesSlot = _playerManager.SalesSlots.Values.FirstOrDefault((SalesSlot s) => s.EmployeeId == employeeId);
		if (salesSlot == null || salesSlot.Goods == null)
		{
			return false;
		}
		return _tycoonManager.CalculateTurnsUntilPanic(value, salesSlot.Goods) <= 1;
	}

	private static string GetRandomText(string[] texts)
	{
		if (texts == null || texts.Length == 0)
		{
			return null;
		}
		return texts[UnityEngine.Random.Range(0, texts.Length)];
	}

	private static string GetRandomSaySfx()
	{
		return UnityEngine.Random.Range(0, 3) switch
		{
			0 => "Employee_Say1",
			1 => "Employee_Say2",
			_ => "Employee_Say3",
		};
	}

	private void RequestEmployeeSkeleton(int employeeId)
	{
		_skeletonCts?.Cancel();
		_skeletonCts?.Dispose();
		_skeletonCts = null;
		_skeletonCts = new CancellationTokenSource();
		SetEmployeeSkeletonAsync(employeeId, _skeletonCts.Token);
	}

	private void CancelSkeletonLoad()
	{
		_skeletonCts?.Cancel();
		_skeletonCts?.Dispose();
		_skeletonCts = null;
	}

	private async UniTaskVoid SetEmployeeSkeletonAsync(int employeeId, CancellationToken ct)
	{
		UnitSkeletonData unitSkeletonData = await _assetManager.GetNpcSkeletonDataAsync(TEmployee.Table[employeeId].NpcId);
		if (ct.IsCancellationRequested || unitSkeletonData == null)
		{
			return;
		}
		SkeletonGraphic employeeSpine_Skeleton = _view.EmployeeSpine_Skeleton;
		employeeSpine_Skeleton.AnimationState?.ClearTracks();
		employeeSpine_Skeleton.skeletonDataAsset = unitSkeletonData.Nw;
		employeeSpine_Skeleton.Initialize(overwrite: true);
		employeeSpine_Skeleton.Skeleton.SetSkin(employeeId.ToString());
		employeeSpine_Skeleton.Skeleton.SetSlotsToSetupPose();
		employeeSpine_Skeleton.Skeleton.UpdateCache();
		employeeSpine_Skeleton.AnimationState.SetAnimation(0, "idle", loop: true);
	}

	private void OnCleansingBtnClicked(EmployeeCardModel cardModel)
	{
		PurifyEmployeeAsync(cardModel);
	}

	private async UniTaskVoid PurifyEmployeeAsync(EmployeeCardModel cardModel)
	{
		if (!_playerManager.Employees.TryGetValue(cardModel.EmployeeId, out var employee))
		{
			return;
		}
		if (_playerManager.SalesSlots.Values.Any((SalesSlot s) => s.EmployeeId == cardModel.EmployeeId))
		{
			_toastManager.NoticeFail(TLocalizedText.PurifyPlacedEmployeeMessage.LocalText);
			return;
		}
		if (employee.Stress <= 0)
		{
			_toastManager.NoticeFail(TLocalizedText.EmployeeNoStress.LocalText);
			return;
		}
		long num = _tycoonManager.CalculatePurifyCost(employee.Level);
		if (_playerManager.Gold < num)
		{
			_toastManager.NoticeFail(TLocalizedText.NotEnoughGold.LocalText);
			return;
		}
		ConsumeConfirmPopupPresenter consumeConfirmPopupPresenter = await _popupManager.OpenAsync<ConsumeConfirmPopupPresenter>(new ConsumeConfirmPopupModel
		{
			TitleText = TLocalizedText.EmployeePurifyConsumeConfirmTitle.LocalText,
			DescText = TLocalizedText.EmployeePurifyConsumeConfirmDesc.LocalText,
			GoldAmount = num
		});
		if ((bool)await consumeConfirmPopupPresenter.Task && await _tycoonManager.PurifyEmployeeAsync(cardModel.EmployeeId))
		{
			_toastManager.Notice(TLocalizedText.TycoonEmployeePurified.LocalText, "ParadoxHeal");
		}
	}

	private void OnFireBtnClicked(EmployeeCardModel cardModel)
	{
		OpenFireConfirmAsync(cardModel);
	}

	private async UniTaskVoid OpenFireConfirmAsync(EmployeeCardModel cardModel)
	{
		HashSet<int> hashSet = new HashSet<int>(_playerManager.SalesSlots.Values.Where((SalesSlot s) => s.EmployeeId > 0).Select((SalesSlot s) => s.EmployeeId));
		if (hashSet.Contains(cardModel.EmployeeId))
		{
			_toastManager.NoticeFail(TLocalizedText.CannotFirePlacedEmployee.LocalText);
			return;
		}
		EmployeeFirePopupPresenter employeeFirePopupPresenter = await _popupManager.OpenAsync<EmployeeFirePopupPresenter>(new EmployeeFirePopupModel
		{
			EmployeeId = cardModel.EmployeeId,
			Level = cardModel.Level,
			Exp = cardModel.Exp,
			Stress = cardModel.Stress,
			IsPanic = cardModel.IsPanic,
			IsPlaced = hashSet.Contains(cardModel.EmployeeId),
			LearnedSkillIds = cardModel.LearnedSkillIds
		});
		if ((bool)await employeeFirePopupPresenter.Task)
		{
			_tycoonManager.TryFireEmployee(cardModel.EmployeeId);
			_toastManager.Notice(TLocalizedText.TycoonEmployeeFired.LocalText, "Employee_Fire");
			_selectedCardModel = null;
			CancelSkeletonLoad();
			_view.EmployeeSpine.SetActive(false);
			ExitButtonMode();
		}
	}

	private void RefreshScroller()
	{
		_scrollerModel.Mode = EEmployeePopupMode.Info;
		_view.ScrollView.Inject(_scrollerModel);
	}

	private void RefreshCountText()
	{
		_view.CountText.text = string.Format("{0}/{1}", _playerManager.Employees.Count, TConst.MaxEmployeeCountBase.IntValue);
	}

	protected override void OnInputEnabled()
	{
		_scrollerModel.RxIsInputEnabled.Value = true;
		base.OnInputEnabled();
		SuspendNav();
		_inputDisposable?.Dispose();
		_inputDisposable = new CompositeDisposable();
		(InputManager as GameInputManager).UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnEscapePressed();
		}).AddTo(_inputDisposable);
	}

	protected override void OnInputDisabled()
	{
		_inputDisposable?.Dispose();
		_inputDisposable = null;
		_scrollerModel.RxIsInputEnabled.Value = false;
		_isButtonMode = false;
		ClearNav();
		base.OnInputDisabled();
	}

	private void OnEscapePressed()
	{
		if (!_isButtonMode)
		{
			_model.OnEscape.OnNext(Unit.Default);
			return;
		}
		if (_selectedCardModel != null)
		{
			_selectedCardModel.RxIsSelected.Value = false;
			_selectedCardModel = null;
		}
		ExitButtonMode();
	}

	private void EnterButtonMode(EmployeeCardModel cardModel)
	{
		if (global::InputManager.IsGamePad)
		{
			EmployeeCardView cardView = _view.ScrollView.GetCardView(cardModel.EmployeeId);
			if (!(cardView == null))
			{
				_isButtonMode = true;
				_scrollerModel.RxIsInputEnabled.Value = false;
				ClearNav();
				ConnectNav(cardView.Btns_BtnCleansing, ENavDir.Up, cardView.Btns_BtnFire);
				ConnectNav(cardView.Btns_BtnFire, ENavDir.Down, cardView.Btns_BtnCleansing);
				SetDefaultNav(cardView.Btns_BtnCleansing);
				ResumeNav();
			}
		}
	}

	private void ExitButtonMode()
	{
		if (_isButtonMode)
		{
			_isButtonMode = false;
			ClearNav();
			SuspendNav();
			_scrollerModel.RxIsInputEnabled.Value = true;
		}
	}
}
