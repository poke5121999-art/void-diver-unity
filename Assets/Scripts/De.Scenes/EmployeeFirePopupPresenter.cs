using System.Threading;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using Spine;
using Spine.Unity;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class EmployeeFirePopupPresenter : GamePopupPresenter
{
	[View]
	private EmployeeFirePopupView _view;

	[InjectModel]
	private EmployeeFirePopupModel _model;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private SoundManager _soundManager;

	private CancellationTokenSource _skeletonCts;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	public override bool IsInputActionPopup => true;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		EmployeeCardModel model = new EmployeeCardModel
		{
			EmployeeId = _model.EmployeeId,
			Level = _model.Level,
			Exp = _model.Exp,
			Stress = _model.Stress,
			IsPanic = _model.IsPanic,
			IsPlaced = _model.IsPlaced,
			SlotType = EEmployeeSlotType.Viewing,
			LearnedSkillIds = _model.LearnedSkillIds
		};
		_view.EmployeeInfoCard.Inject(model);
		InitBubbleText();
		CancelSkeletonLoad();
		_skeletonCts = new CancellationTokenSource();
		SetEmployeeSkeletonAsync(_skeletonCts.Token).Forget();
		_view.Confirm_Btn.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close(true);
		}).AddTo(this);
		_view.Cancel_Btn.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close(false);
		}).AddTo(this);
		_popupManager.OnBeforeOpenPopup.Subscribe(delegate
		{
			ResetSpinePhysics();
		}).AddTo(this);
		_popupManager.OnPopupClosed.Subscribe(delegate
		{
			ResetSpinePhysics();
		}).AddTo(this);
		InitGamePadNav();
	}

	private void ResetSpinePhysics()
	{
		ExposedList<PhysicsConstraint> physicsConstraints = _view.EmployeeSpine_Skeleton.Skeleton.PhysicsConstraints;
		for (int i = 0; i < physicsConstraints.Count; i++)
		{
			physicsConstraints.Items[i].Reset();
		}
	}

	public override void Close(object result = null)
	{
		CancelSkeletonLoad();
		base.Close(result);
	}

	private void CancelSkeletonLoad()
	{
		_skeletonCts?.Cancel();
		_skeletonCts?.Dispose();
		_skeletonCts = null;
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
		}
	}

	private async UniTaskVoid SetEmployeeSkeletonAsync(CancellationToken ct)
	{
		UnitSkeletonData unitSkeletonData = await _assetManager.GetNpcSkeletonDataAsync(TEmployee.Table[_model.EmployeeId].NpcId);
		if (ct.IsCancellationRequested || unitSkeletonData == null)
		{
			return;
		}
		SkeletonGraphic employeeSpine_Skeleton = _view.EmployeeSpine_Skeleton;
		employeeSpine_Skeleton.AnimationState?.ClearTracks();
		employeeSpine_Skeleton.skeletonDataAsset = unitSkeletonData.Sw;
		employeeSpine_Skeleton.Initialize(overwrite: true);
		employeeSpine_Skeleton.Skeleton.SetSkin(_model.EmployeeId.ToString());
		employeeSpine_Skeleton.Skeleton.SetSlotsToSetupPose();
		employeeSpine_Skeleton.Skeleton.UpdateCache();
		employeeSpine_Skeleton.AnimationState.SetAnimation(0, "idle", loop: true);
	}

	private void InitBubbleText()
	{
		TEmployee tEmployee = TEmployee.Table[_model.EmployeeId];
		string randomText = GetRandomText(_model.IsPanic ? tEmployee.BubbleTextsOnFirePanic : tEmployee.BubbleTextsOnFire);
		_view.EmployeeSpine_TextBubble.SetActive(!string.IsNullOrEmpty(randomText));
		if (!string.IsNullOrEmpty(randomText))
		{
			_view.EmployeeSpine_TextBubble_Text.gameObject.SetActive(!_model.IsPanic);
			_view.EmployeeSpine_TextBubble_TextPanic.gameObject.SetActive(_model.IsPanic);
			(_model.IsPanic ? _view.EmployeeSpine_TextBubble_TextPanic : _view.EmployeeSpine_TextBubble_Text).text = randomText;
			_soundManager.PlaySfx(GetRandomSaySfx());
		}
	}

	private static string GetRandomText(string[] texts)
	{
		if (texts == null || texts.Length == 0)
		{
			return null;
		}
		return texts[Random.Range(0, texts.Length)];
	}

	private static string GetRandomSaySfx()
	{
		return Random.Range(0, 3) switch
		{
			0 => "Employee_Say1",
			1 => "Employee_Say2",
			_ => "Employee_Say3",
		};
	}

	private void InitGamePadNav()
	{
		Selectable[] items = new Selectable[2] { _view.Confirm_Btn, _view.Cancel_Btn };
		SetNavHorizontal(items);
		SetDefaultNav(_view.Confirm_Btn);
	}
}
