using System;
using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using UniRx;
using UniRx.Triggers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ArtifactDealCardPresenter : BaseSlotPresenter<ArtifactDealCardModel>
{
	[InjectModel]
	private ArtifactDealCardModel _model;

	[View]
	private ArtifactDealCardView _view;

	[Inject]
	private ArtifactDealManager _artifactDealManager;

	[SerializeField]
	private TextMeshProUGUI _textPercent;

	[SerializeField]
	private TextMeshProUGUI _textTitle;

	private bool _isInteractable;

	protected override ArtifactDealCardModel SlotModel => _model;

	protected override void Start()
	{
		if (Button != null)
		{
			Button.OnClick().Subscribe(delegate
			{
				OnClick();
			}).AddTo(this);
		}
		if (ButtonController != null)
		{
			ButtonController.OnEnter.Subscribe(delegate
			{
				OnSlotEnter();
			}).AddTo(this);
		}
		if (ButtonController != null)
		{
			ButtonController.OnExit.Subscribe(delegate
			{
				OnSlotExit();
			}).AddTo(this);
		}
	}

	private void OnInject(ArtifactDealCardModel model)
	{
		InitSlot(model);
		TArtifactDealCard data = TArtifactDealCard.Table[_model.Id];
		_textTitle.text = data.Name;
		string text = string.Empty;
		if (data.SuccessEffects != null && data.SuccessEffects.Length > 0)
		{
			text = text + TLocalizedText.ArtifactDealCardPlaySuccessHeader.LocalText;
			text = data.SuccessEffects.Aggregate(text, (string current, ArtifactDealCardEffect effect) => current + "\n" + effect.EffectType.ToDescription(effect.Value));
		}
		if (data.FailEffects != null && data.FailEffects.Length > 0)
		{
			text = text + "\n" + TLocalizedText.ArtifactDealCardPlayFailureHeader.LocalText;
			text = data.FailEffects.Aggregate(text, (string current, ArtifactDealCardEffect effect) => current + "\n" + effect.EffectType.ToDescription(effect.Value));
		}
		_view.TextDesc.text = text;
		Color32 color = _model.Rarity switch
		{
			EArtifactDealCardRarityType.Normal => new Color32(112, 112, 112, byte.MaxValue),
			EArtifactDealCardRarityType.Rare => new Color32(46, 155, 143, byte.MaxValue),
			EArtifactDealCardRarityType.Elite => new Color32(0, 144, byte.MaxValue, byte.MaxValue),
			EArtifactDealCardRarityType.Epic => new Color32(193, 65, 204, byte.MaxValue),
			EArtifactDealCardRarityType.Legend => new Color32(byte.MaxValue, 143, 43, byte.MaxValue),
			_ => Color.white,
		};
		_view.Line.color = color;
		_view.Deco.color = color;
		UpdateSuccessPercent();
		UpdateLocked();
		GetComponent<Button>().OnPointerDownAsObservable().Subscribe(delegate
		{
			if (!_model.RxIsLocked.Value)
			{
				_model.RxIsPressed.Value = true;
			}
		}).AddTo(Disposable);
		GetComponent<Button>().OnPointerUpAsObservable().Subscribe(delegate
		{
			_model.RxIsPressed.Value = false;
		}).AddTo(Disposable);
		EArtifactDealCardRarityType rarity = _model.Rarity;
		_view.FocusedBack_Tier1.SetActive(rarity == EArtifactDealCardRarityType.Normal);
		_view.FocusedBack_Tier2.SetActive(rarity == EArtifactDealCardRarityType.Rare);
		_view.FocusedBack_Tier3.SetActive(rarity == EArtifactDealCardRarityType.Elite);
		_view.FocusedBack_Tier4.SetActive(rarity == EArtifactDealCardRarityType.Epic);
		_view.FocusedBack_Tier5.SetActive(rarity == EArtifactDealCardRarityType.Legend);
		_view.FocusedBack.gameObject.SetActive(value: false);
		_view.DisableFront_Tier1.SetActive(rarity == EArtifactDealCardRarityType.Normal);
		_view.DisableFront_Tier2.SetActive(rarity == EArtifactDealCardRarityType.Rare);
		_view.DisableFront_Tier3.SetActive(rarity == EArtifactDealCardRarityType.Elite);
		_view.DisableFront_Tier4.SetActive(rarity == EArtifactDealCardRarityType.Epic);
		_view.DisableFront_Tier5.SetActive(rarity == EArtifactDealCardRarityType.Legend);
		_view.DisableFront.SetActive(value: false);
		_view.Focused_Tier1.SetActive(rarity == EArtifactDealCardRarityType.Normal);
		_view.Focused_Tier2.SetActive(rarity == EArtifactDealCardRarityType.Rare);
		_view.Focused_Tier3.SetActive(rarity == EArtifactDealCardRarityType.Elite);
		_view.Focused_Tier4.SetActive(rarity == EArtifactDealCardRarityType.Epic);
		_view.Focused_Tier5.SetActive(rarity == EArtifactDealCardRarityType.Legend);
		_view.Focused.SetActive(value: false);
	}

	protected override void OnSlotEnter()
	{
		base.OnSlotEnter();
		if (_model.RxIsLocked.Value)
		{
			_view.DisableFront.SetActive(value: true);
		}
	}

	protected override void OnSlotExit()
	{
		base.OnSlotExit();
		_view.DisableFront.SetActive(value: false);
		_view.FocusedBack.gameObject.SetActive(value: false);
	}

	public void SetInteractable(bool interactable)
	{
		_isInteractable = interactable;
		UpdateLocked();
	}

	private void UpdateLocked()
	{
		_model.RxIsLocked.Value = _artifactDealManager.GetCardPlayability(_model.Id) != ECardPlayability.Playable || !_isInteractable;
	}

	private void UpdateSuccessPercent()
	{
		int percent = _artifactDealManager.GetCardPlaySuccessPercent(_model.Id);
		string color = ((percent < 50) ? ((percent < 25) ? "#A91414" : "#994C00") : ((percent < 75) ? "#AD9300" : "#6E9913"));
		_textPercent.text = string.Format(TLocalizedText.SuccessChanceFormat.LocalText, percent).WithColor(color);
	}
}
