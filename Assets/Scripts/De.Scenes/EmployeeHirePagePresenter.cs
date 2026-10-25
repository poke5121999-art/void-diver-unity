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
using VContainer;

public class EmployeeHirePagePresenter : BasePanelPresenter
{
	[View]
	private EmployeeHirePageView _view;

	[InjectModel]
	private EmployeeHirePageModel _model;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private TycoonManager _tycoonManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private SoundManager _soundManager;

	private EmployeeScrollerModel _scrollerModel;

	private CompositeDisposable _disposable;

	private CompositeDisposable _inputDisposable;

	private CancellationTokenSource _skeletonCts;

	protected override BasePanelModel PanelModel => _model;

	private EmployeeScrollerPresenter _scrollerPresenter => _view.ScrollView;

	private void OnInject(EmployeeHirePageModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_skeletonCts?.Cancel();
		_skeletonCts?.Dispose();
		_skeletonCts = null;
		_view.EmployeeSpine.SetActive(false);
		_scrollerModel = new EmployeeScrollerModel
		{
			Mode = (EEmployeePopupMode)1
		};
		_scrollerModel.OnSlotClick.Subscribe(OnSlotClick).AddTo(_disposable);
		_scrollerModel.OnSlotEnter.Subscribe(OnSlotHover).AddTo(_disposable);
		_view.ScrollView.Inject(_scrollerModel);
		_tycoonManager.OnEmployeesChanged.Subscribe(delegate
		{
			RefreshScroller();
		}).AddTo(_disposable);
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
		_view.EmployeeSpine.SetActive(true);
		UpdateBubbleText(cardModel.EmployeeId);
		RequestEmployeeSkeleton(cardModel.EmployeeId);
	}

	private void OnSlotClick(EmployeeCardModel cardModel)
	{
		_view.EmployeeSpine.SetActive(true);
		UpdateBubbleText(cardModel.EmployeeId);
		RequestEmployeeSkeleton(cardModel.EmployeeId);
		OpenHireConfirmAsync(cardModel);
	}

	private void UpdateBubbleText(int employeeId)
	{
		string randomText = GetRandomText(TEmployee.Table[employeeId].BubbleTextsOnHire);
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
		// SetEmployeeSkeletonAsync MoveNext @0x1808DB700: skeletonDataAsset = *(unitSkeletonData + 0x20) = Sw (Nw is 0x18 and null in the employee assets)
		employeeSpine_Skeleton.skeletonDataAsset = unitSkeletonData.Sw;
		employeeSpine_Skeleton.Initialize(overwrite: true);
		employeeSpine_Skeleton.Skeleton.SetSkin(employeeId.ToString());
		employeeSpine_Skeleton.Skeleton.SetSlotsToSetupPose();
		employeeSpine_Skeleton.Skeleton.UpdateCache();
		employeeSpine_Skeleton.AnimationState.SetAnimation(0, "idle", loop: true);
	}

	private async UniTaskVoid OpenHireConfirmAsync(EmployeeCardModel cardModel)
	{
		TycoonHireConfirmPopupPresenter tycoonHireConfirmPopupPresenter = await _popupManager.OpenAsync<TycoonHireConfirmPopupPresenter>(new TycoonHireConfirmPopupModel
		{
			EmployeeId = cardModel.EmployeeId,
			Level = cardModel.Level,
			HireCost = cardModel.HireCost,
			LearnedSkillIds = cardModel.LearnedSkillIds
		});
		await tycoonHireConfirmPopupPresenter.Task;
	}

	private void RefreshScroller()
	{
		_view.ScrollView.Inject(_scrollerModel);
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
			_model.OnEscape.OnNext(Unit.Default);
		}).AddTo(_inputDisposable);
	}

	protected override void OnInputDisabled()
	{
		_inputDisposable?.Dispose();
		_inputDisposable = null;
		_scrollerModel.RxIsInputEnabled.Value = false;
		base.OnInputDisabled();
	}
}
