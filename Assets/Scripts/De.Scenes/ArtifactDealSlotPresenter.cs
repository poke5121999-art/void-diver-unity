using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ArtifactDealSlotPresenter : BaseSlotPresenter<ArtifactDealSlotModel>
{
	[InjectModel]
	private ArtifactDealSlotModel _model;

	[View]
	private ArtifactDealSlotView _view;

	[Inject]
	private ArtifactDealManager _artifactDealManager;

	protected override ArtifactDealSlotModel SlotModel => _model;

	protected override Button Button => _view.Slot;

	public int PrefixId => _model.ArtifactPrefixId;

	public bool IsMatched { get; private set; }

	private bool IsPreferPrefix => _model.IsPreferPrefix;

	private bool IsNegativeFramePrefix => TArtifactPrefix.Table[_model.ArtifactPrefixId].PriceMultiplier < 0f;

	private void UpdateMatch()
	{
		if (_model.ArtifactCategory != EArtifactCategory.None)
		{
			return;
		}
		EArtifactDealState value = _artifactDealManager.RxDealState.Value;
		if (value == EArtifactDealState.AwaitSelectArtifact || value == EArtifactDealState.AwaitSwapArtifact)
		{
			IsMatched = false;
			_view.FrameNegativePrefix_Outline.gameObject.SetActive(false);
			_view.FramePositivePrefix_Outline.gameObject.SetActive(false);
			_view.FrameNegativePrefix_positive.SetActive(false);
			_view.FrameNegativePrefix_negative.SetActive(false);
			_view.FramePositivePrefix_positive.SetActive(false);
			_view.FramePositivePrefix_negative.SetActive(false);
			return;
		}
		int[] prefixes = _artifactDealManager.SelectedGoods.GetPrefixes();
		bool flag = prefixes != null && prefixes.Contains(_model.ArtifactPrefixId);
		IsMatched = flag;
		if (IsNegativeFramePrefix)
		{
			_view.FrameNegativePrefix_Outline.gameObject.SetActive(flag);
			UpdateBenefit(_view.FrameNegativePrefix_positive, _view.FrameNegativePrefix_positive_TextNum, _view.FrameNegativePrefix_negative, _view.FrameNegativePrefix_negative_TextNum, flag);
		}
		else
		{
			_view.FramePositivePrefix_Outline.gameObject.SetActive(flag);
			UpdateBenefit(_view.FramePositivePrefix_positive, _view.FramePositivePrefix_positive_TextNum, _view.FramePositivePrefix_negative, _view.FramePositivePrefix_negative_TextNum, flag);
		}
	}

	public void SetHoverMatchedOutline(int[] hoveredPrefixes)
	{
		if (_model.ArtifactCategory != EArtifactCategory.None)
		{
			return;
		}
		if (hoveredPrefixes == null)
		{
			UpdateMatch();
			return;
		}
		bool active = hoveredPrefixes.Contains(_model.ArtifactPrefixId);
		(IsNegativeFramePrefix ? _view.FrameNegativePrefix_Outline : _view.FramePositivePrefix_Outline).gameObject.SetActive(active);
	}

	private void UpdateBenefit(GameObject positiveBadge, TextMeshProUGUI positiveTextNum, GameObject negativeBadge, TextMeshProUGUI negativeTextNum, bool isMatched)
	{
		if (!isMatched)
		{
			positiveBadge.SetActive(false);
			negativeBadge.SetActive(false);
			return;
		}
		TArtifactPrefix tArtifactPrefix = TArtifactPrefix.Table[_model.ArtifactPrefixId];
		int num = (_model.IsPreferPrefix ? tArtifactPrefix.PreferPriceBonusPercent : tArtifactPrefix.DislikePricePenaltyPercent);
		bool flag = num >= 0;
		positiveBadge.SetActive(flag);
		negativeBadge.SetActive(!flag);
		if (flag)
		{
			positiveTextNum.text = string.Format("+{0}%", num);
		}
		else
		{
			negativeTextNum.text = string.Format("{0}%", num);
		}
	}

	public async UniTask PlayRemoveAnimationAsync(float duration, CancellationToken ct)
	{
		GameObject cancel = _view.Cancel;
		if (cancel != null)
		{
			cancel.SetActive(true);
		}
		await UniTask.Delay((int)(duration * 1000f), false, PlayerLoopTiming.Update, ct).SuppressCancellationThrow();
		if (!ct.IsCancellationRequested)
		{
			base.gameObject.SetActive(false);
			if (cancel != null)
			{
				cancel.SetActive(false);
			}
		}
	}

	public async UniTask PlayAddAnimationAsync(float duration, CancellationToken ct)
	{
		GameObject gameObject = (IsNegativeFramePrefix ? _view.FrameNegativePrefix_ApearFx : _view.FramePositivePrefix_ApearFx);
		if (gameObject != null)
		{
			gameObject.SetActive(false);
			gameObject.SetActive(true);
		}
		CanvasGroup canvasGroup = GetOrAddCanvasGroup();
		canvasGroup.alpha = 0f;
		base.transform.localScale = Vector3.one * 0.5f;
		Tween tween = canvasGroup.DOFade(1f, duration).SetLink(base.gameObject);
		Tween tween2 = base.transform.DOScale(1f, duration).SetEase(Ease.OutBack).SetLink(base.gameObject);
		await UniTask.WhenAll(tween.AsyncWaitForCompletion().AsUniTask(), tween2.AsyncWaitForCompletion().AsUniTask());
	}

	private CanvasGroup GetOrAddCanvasGroup()
	{
		CanvasGroup component = GetComponent<CanvasGroup>();
		if (component == null)
		{
			component = base.gameObject.AddComponent<CanvasGroup>();
		}
		return component;
	}

	private void OnInject(ArtifactDealSlotModel model)
	{
		InitSlot(model);
		if (model.ArtifactCategory == EArtifactCategory.None)
		{
			bool isNegativeFramePrefix = IsNegativeFramePrefix;
			_view.FrameCategory.gameObject.SetActive(false);
			_view.FramePositivePrefix.gameObject.SetActive(!isNegativeFramePrefix);
			_view.FrameNegativePrefix.gameObject.SetActive(isNegativeFramePrefix);
			TArtifactPrefix tArtifactPrefix = TArtifactPrefix.Table[model.ArtifactPrefixId];
			if (isNegativeFramePrefix)
			{
				_view.FrameNegativePrefix_Text.text = tArtifactPrefix.Name;
			}
			else
			{
				_view.FramePositivePrefix_Text.text = tArtifactPrefix.Name;
			}
		}
		else
		{
			_view.FrameCategory.gameObject.SetActive(true);
			_view.FramePositivePrefix.gameObject.SetActive(false);
			_view.FrameNegativePrefix.gameObject.SetActive(false);
			_view.FrameCategory_Text.text = model.ArtifactCategory.ToLocalText();
		}
		UpdateMatch();
	}
}
