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
using VContainer;

public class EmployeeLevelUpPopupPresenter : GamePopupPresenter
{
	[View]
	private EmployeeLevelUpPopupView _view;

	[InjectModel]
	private EmployeeLevelUpPopupModel _model;

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
		_soundManager.PlaySfx("Employee_LevelUp");
		EmployeeCardModel model = new EmployeeCardModel
		{
			EmployeeId = _model.EmployeeId,
			Level = _model.NewLevel,
			Exp = _model.Exp,
			Stress = _model.Stress,
			PrevLevel = _model.PrevLevel,
			SlotType = EEmployeeSlotType.Viewing,
			IsLevelUp = true,
			LearnedSkillIds = _model.LearnedSkillIds,
			NewlyLearnedSkillIds = _model.NewlyLearnedSkillIds
		};
		_view.EmployeeCard.Inject(model);
		InitTitle();
		InitBubbleText();
		CancelSkeletonLoad();
		_skeletonCts = new CancellationTokenSource();
		SetEmployeeSkeletonAsync(_skeletonCts.Token).Forget();
		_view.Confirm_Btn.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close();
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
		ExposedList<PhysicsConstraint> physicsConstraints = _view.Skeleton.Skeleton.PhysicsConstraints;
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
				Close();
			}).AddTo(InputDisposable);
		}
	}

	private void InitTitle()
	{
		string name = TEmployee.Table[_model.EmployeeId].Name;
		_view.Title_TextMain.text = string.Format(TLocalizedText.EmployeeLevelUpTitleFormat.LocalText, name);
	}

	private void InitBubbleText()
	{
		string randomText = GetRandomText(TEmployee.Table[_model.EmployeeId].BubbleTextsOnLevelUp);
		_view.TextBubble.SetActive(!string.IsNullOrEmpty(randomText));
		if (!string.IsNullOrEmpty(randomText))
		{
			_view.TextBubble_Text.text = randomText;
		}
	}

	private async UniTaskVoid SetEmployeeSkeletonAsync(CancellationToken ct)
	{
		UnitSkeletonData unitSkeletonData = await _assetManager.GetNpcSkeletonDataAsync(TEmployee.Table[_model.EmployeeId].NpcId);
		if (ct.IsCancellationRequested || unitSkeletonData == null)
		{
			return;
		}
		SkeletonGraphic skeleton = _view.Skeleton;
		skeleton.AnimationState?.ClearTracks();
		// SetEmployeeSkeletonAsync MoveNext @0x18097C720: skeletonDataAsset = *(unitSkeletonData + 0x20) = Sw (Nw is 0x18 and null in the employee assets)
		skeleton.skeletonDataAsset = unitSkeletonData.Sw;
		skeleton.Initialize(overwrite: true);
		skeleton.Skeleton.SetSkin(_model.EmployeeId.ToString());
		skeleton.Skeleton.SetSlotsToSetupPose();
		skeleton.Skeleton.UpdateCache();
		skeleton.AnimationState.SetAnimation(0, "idle", loop: true);
	}

	private static string GetRandomText(string[] texts)
	{
		if (texts == null || texts.Length == 0)
		{
			return null;
		}
		return texts[Random.Range(0, texts.Length)];
	}

	private void InitGamePadNav()
	{
		SetDefaultNav(_view.Confirm_Btn);
	}
}
