using System.Threading;
using Cysharp.Threading.Tasks;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using SomeNet.Utility;
using Spine.Unity;
using UniRx;
using UnityEngine;
using VContainer;

public class ArtifactDealBackOutPopupPresenter : GamePopupPresenter
{
	private const float AutoCloseDelay = 2.5f;

	[InjectModel]
	private ArtifactDealBackOutPopupModel _model;

	[View]
	private ArtifactDealBackOutPopupView _view;

	[Inject]
	private AssetManager _assetManager;

	private CancellationTokenSource _skeletonCts;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	public override bool IsInputActionPopup => true;

	public override string OpenSfx => "CampaignFail";

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_skeletonCts = new CancellationTokenSource();
		SetNpcSkeletonAsync(_skeletonCts.Token).Forget();
		ShowTextBubble(PickBubbleText(TArtifactDeal.Table[_model.ArtifactDealId].BubbleTextsOnResultFail));
		AutoCloseAsync().Forget();
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
	}

	private async UniTaskVoid AutoCloseAsync()
	{
		if (!(await UniTask.Delay((int)(2.5f * 1000f), ignoreTimeScale: false, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy()).SuppressCancellationThrow()))
		{
			Close();
		}
	}

	private void ShowTextBubble(string message)
	{
		_view.TextBubble.SetActive(value: true);
		_view.TextBubble_Text.text = message;
	}

	private string PickBubbleText(string[] texts)
	{
		if (texts == null || texts.Length < 1)
		{
			return string.Empty;
		}
		return texts[RandomUtility.GetRandomInt(0, texts.Length)];
	}

	private async UniTaskVoid SetNpcSkeletonAsync(CancellationToken ct)
	{
		TArtifactDeal dealData = TArtifactDeal.Table[_model.ArtifactDealId];
		UnitSkeletonData unitSkeletonData = await _assetManager.GetNpcSkeletonDataAsync(dealData.SpineId);
		if (ct.IsCancellationRequested || unitSkeletonData == null)
		{
			return;
		}
		SkeletonGraphic spine = _view.Spine;
		spine.AnimationState?.ClearTracks();
		spine.skeletonDataAsset = unitSkeletonData.Sw;
		spine.Initialize(overwrite: true);
		if (dealData.MultiSkinId > 0)
		{
			spine.Skeleton.SetMultiSkin(dealData.MultiSkinId);
		}
		spine.Skeleton.SetSlotsToSetupPose();
		spine.Skeleton.UpdateCache();
		spine.AnimationState.SetAnimation(0, "idle", loop: true);
	}

	private void CancelSkeletonLoad()
	{
		_skeletonCts?.Cancel();
		_skeletonCts?.Dispose();
		_skeletonCts = null;
	}

	public override void Close(object result = null)
	{
		CancelSkeletonLoad();
		base.Close(result);
	}
}
