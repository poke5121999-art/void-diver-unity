using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using SomeNet.Utility;
using Spine.Unity;
using UniRx;
using UnityEngine;
using VContainer;

public class ArtifactDealCompletePopupPresenter : GamePopupPresenter
{
	private const float TextBubbleToCoinDelay = 0.4f;

	private const float CoinAttractionDuration = 0.8f;

	private const float PriceTweenDuration = 0.5f;

	private const float ExpTweenDuration = 0.5f;

	private const float AutoCloseHoldAfterSequence = 1.5f;

	[InjectModel]
	private ArtifactDealCompletePopupModel _model;

	[View]
	private ArtifactDealCompletePopupView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private SoundManager _soundManager;

	private CancellationTokenSource _skeletonCts;

	private Tweener _priceTweener;

	private Tweener _expTweener;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	public override bool IsInputActionPopup => true;

	public override string OpenSfx => "ArtifactDeal_Success";

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_skeletonCts = new CancellationTokenSource();
		SetNpcSkeletonAsync(_skeletonCts.Token).Forget();
		_view.GoodsSlot.Inject(new InventoryGoodsSlotModel(_model.DealGoods, InventoryGoodsSlotPresenter.ELayout.Normal));
		SetDealPrice(_model.InitialCoin, 0);
		SetDealExp(0);
		PlayCompleteSequenceAsync().Forget();
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
	}

	private async UniTaskVoid PlayCompleteSequenceAsync()
	{
		CancellationToken ct = this.GetCancellationTokenOnDestroy();
		ShowTextBubble(PickBubbleText(TArtifactDeal.Table[_model.ArtifactDealId].BubbleTextsOnResultSuccess));
		if (await WaitAsync(TextBubbleToCoinDelay, ct))
		{
			return;
		}
		_view.CoinAttraction.SetActive(value: false);
		_view.CoinAttraction.SetActive(value: true);
		if (await WaitAsync(CoinAttractionDuration, ct))
		{
			return;
		}
		SetDealPrice(_model.FinalCoin, _model.BalancePercent, PriceTweenDuration);
		if (await WaitAsync(PriceTweenDuration, ct))
		{
			return;
		}
		SetDealExp(_model.GainExp, ExpTweenDuration);
		if (await WaitAsync(ExpTweenDuration, ct))
		{
			return;
		}
		if (!(await WaitAsync(AutoCloseHoldAfterSequence, ct)))
		{
			Close();
		}
	}

	private void SetDealPrice(int targetCoin, int balancePercent, float duration = 0f)
	{
		_priceTweener?.Kill();
		_priceTweener = null;
		_view.Gold_TextBalanceValue.text = ((balancePercent == 0) ? string.Empty : string.Format("{0}%", balancePercent));
		_view.Gold_TextBalanceValue.color = ((balancePercent >= 0) ? new Color32(90, 113, 74, byte.MaxValue) : new Color32(148, 77, 78, byte.MaxValue));
		_view.Gold_TextBalanceValue_BalanceUp.gameObject.SetActive(balancePercent > 0);
		_view.Gold_TextBalanceValue_BalanceDown.gameObject.SetActive(balancePercent < 0);
		_view.Gold_TextBalanceValue_BalanceNone.gameObject.SetActive(balancePercent == 0);
		if (duration <= 0f)
		{
			_view.Gold_TextBalanceValue_TextValue.text = targetCoin.ToString();
			return;
		}
		_soundManager.PlaySfx("ArtifactDeal_Rolling");
		int current = (int.TryParse(_view.Gold_TextBalanceValue_TextValue.text, out var result) ? result : 0);
		_priceTweener = DOTween.To(() => current, delegate(int x)
		{
			current = x;
			_view.Gold_TextBalanceValue_TextValue.text = x.ToString();
		}, targetCoin, duration).SetEase(Ease.OutQuad).SetLink(base.gameObject);
	}

	private void SetDealExp(int targetExp, float duration = 0f)
	{
		_expTweener?.Kill();
		_expTweener = null;
		if (duration <= 0f)
		{
			_view.Exp_TextExp_TextValue.text = targetExp.ToString();
			return;
		}
		_soundManager.PlaySfx("ArtifactDeal_Rolling");
		int current = (int.TryParse(_view.Exp_TextExp_TextValue.text, out var result) ? result : 0);
		_expTweener = DOTween.To(() => current, delegate(int x)
		{
			current = x;
			_view.Exp_TextExp_TextValue.text = x.ToString();
		}, targetExp, duration).SetEase(Ease.OutQuad).SetLink(base.gameObject);
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

	private async UniTask<bool> WaitAsync(float seconds, CancellationToken ct)
	{
		return await UniTask.Delay((int)(seconds * 1000f), ignoreTimeScale: false, PlayerLoopTiming.Update, ct).SuppressCancellationThrow();
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
