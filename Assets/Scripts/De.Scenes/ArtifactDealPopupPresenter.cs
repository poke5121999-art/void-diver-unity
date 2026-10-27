using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Enum;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameCommon.Data;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using SomeNet.Utility;
using NemoLib.Popup;
using TMPro;
using Spine.Unity;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ArtifactDealPopupPresenter : GamePopupPresenter
{
	private enum EItemPanelType
	{
		Inventory = 0,
		Stash = 1
	}

	[SerializeField]
	private ArtifactDealSequenceTimeConfig _sequenceTimeConfig;

	[View]
	private ArtifactDealPopupView _view;

	[InjectModel]
	private ArtifactDealPopupModel _model;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private ArtifactDealManager _artifactDealManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GameUiEventSystem _uiEventSystem;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameEventSystem _eventSystem;

	private CancellationTokenSource _skeletonCts;

	private Tweener _remainCountTweener;

	private Tweener _dealCoinTweener;

	private Tweener _dealExpTweener;

	private PlayerInventoryPanelModel _inventoryPanelModel;

	private StashPanelModel _stashPanelModel;

	private InventoryGoodsSlotModel _dealingSlotModel;

	private GoodsTooltipModel _dealingTooltipModel;

	private EItemPanelType _itemPanelType;

	private readonly string[] _guestSaySfxKeys = new string[3] { "ArtifactDeal_GuestSay1", "ArtifactDeal_GuestSay2", "ArtifactDeal_GuestSay3" };

	private int _lastPlayedCardIndex = -1;

	private CompositeDisposable _cardDisposables = new CompositeDisposable();

	private readonly Dictionary<int, ArtifactDealCardPlayPresenter> _cardPlaySlotsByCardId = new Dictionary<int, ArtifactDealCardPlayPresenter>();

	private int _textBubbleGeneration;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override string OpenSfx => "ArtifactUI";

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private Inventory MyInventory => _gameManager.MyInventory;

	private GameObject[] CategorySlots => _view.Requirement_Body_Categories;

	private GameObject[] PreferPrefixSlots => _view.Requirement_Body_PreferedPrefixes;

	private GameObject[] DislikePrefixSlots => _view.Requirement_Body_DislikePrefixes;

	private GameObject[] CardSlots => _view.CardPlayZone_Animation_Cards;

	private TArtifactDeal DealData => TArtifactDeal.Table[_model.ArtifactDealId];

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_skeletonCts = new CancellationTokenSource();
		SetNpcSkeletonAsync(_skeletonCts.Token).Forget();
		List<(EArtifactCategory, int[])> ownedArtifacts = _gameManager.MyInventory.Slots.Values.Concat(_playerManager.StashSlots.Values).Where((GoodsSlot s) => s.Type == EGoodsType.Artifact).Select((GoodsSlot s) => (s.Goods.GetArtifactCategory(), s.Goods.GetPrefixes()))
			.ToList();
		_artifactDealManager.Initialize(_model.ArtifactDealId, ownedArtifacts);
		_artifactDealManager.RxDealState.Subscribe(ApplyStateUi).AddTo(OpenDisposable);
		_artifactDealManager.RxDealState.Subscribe(OnDealStateChanged).AddTo(OpenDisposable);
		SetRequirementSlots();
		ClearDealPrice();
		_view.DealingPrice_Body_Exp_TextExp_TextValue.text = "0";
		SetRemainCount(0);
		_inventoryPanelModel = new PlayerInventoryPanelModel
		{
			PlayerInventory = _gameManager.MyInventory,
			AllowedSubTypes = new EGoodsType[1] { EGoodsType.Artifact },
			ArtifactFilterTypes = _artifactDealManager.Categories.ToArray(),
			IsArtifactDeal = true
		};
		_inventoryPanelModel.OnSlotClick.Subscribe(delegate(InventoryGoodsSlotModel slotModel)
		{
			OnSlotClick(slotModel);
		}).AddTo(OpenDisposable);
		_inventoryPanelModel.OnEquipmentSlotClick.Subscribe(OnEquipmentSlotClick).AddTo(OpenDisposable);
		_inventoryPanelModel.OnSafeSlotClick.Subscribe(delegate(InventoryGoodsSlotModel slotModel)
		{
			OnSlotClick(slotModel);
		}).AddTo(OpenDisposable);
		_inventoryPanelModel.RxFocusedSlotModel.Subscribe(UpdateHoverMatchedOutline).AddTo(OpenDisposable);
		_inventoryPanelModel.RxFocusedSafeSlotModel.Subscribe(UpdateHoverMatchedOutline).AddTo(OpenDisposable);
		_view.InventoryPanel.Inject(_inventoryPanelModel);
		_view.StashPanel.gameObject.SetActive(true);
		_stashPanelModel = new StashPanelModel
		{
			AllowedSubtypes = new EGoodsType[1] { EGoodsType.Artifact },
			ArtifactFilterTypes = _artifactDealManager.Categories.ToArray(),
			EnableFilterButtons = false,
			IsArtifactDeal = true
		};
		_stashPanelModel.OnSlotClick.Subscribe(delegate(InventoryGoodsSlotModel slotModel)
		{
			OnSlotClick(slotModel);
		}).AddTo(OpenDisposable);
		_stashPanelModel.RxFocusedSlotModel.Subscribe(UpdateHoverMatchedOutline).AddTo(OpenDisposable);
		_view.StashPanel.Inject(_stashPanelModel);
		int num = _gameManager.MyInventory.Slots.Values.Count(IsDealableArtifactSlot);
		int num2 = _playerManager.StashSlots.Values.Count(IsDealableArtifactSlot);
		if (num == 0 && num2 > 0)
		{
			_itemPanelType = EItemPanelType.Stash;
		}
		_view.DealButtonLayout_Button.OnClickAsObservable().Subscribe(delegate
		{
			OnClickAcceptDeal();
		}).AddTo(OpenDisposable);
		_view.Other_Button.OnClickAsObservable().Subscribe(delegate
		{
			OnClickSwapArtifact();
		}).AddTo(OpenDisposable);
		_view.CardPlayZone_Reroll_Button.OnClickAsObservable().Subscribe(delegate
		{
			OnClickReroll();
		}).AddTo(OpenDisposable);
		_view.GiveupButtonLayout_Button.OnClickAsObservable().Subscribe(delegate
		{
			OnClickGiveUpAsync().Forget();
		}).AddTo(OpenDisposable);
		_artifactDealManager.RequestStateSelectingArtifact();
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Back();
		}).AddTo(InputDisposable);
		UiActions.Switch.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			SwapItemPanel();
		}).AddTo(InputDisposable);
		ApplyStateUi(_artifactDealManager.RxDealState.Value);
	}

	protected override void OnInputDisabled()
	{
		DisableAllPanelInput();
		base.OnInputDisabled();
	}

	private void DisableAllPanelInput()
	{
		_inventoryPanelModel.RxIsInputEnabled.Value = false;
		_stashPanelModel.RxIsInputEnabled.Value = false;
	}

	private static bool IsNegotiatingState(EArtifactDealState s)
	{
		if (s < EArtifactDealState.AwaitAction || s > EArtifactDealState.PlayDealSetup)
		{
			return s == EArtifactDealState.PlaySwapSetup;
		}
		return true;
	}

	private static bool IsClosingState(EArtifactDealState s)
	{
		if (s < EArtifactDealState.StopBeforeDeal || s > EArtifactDealState.GiveUpDeal)
		{
			return s == EArtifactDealState.AcceptDeal;
		}
		return true;
	}

	private void ApplyStateUi(EArtifactDealState next)
	{
		bool flag = next == EArtifactDealState.AwaitSelectArtifact || next == EArtifactDealState.AwaitSwapArtifact;
		bool flag2 = IsNegotiatingState(next);
		bool isActive = flag2 || next == EArtifactDealState.AcceptDeal || next == EArtifactDealState.GiveUpDeal;
		bool flag3 = flag2 || IsClosingState(next);
		_view.InventoryPanel.gameObject.SetActive(flag && _itemPanelType == EItemPanelType.Inventory);
		_view.StashPanel.gameObject.SetActive(flag && _itemPanelType == EItemPanelType.Stash);
		SetActiveMiniGameUi(isActive);
		_view.PlaceHolder.SetActive(flag3);
		_view.RemainPlayCount.gameObject.SetActive(flag3);
		_view.RemainPlayCountLine.gameObject.SetActive(flag3);
		if (!flag3)
		{
			_view.TextBubble.SetActive(false);
		}
		SetCardsInteractable(next == EArtifactDealState.AwaitAction);
		if (base.IsInputEnable)
		{
			DisableAllPanelInput();
			ClearNav();
			if (next == EArtifactDealState.AwaitSelectArtifact || next == EArtifactDealState.AwaitSwapArtifact)
			{
				((_itemPanelType == EItemPanelType.Inventory) ? (BasePanelModel)_inventoryPanelModel : _stashPanelModel).RxIsInputEnabled.Value = true;
			}
			else if (flag2)
			{
				RebuildMiniGameNav();
			}
		}
	}

	private void OnDealStateChanged(EArtifactDealState next)
	{
		switch (next)
		{
		case EArtifactDealState.None:
		case EArtifactDealState.AwaitSelectArtifact:
			break;
		case EArtifactDealState.AwaitSwapArtifact:
			SetRequirementSlots();
			break;
		case EArtifactDealState.AwaitAction:
			SetRequirementSlots();
			SetDealPrice(_artifactDealManager.GetDealPrice(), _artifactDealManager.GetOriginalPrice());
			SetDealExp((int)_artifactDealManager.GetDealExp());
			SetRemainPlayCount();
			SetRemainRerollCount();
			SetCards();
			break;
		case EArtifactDealState.AwaitActionWithNoCount:
			PlayNoMorePlayCountAsync();
			break;
		case EArtifactDealState.PlayCard:
		case EArtifactDealState.PlayReroll:
		case EArtifactDealState.PlayDealSetup:
		case EArtifactDealState.PlaySwapSetup:
			ConsumePendingEventsAsync(next);
			break;
		case EArtifactDealState.StopBeforeDeal:
			PlayStopping();
			break;
		case EArtifactDealState.GiveUpDeal:
			PlayGivingUpAsync();
			break;
		case EArtifactDealState.AcceptDeal:
			PlayConcludingAsync();
			break;
		}
	}

	private void BindDealingArtifactView(Goods goods)
	{
		_view.DealingArtifact_GoodsSlot.Inject(new InventoryGoodsSlotModel(_dealingSlotModel.GoodsSlot, InventoryGoodsSlotPresenter.ELayout.Normal));
		_dealingTooltipModel = new GoodsTooltipModel
		{
			Goods = goods,
			HideEffectGroup = true
		};
		if (_view.DealingArtifactTooltip.gameObject.activeInHierarchy)
		{
			_view.DealingArtifactTooltip.Inject(_dealingTooltipModel);
		}
	}

	private void PlayRollingSfx()
	{
		if (_artifactDealManager.RxDealState.Value != EArtifactDealState.PlayDealSetup)
		{
			_soundManager.PlaySfx("ArtifactDeal_Rolling");
		}
	}

	private void SetRemainCount(int targetCount, float duration = 0f)
	{
		_remainCountTweener?.Kill();
		_remainCountTweener = null;
		if (duration <= 0f)
		{
			_view.RemainPlayCount_Count.text = targetCount.ToString();
			return;
		}
		PlayRollingSfx();
		int current = (int.TryParse(_view.RemainPlayCount_Count.text, out var result) ? result : 0);
		_remainCountTweener = DOTween.To(() => current, delegate(int x)
		{
			current = x;
			_view.RemainPlayCount_Count.text = x.ToString();
		}, targetCount, duration).SetEase(Ease.OutQuad).SetLink(base.gameObject);
	}

	private async UniTask<bool> OnDealStartedAsync(ArtifactDealStartedEvent e, CancellationToken ct)
	{
		SetCards();
		Animator handsAnimator = _view.CardPlayZone_Animation.GetComponent<Animator>();
		handsAnimator.Play("State2");
		handsAnimator.Update(0f);
		handsAnimator.speed = 0f;
		string message = PickBubbleText(DealData.BubbleTextsOnDealStart);
		ShowTextBubble(message, _sequenceTimeConfig.GetSeconds(EArtifactDealStepDelay.InitialSetupTextBubbleDuration));
		if (await WaitSequenceStepAsync(_sequenceTimeConfig.GetSeconds(EArtifactDealStepDelay.InitialSetupAfterTextBubble), ct))
		{
			return true;
		}
		SetRemainCount(e.InitialPlayCount, _sequenceTimeConfig.GetSeconds(EArtifactDealStepDelay.InitialSetupRemainCountTween));
		if (await WaitSequenceStepAsync(_sequenceTimeConfig.GetSeconds(EArtifactDealStepDelay.InitialSetupAfterRemainCount), ct))
		{
			return true;
		}
		SetRequirementSlots();
		await AnimateDealPriceAsync(new PriceChange
		{
			Before = e.DealPrice,
			After = e.DealPrice
		}, ct, showChangeFloating: false);
		handsAnimator.speed = 1f;
		if (await WaitSequenceStepAsync(_sequenceTimeConfig.GetSeconds(EArtifactDealStepDelay.InitialSetupAfterHandDeal), ct))
		{
			return true;
		}
		_view.DealStart_Normal_TextDesc.text = TLocalizedText.ArtifactDealStartDealFormat.Format(TNpc.Table[DealData.NpcId].Name);
		_view.DealStart.SetActive(false);
		_view.DealStart.SetActive(true);
		_soundManager.PlaySfx("ArtifactDeal_Start");
		if (await WaitSequenceStepAsync(_sequenceTimeConfig.GetSeconds(EArtifactDealStepDelay.InitialSetupAfterDealStartTitle), ct))
		{
			return true;
		}
		return ct.IsCancellationRequested;
	}

	private async UniTask<bool> OnGoodsSwappedAsync(ArtifactDealGoodsSwappedEvent e, CancellationToken ct)
	{
		SetRequirementSlots();
		if (e.PriceBefore != e.PriceAfter)
		{
			await AnimateDealPriceAsync(new PriceChange
			{
				Before = e.PriceBefore,
				After = e.PriceAfter
			}, ct);
		}
		return ct.IsCancellationRequested;
	}

	private async UniTaskVoid ConsumePendingEventsAsync(EArtifactDealState finishState)
	{
		CancellationToken ct = this.GetCancellationTokenOnDestroy();
		IArtifactDealEvent[] array = _artifactDealManager.PendingEvents.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			if (await PlayEventAsync(array[i], ct))
			{
				return;
			}
		}
		if (finishState == EArtifactDealState.PlayDealSetup || finishState == EArtifactDealState.PlaySwapSetup || (finishState == EArtifactDealState.PlayCard && _artifactDealManager.RemainPlayCount >= 1))
		{
			SetCardsInteractable(interactable: true);
			if (await WaitSequenceStepAsync(EArtifactDealStepDelay.CardUsableLightUpLead, ct))
			{
				return;
			}
		}
		_artifactDealManager.NotifyStateFinished(finishState);
	}

	private async UniTask<bool> PlayEventAsync(IArtifactDealEvent dealEvent, CancellationToken ct)
	{
		if (!(dealEvent is ArtifactDealMatchingBonusChangedEvent artifactDealMatchingBonusChangedEvent))
		{
			if (!(dealEvent is ArtifactDealStartedEvent e))
			{
				if (!(dealEvent is ArtifactDealHandRerolledEvent e2))
				{
					if (!(dealEvent is ArtifactDealGoodsSwappedEvent e3))
					{
						if (!(dealEvent is ArtifactDealNpcHesitatingEvent e4))
						{
							if (!(dealEvent is ArtifactDealCardResolvedEvent e5))
							{
								if (!(dealEvent is ArtifactDealPreferTagsAddedEvent artifactDealPreferTagsAddedEvent))
								{
									if (!(dealEvent is ArtifactDealPreferTagsRerolledEvent artifactDealPreferTagsRerolledEvent))
									{
										if (!(dealEvent is ArtifactDealDislikeTagsRerolledEvent artifactDealDislikeTagsRerolledEvent))
										{
											if (!(dealEvent is ArtifactDealDislikeTagsRemovedEvent artifactDealDislikeTagsRemovedEvent))
											{
												if (!(dealEvent is ArtifactDealSpecificPreferTagAddedEvent artifactDealSpecificPreferTagAddedEvent))
												{
													if (!(dealEvent is ArtifactDealPriceChangedEvent artifactDealPriceChangedEvent))
													{
														if (!(dealEvent is ArtifactDealCardConsumedEvent e6))
														{
															if (dealEvent is ArtifactDealPriceFloorReachedEvent)
															{
																PlayPriceFloorFeedback();
															}
														}
														else if (await OnCardConsumedAsync(e6, ct))
														{
															return true;
														}
													}
													else
													{
														await AnimateDealPriceAsync(new PriceChange
														{
															Before = artifactDealPriceChangedEvent.Before,
															After = artifactDealPriceChangedEvent.After
														}, ct);
													}
												}
												else if (await AnimateTagDiffAsync(new int[1] { artifactDealSpecificPreferTagAddedEvent.AddedId }, null, null, null, ct))
												{
													return true;
												}
											}
											else if (await AnimateTagDiffAsync(null, null, null, artifactDealDislikeTagsRemovedEvent.RemovedIds, ct))
											{
												return true;
											}
										}
										else if (await AnimateSlotTransitionsAsync(_view.Requirement_Body_DislikePrefixes, artifactDealDislikeTagsRerolledEvent.BeforeSlots, artifactDealDislikeTagsRerolledEvent.AfterSlots, isPrefer: false, ct))
										{
											return true;
										}
									}
									else if (await AnimateSlotTransitionsAsync(_view.Requirement_Body_PreferedPrefixes, artifactDealPreferTagsRerolledEvent.BeforeSlots, artifactDealPreferTagsRerolledEvent.AfterSlots, isPrefer: true, ct))
									{
										return true;
									}
								}
								else if (await AnimateTagDiffAsync(artifactDealPreferTagsAddedEvent.AddedIds, null, null, null, ct))
								{
									return true;
								}
							}
							else if (await OnCardResolvedAsync(e5, ct))
							{
								return true;
							}
						}
						else if (await OnNpcHesitatingAsync(e4, ct))
						{
							return true;
						}
					}
					else if (await OnGoodsSwappedAsync(e3, ct))
					{
						return true;
					}
				}
				else if (await OnHandRerolledAsync(e2, ct))
				{
					return true;
				}
			}
			else if (await OnDealStartedAsync(e, ct))
			{
				return true;
			}
		}
		else if (artifactDealMatchingBonusChangedEvent.AfterTier > artifactDealMatchingBonusChangedEvent.BeforeTier)
		{
			_soundManager.PlaySfx("ArtifactDeal_Tag_Match_Good");
			_soundManager.PlaySfx("ArtifactDeal_Tag_Match_Set");
		}
		return ct.IsCancellationRequested;
	}

	private string PickBubbleText(string[] texts)
	{
		if (texts == null || texts.Length < 1)
		{
			return string.Empty;
		}
		return texts[RandomUtility.GetRandomInt(0, texts.Length)];
	}

	private string[] CardResultBubbleTexts(ArtifactDealCardResolvedEvent e)
	{
		if (!e.IsSuccess)
		{
			return DealData.BubbleTextsOnCardFail;
		}
		if (e.Effects == null || e.Effects.Length < 1)
		{
			return DealData.BubbleTextsOnCardSuccessPreferTag;
		}
		switch (e.Effects[0].EffectType)
		{
		case EArtifactDealCardEffectType.IncreasePricePercent:
		case EArtifactDealCardEffectType.DecreasePricePercent:
			return DealData.BubbleTextsOnCardSuccessPrice;
		case EArtifactDealCardEffectType.RerollDislikeTags:
		case EArtifactDealCardEffectType.RemoveDislikeTags:
			return DealData.BubbleTextsOnCardSuccessDislikeTag;
		default:
			return DealData.BubbleTextsOnCardSuccessPreferTag;
		}
	}

	private async UniTask<bool> OnNpcHesitatingAsync(ArtifactDealNpcHesitatingEvent e, CancellationToken ct)
	{
		EArtifactDealStepDelay step = EArtifactDealStepDelay.HesitateLowDuration;
		string[] texts;
		if (e.Tier == EArtifactDealSuccessRateTier.Low)
		{
			texts = DealData.BubbleTextsOnHesitationLowChance;
		}
		else
		{
			step = EArtifactDealStepDelay.HesitateMidDuration;
			texts = DealData.BubbleTextsOnHesitationMidChance;
		}
		ShowTextBubble(PickBubbleText(texts), -1f);
		return await WaitSequenceStepAsync(step, ct);
	}

	private async UniTask<bool> OnCardResolvedAsync(ArtifactDealCardResolvedEvent e, CancellationToken ct)
	{
		ShowTextBubble(PickBubbleText(CardResultBubbleTexts(e)), -1f);
		if (await WaitSequenceStepAsync(EArtifactDealStepDelay.CardResultBubbleWait, ct))
		{
			return true;
		}
		GameObject suggest = (e.IsSuccess ? _view.DealSuggestSuccess : _view.DealSuggestFail);
		TextMeshProUGUI textMeshProUGUI = (e.IsSuccess ? _view.DealSuggestSuccess_Normal_TextDesc : _view.DealSuggestFail_Normal_TextDesc);
		textMeshProUGUI.text = ((e.Effects == null || e.Effects.Length < 1) ? string.Empty : string.Join("\n", e.Effects.Select((ArtifactDealCardEffect effect) => effect.EffectType.ToDescription(effect.Value))));
		_soundManager.PlaySfx(e.IsSuccess ? "ArtifactDeal_Card_Success" : "ArtifactDeal_Card_Fail");
		suggest.SetActive(true);
		HideDealSuggestAfterAsync(suggest);
		return await WaitSequenceStepAsync(EArtifactDealStepDelay.CardResultSuggestToNumberDelay, ct);
	}

	private async UniTaskVoid HideDealSuggestAfterAsync(GameObject suggest)
	{
		CancellationToken cancellationTokenOnDestroy = this.GetCancellationTokenOnDestroy();
		if (!(await WaitSequenceStepAsync(_sequenceTimeConfig.GetSeconds(EArtifactDealStepDelay.CardResultSuggestDuration), cancellationTokenOnDestroy)) && suggest != null)
		{
			suggest.SetActive(false);
		}
	}

	private async UniTask<bool> OnCardConsumedAsync(ArtifactDealCardConsumedEvent e, CancellationToken ct)
	{
		SetCards();
		SetRemainPlayCount();
		return await WaitSequenceStepAsync(EArtifactDealStepDelay.CardPlayingAfterHandSync, ct);
	}

	private async UniTaskVoid PlayCardCompleteAfterDelayAsync(ArtifactDealCardPlayPresenter playSlot)
	{
		float seconds = _sequenceTimeConfig.GetSeconds(EArtifactDealStepDelay.CardUpToCompleteOffset);
		if (!(await WaitSequenceStepAsync(seconds, this.GetCancellationTokenOnDestroy())))
		{
			playSlot.PlayComplete();
		}
	}

	private void PlayPriceFloorFeedback()
	{
		_view.DealingPrice.transform.DOPunchScale(Vector3.one * 0.12f, 0.3f, 10, 0.5f).SetLink(_view.DealingPrice);
		TextMeshProUGUI text = _view.DealingPrice_Body_Coin_TextBalanceValue_TextValue;
		Color original = text.color;
		DOTween.To(() => text.color, delegate(Color x)
		{
			text.color = x;
		}, new Color(0.5803922f, 0.3019608f, 0.30588236f, 1f), 0.12f).SetLoops(2, LoopType.Yoyo).SetLink(text.gameObject)
			.OnComplete(delegate
			{
				text.color = original;
			});
	}

	private async UniTask<bool> AnimateTagDiffAsync(int[] addedPrefer, int[] removedPrefer, int[] addedDislike, int[] removedDislike, CancellationToken ct)
	{
		TagDiff diff = new TagDiff
		{
			AddedPrefer = (addedPrefer ?? Array.Empty<int>()),
			RemovedPrefer = (removedPrefer ?? Array.Empty<int>()),
			AddedDislike = (addedDislike ?? Array.Empty<int>()),
			RemovedDislike = (removedDislike ?? Array.Empty<int>())
		};
		if (diff.HasAny)
		{
			await AnimateRequirementSlotsAsync(diff, ct);
		}
		return ct.IsCancellationRequested;
	}

	private async UniTask<bool> OnHandRerolledAsync(ArtifactDealHandRerolledEvent e, CancellationToken ct)
	{
		ShowTextBubble(PickBubbleText(DealData.BubbleTextsOnReroll), -1f);
		Animator handsAnimator = _view.CardPlayZone_Animation.GetComponent<Animator>();
		handsAnimator.Play("State1");
		handsAnimator.Update(0f);
		if (await WaitSequenceStepAsync(EArtifactDealStepDelay.RerollHandSetDelay, ct))
		{
			return true;
		}
		SetCards();
		if (await WaitSequenceStepAsync(EArtifactDealStepDelay.RerollUsableDelay, ct))
		{
			return true;
		}
		handsAnimator.Play("CardsIdle");
		SetCardsInteractable(interactable: true);
		SetRemainRerollCount();
		PlayRerollCountFeedback();
		if (await WaitSequenceStepAsync(EArtifactDealStepDelay.RerollAfterUpdate, ct))
		{
			return true;
		}
		return ct.IsCancellationRequested;
	}

	private void PlayRerollCountFeedback()
	{
		_view.CardPlayZone_Reroll_Button_TextSub.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 8, 0.5f).SetLink(_view.CardPlayZone_Reroll_Button_TextSub.gameObject);
	}

	private async UniTaskVoid PlayNoMorePlayCountAsync()
	{
		CancellationToken ct = this.GetCancellationTokenOnDestroy();
		Animation component = _view.RemainPlayCount.GetComponent<Animation>();
		if (component != null)
		{
			component.Play();
			if (!(await WaitSequenceStepAsync(EArtifactDealStepDelay.NoMorePlayCountAlertWait, ct)))
			{
				string message = PickBubbleText(DealData.BubbleTextsOnPlayCountExhausted);
				ShowTextBubble(message, -1f);
				await WaitSequenceStepAsync(EArtifactDealStepDelay.NoMorePlayCountWait, ct);
			}
		}
	}

	private async UniTask AnimateDealPriceAsync(PriceChange change, CancellationToken ct, bool showChangeFloating = true)
	{
		int originalPrice = _artifactDealManager.GetOriginalPrice();
		if (showChangeFloating)
		{
			int intValue = TConst.CoinToGoldRate.IntValue;
			int num = change.After / intValue - change.Before / intValue;
			Animator component = _view.DealingPrice_Body_Change.GetComponent<Animator>();
			SetPriceChangeText(num);
			component.gameObject.SetActive(num != 0);
			if (num > 0)
			{
				component.Play("ArtifactDealPriceUp", 0, 0f);
				component.Update(0f);
				_soundManager.PlaySfx("ArtifactDeal_Plus");
			}
			else if (num < 0)
			{
				component.Play("ArtifactDealPriceDown", 0, 0f);
				component.Update(0f);
				_soundManager.PlaySfx("ArtifactDeal_Minus");
			}
			if (num != 0)
			{
				if (await WaitSequenceStepAsync(EArtifactDealStepDelay.PriceChangeFloatingDuration, ct))
				{
					return;
				}
				_view.DealingPrice_Body_Change_TextChangeUp.gameObject.SetActive(false);
				_view.DealingPrice_Body_Change_TextChangeDown.gameObject.SetActive(false);
				_view.DealingPrice_Body_Change.SetActive(false);
			}
		}
		SetDealPrice(change.After, originalPrice, _sequenceTimeConfig.GetSeconds(EArtifactDealStepDelay.DealPriceTweenDuration));
		if (!(await WaitSequenceStepAsync(EArtifactDealStepDelay.DealPriceTweenDuration, ct)))
		{
			SetDealExp((int)_artifactDealManager.GetDealExp(), _sequenceTimeConfig.GetSeconds(EArtifactDealStepDelay.DealExpTweenDuration));
			await WaitSequenceStepAsync(EArtifactDealStepDelay.PriceUpdateWait, ct);
		}
	}

	private void SetPriceChangeText(int deltaCoin)
	{
		if (deltaCoin > 0)
		{
			_view.DealingPrice_Body_Change_TextChangeUp.text = string.Format("+{0}", deltaCoin);
		}
		else if (deltaCoin < 0)
		{
			_view.DealingPrice_Body_Change_TextChangeDown.text = deltaCoin.ToString();
		}
		_view.DealingPrice_Body_Change_TextChangeUp.gameObject.SetActive(deltaCoin > 0);
		_view.DealingPrice_Body_Change_TextChangeDown.gameObject.SetActive(deltaCoin < 0);
	}

	private int ToBalancePercent(int price, int originalPrice)
	{
		if (originalPrice == 0)
		{
			return 0;
		}
		return Mathf.RoundToInt((float)(price - originalPrice) * 100f / (float)originalPrice);
	}

	private async UniTask AnimateRequirementSlotsAsync(TagDiff diff, CancellationToken ct)
	{
		int num = ((diff.RemovedPrefer != null) ? diff.RemovedPrefer.Count : 0) + ((diff.RemovedDislike != null) ? diff.RemovedDislike.Count : 0);
		if (num >= 1)
		{
			_soundManager.PlaySfx("ArtifactDeal_Tag_Delete");
			await AnimateRemovedSlotsAsync(diff.RemovedPrefer, diff.RemovedDislike, _sequenceTimeConfig.GetSeconds(EArtifactDealStepDelay.RemoveSlotCancelHold), ct);
			if (await WaitSequenceStepAsync(_sequenceTimeConfig.GetSeconds(EArtifactDealStepDelay.TagUpdateAfterRemove), ct))
			{
				return;
			}
		}
		bool[] activeBefore = SnapshotActiveStates(_view.Requirement_Body_PreferedPrefixes);
		bool[] activeBefore2 = SnapshotActiveStates(_view.Requirement_Body_DislikePrefixes);
		SetRequirementSlots();
		List<ArtifactDealSlotPresenter> list = CollectNewlyActiveSlots(_view.Requirement_Body_PreferedPrefixes, activeBefore);
		List<ArtifactDealSlotPresenter> list2 = CollectNewlyActiveSlots(_view.Requirement_Body_DislikePrefixes, activeBefore2);
		if (list.Any((ArtifactDealSlotPresenter s) => s.IsMatched))
		{
			_soundManager.PlaySfx("ArtifactDeal_Tag_Match_Good");
		}
		if (list2.Any((ArtifactDealSlotPresenter s) => s.IsMatched))
		{
			_soundManager.PlaySfx("ArtifactDeal_Tag_Match_Bad");
		}
		if (list.Any((ArtifactDealSlotPresenter s) => !s.IsMatched) || list2.Any((ArtifactDealSlotPresenter s) => !s.IsMatched))
		{
			_soundManager.PlaySfx("ArtifactDeal_Tag_Create");
		}
		list.AddRange(list2);
		if (list.Count >= 1)
		{
			float slotAddDuration = _sequenceTimeConfig.GetSeconds(EArtifactDealStepDelay.SlotAddAnimationDuration);
			await UniTask.WhenAll(list.Select((ArtifactDealSlotPresenter slot) => slot.PlayAddAnimationAsync(slotAddDuration, ct)).ToList());
			await WaitSequenceStepAsync(_sequenceTimeConfig.GetSeconds(EArtifactDealStepDelay.TagUpdateAfterAdd), ct);
		}
	}

	private async UniTask<bool> AnimateSlotTransitionsAsync(GameObject[] slots, int[] before, int[] after, bool isPrefer, CancellationToken ct)
	{
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		for (int i = 0; i < slots.Length; i++)
		{
			int num = ((i < before.Length) ? before[i] : 0);
			int num2 = ((i < after.Length) ? after[i] : 0);
			if (num != num2)
			{
				if (num != 0)
				{
					list.Add(i);
				}
				if (num2 != 0)
				{
					list2.Add(i);
				}
			}
		}
		if (list.Count == 0 && list2.Count == 0)
		{
			return ct.IsCancellationRequested;
		}
		if (list.Count > 0)
		{
			_soundManager.PlaySfx("ArtifactDeal_Tag_Delete");
			float cancelHold = _sequenceTimeConfig.GetSeconds(EArtifactDealStepDelay.RemoveSlotCancelHold);
			await UniTask.WhenAll(list.Select((int i) => slots[i].GetComponent<ArtifactDealSlotPresenter>().PlayRemoveAnimationAsync(cancelHold, ct)).ToList());
			if (await WaitSequenceStepAsync(_sequenceTimeConfig.GetSeconds(EArtifactDealStepDelay.TagUpdateAfterRemove), ct))
			{
				return true;
			}
		}
		SetRequirementSlots();
		if (list2.Count >= 1)
		{
			List<ArtifactDealSlotPresenter> list3 = list2.Select((int i) => slots[i].GetComponent<ArtifactDealSlotPresenter>()).Where((ArtifactDealSlotPresenter s) => s != null).ToList();
			if (isPrefer && list3.Any((ArtifactDealSlotPresenter s) => s.IsMatched))
			{
				_soundManager.PlaySfx("ArtifactDeal_Tag_Match_Good");
			}
			if (!isPrefer && list3.Any((ArtifactDealSlotPresenter s) => s.IsMatched))
			{
				_soundManager.PlaySfx("ArtifactDeal_Tag_Match_Bad");
			}
			if (list3.Any((ArtifactDealSlotPresenter s) => !s.IsMatched))
			{
				_soundManager.PlaySfx("ArtifactDeal_Tag_Create");
			}
			float slotAddDuration = _sequenceTimeConfig.GetSeconds(EArtifactDealStepDelay.SlotAddAnimationDuration);
			await UniTask.WhenAll(list3.Select((ArtifactDealSlotPresenter slot) => slot.PlayAddAnimationAsync(slotAddDuration, ct)).ToList());
			if (await WaitSequenceStepAsync(_sequenceTimeConfig.GetSeconds(EArtifactDealStepDelay.TagUpdateAfterAdd), ct))
			{
				return true;
			}
		}
		return ct.IsCancellationRequested;
	}

	private async UniTask AnimateRemovedSlotsAsync(IReadOnlyList<int> removedPrefer, IReadOnlyList<int> removedDislike, float duration, CancellationToken ct)
	{
		List<UniTask> list = new List<UniTask>();
		foreach (int item in removedPrefer)
		{
			ArtifactDealSlotPresenter artifactDealSlotPresenter = FindActiveSlotByPrefixId(_view.Requirement_Body_PreferedPrefixes, item);
			if (artifactDealSlotPresenter != null)
			{
				list.Add(artifactDealSlotPresenter.PlayRemoveAnimationAsync(duration, ct));
			}
		}
		foreach (int item2 in removedDislike)
		{
			ArtifactDealSlotPresenter artifactDealSlotPresenter2 = FindActiveSlotByPrefixId(_view.Requirement_Body_DislikePrefixes, item2);
			if (artifactDealSlotPresenter2 != null)
			{
				list.Add(artifactDealSlotPresenter2.PlayRemoveAnimationAsync(duration, ct));
			}
		}
		if (list.Count >= 1)
		{
			await UniTask.WhenAll(list);
		}
	}

	private bool[] SnapshotActiveStates(GameObject[] slots)
	{
		bool[] array = new bool[slots.Length];
		for (int i = 0; i < slots.Length; i++)
		{
			array[i] = slots[i].activeSelf;
		}
		return array;
	}

	private List<ArtifactDealSlotPresenter> CollectNewlyActiveSlots(GameObject[] slots, bool[] activeBefore)
	{
		List<ArtifactDealSlotPresenter> list = new List<ArtifactDealSlotPresenter>();
		for (int i = 0; i < slots.Length; i++)
		{
			if (!activeBefore[i] && slots[i].activeSelf)
			{
				ArtifactDealSlotPresenter component = slots[i].GetComponent<ArtifactDealSlotPresenter>();
				if (component != null)
				{
					list.Add(component);
				}
			}
		}
		return list;
	}

	private ArtifactDealSlotPresenter FindActiveSlotByPrefixId(GameObject[] slots, int prefixId)
	{
		foreach (GameObject gameObject in slots)
		{
			if (gameObject.activeSelf)
			{
				ArtifactDealSlotPresenter component = gameObject.GetComponent<ArtifactDealSlotPresenter>();
				if (component != null && component.PrefixId == prefixId)
				{
					return component;
				}
			}
		}
		return null;
	}

	private UniTask<bool> WaitSequenceStepAsync(EArtifactDealStepDelay step, CancellationToken ct)
	{
		return WaitSequenceStepAsync(_sequenceTimeConfig.GetSeconds(step), ct);
	}

	private async UniTask<bool> WaitSequenceStepAsync(float seconds, CancellationToken ct)
	{
		// Trap 59 (docs/DECODING.md): float32 product before the integer conversion, as IL2CPP; Mono needs the (float).
		return await UniTask.Delay((int)(float)(seconds * 1000f), ignoreTimeScale: false, PlayerLoopTiming.Update, ct).SuppressCancellationThrow();
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

	private async UniTaskVoid FinishSequenceAfterAsync(EArtifactDealState sequence, float seconds)
	{
		if (!(await UniTask.Delay((int)(float)(seconds * 1000f), ignoreTimeScale: false, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy()).SuppressCancellationThrow()))
		{
			_artifactDealManager.NotifyStateFinished(sequence);
		}
	}

	private void ShowTextBubble(string message, float time)
	{
		int generation = ++_textBubbleGeneration;
		_view.TextBubble.SetActive(true);
		_view.TextBubble_Text.text = message;
		if (!string.IsNullOrEmpty(message))
		{
			PlayGuestSaySfx();
		}
		if (time >= 0f)
		{
			HideTextBubbleAfterAsync(time, generation);
		}
	}

	private async UniTaskVoid HideTextBubbleAfterAsync(float seconds, int generation)
	{
		if (!(await UniTask.Delay((int)(float)(seconds * 1000f), ignoreTimeScale: false, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy()).SuppressCancellationThrow()) && generation == _textBubbleGeneration)
		{
			_view.TextBubble.SetActive(false);
		}
	}

	private void PlayGuestSaySfx()
	{
		_soundManager.PlaySfx(_guestSaySfxKeys[RandomUtility.GetRandomInt(0, _guestSaySfxKeys.Length)]);
	}

	private void RebuildMiniGameNav(Selectable preferred = null)
	{
		ClearNav();
		Button dealButtonLayout_Button = _view.DealButtonLayout_Button;
		Button other_Button = _view.Other_Button;
		Button cardPlayZone_Reroll_Button = _view.CardPlayZone_Reroll_Button;
		Button button;
		Button button2;
		if (!DealData.AllowGiveUp)
		{
			button = null;
			button2 = dealButtonLayout_Button;
		}
		else
		{
			button = _view.GiveupButtonLayout_Button;
			button2 = ((button != null) ? button : dealButtonLayout_Button);
		}
		List<Selectable> list = _view.CardPlayZone_Animation_Cards.Where((GameObject go) => go.activeSelf).Select(GetCardSelectable).ToList();
		ConnectNav(other_Button, ENavDir.Down, cardPlayZone_Reroll_Button);
		ConnectNav(cardPlayZone_Reroll_Button, ENavDir.Up, other_Button);
		if (button != null)
		{
			ConnectNav(button, ENavDir.Down, dealButtonLayout_Button);
			ConnectNav(dealButtonLayout_Button, ENavDir.Up, button);
		}
		Selectable selectable = ((list.Count > 0) ? list[0] : null);
		Selectable selectable2 = ((list.Count > 0) ? list[list.Count - 1] : null);
		if (list.Count > 0)
		{
			SetNavHorizontal(list);
			ConnectNav(selectable, ENavDir.Left, cardPlayZone_Reroll_Button);
			ConnectNav(selectable2, ENavDir.Right, button2);
		}
		ConnectNav(cardPlayZone_Reroll_Button, ENavDir.Right, (selectable != null) ? selectable : button2);
		if (button != null)
		{
			ConnectNav(button, ENavDir.Left, (selectable2 != null) ? selectable2 : cardPlayZone_Reroll_Button);
		}
		ConnectNav(dealButtonLayout_Button, ENavDir.Left, (selectable2 != null) ? selectable2 : cardPlayZone_Reroll_Button);
		if (preferred == null)
		{
			preferred = ((selectable != null) ? selectable : cardPlayZone_Reroll_Button);
		}
		SetDefaultNav(preferred);
	}

	private void SetActiveMiniGameUi(bool isActive)
	{
		_view.DealingArtifact.SetActive(isActive);
		_view.DealingArtifactTooltip.gameObject.SetActive(isActive);
		_view.DealingPrice.SetActive(isActive);
		_view.CardPlayZone.SetActive(isActive);
		_view.DealButtonLayout.SetActive(isActive);
		_view.GiveupButtonLayout.SetActive(isActive && DealData.AllowGiveUp);
		_view.Other.SetActive(isActive);
		if (isActive && _dealingTooltipModel != null)
		{
			_view.DealingArtifactTooltip.Inject(_dealingTooltipModel);
		}
	}

	private void SetActiveNpcStageUi(bool isActive)
	{
		_view.PlaceHolder.SetActive(isActive);
		_view.RemainPlayCount.gameObject.SetActive(isActive);
		_view.RemainPlayCountLine.gameObject.SetActive(isActive);
		if (!isActive)
		{
			_view.TextBubble.SetActive(false);
		}
	}

	private void SetRequirementSlots()
	{
		List<EArtifactCategory> list = _artifactDealManager.Categories.OrderBy((EArtifactCategory c) => c).ToList();
		for (int i = 0; i < CategorySlots.Length; i++)
		{
			bool flag = i < list.Count;
			CategorySlots[i].SetActive(flag);
			if (flag)
			{
				ArtifactDealSlotModel model = new ArtifactDealSlotModel
				{
					ArtifactCategory = list[i]
				};
				CategorySlots[i].GetComponent<ArtifactDealSlotPresenter>().Inject(model);
			}
		}
		for (int j = 0; j < PreferPrefixSlots.Length; j++)
		{
			bool flag2 = j < _artifactDealManager.PreferPrefixIds.Count;
			PreferPrefixSlots[j].SetActive(flag2);
			if (flag2)
			{
				ArtifactDealSlotModel model2 = new ArtifactDealSlotModel
				{
					ArtifactPrefixId = _artifactDealManager.PreferPrefixIds[j],
					IsPreferPrefix = true
				};
				PreferPrefixSlots[j].GetComponent<ArtifactDealSlotPresenter>().Inject(model2);
			}
		}
		for (int k = 0; k < DislikePrefixSlots.Length; k++)
		{
			bool flag3 = k < _artifactDealManager.DislikePrefixIds.Count;
			DislikePrefixSlots[k].SetActive(flag3);
			if (flag3)
			{
				ArtifactDealSlotModel model3 = new ArtifactDealSlotModel
				{
					ArtifactPrefixId = _artifactDealManager.DislikePrefixIds[k],
					IsPreferPrefix = false
				};
				DislikePrefixSlots[k].GetComponent<ArtifactDealSlotPresenter>().Inject(model3);
			}
		}
		ApplyTagMatchedFx();
	}

	private void ApplyTagMatchedFx()
	{
		EArtifactDealState value = _artifactDealManager.RxDealState.Value;
		if (value == EArtifactDealState.AwaitSelectArtifact || value == EArtifactDealState.AwaitSwapArtifact)
		{
			_view.Requirement_TagMatchedFX.SetActive(false);
			return;
		}
		float tagPriceMultiplier = _artifactDealManager.GetTagPriceMultiplier();
		float[] array = new float[5]
		{
			TConst.ArtifactDealTagMatchedBonusValueVfxTier1.FloatValue,
			TConst.ArtifactDealTagMatchedBonusValueVfxTier2.FloatValue,
			TConst.ArtifactDealTagMatchedBonusValueVfxTier3.FloatValue,
			TConst.ArtifactDealTagMatchedBonusValueVfxTier4.FloatValue,
			TConst.ArtifactDealTagMatchedBonusValueVfxTier5.FloatValue
		};
		Image[] array2 = new Image[5] { _view.Requirement_TagMatchedFX_Tier1, _view.Requirement_TagMatchedFX_Tier2, _view.Requirement_TagMatchedFX_Tier3, _view.Requirement_TagMatchedFX_Tier4, _view.Requirement_TagMatchedFX_Tier5 };
		int num = -1;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] < tagPriceMultiplier)
			{
				num = i;
			}
		}
		_view.Requirement_TagMatchedFX.SetActive(num >= 0);
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j].gameObject.SetActive(j == num);
		}
	}

	private void UpdateHoverMatchedOutline(InventoryGoodsSlotModel slotModel)
	{
		EArtifactDealState value = _artifactDealManager.RxDealState.Value;
		int[] hoveredPrefixes;
		if ((value == EArtifactDealState.AwaitSelectArtifact || value == EArtifactDealState.AwaitSwapArtifact) && slotModel != null)
		{
			hoveredPrefixes = ((slotModel.IsSlotEmpty || slotModel.GoodsType != EGoodsType.Artifact) ? null : slotModel.GoodsSlot.Goods.GetPrefixes());
		}
		else
		{
			hoveredPrefixes = null;
		}
		ApplyHoverMatchedOutline(_view.Requirement_Body_PreferedPrefixes, hoveredPrefixes);
		ApplyHoverMatchedOutline(_view.Requirement_Body_DislikePrefixes, hoveredPrefixes);
	}

	private void ApplyHoverMatchedOutline(GameObject[] slots, int[] hoveredPrefixes)
	{
		foreach (GameObject gameObject in slots)
		{
			if (gameObject.activeSelf)
			{
				gameObject.GetComponent<ArtifactDealSlotPresenter>().SetHoverMatchedOutline(hoveredPrefixes);
			}
		}
	}

	private void SetCards()
	{
		_cardDisposables.Dispose();
		_cardDisposables = new CompositeDisposable();
		_cardPlaySlotsByCardId.Clear();
		IReadOnlyList<int> hand = _artifactDealManager.Hand;
		for (int i = 0; i < _view.CardPlayZone_Animation_Cards.Length; i++)
		{
			bool flag = i < hand.Count;
			_view.CardPlayZone_Animation_Cards[i].SetActive(flag);
			if (flag)
			{
				int num = hand[i];
				ArtifactDealCardPlayPresenter componentInChildren = _view.CardPlayZone_Animation_Cards[i].GetComponentInChildren<ArtifactDealCardPlayPresenter>(true);
				if (num < 0)
				{
					componentInChildren.PlayComplete();
					continue;
				}
				ArtifactDealCardPlayModel artifactDealCardPlayModel = new ArtifactDealCardPlayModel();
				artifactDealCardPlayModel.Id = num;
				artifactDealCardPlayModel.OnClick.Subscribe(OnCardSlotClick).AddTo(_cardDisposables);
				componentInChildren.Inject(artifactDealCardPlayModel);
				_cardPlaySlotsByCardId[num] = componentInChildren;
			}
		}
		SetCardsInteractable(_artifactDealManager.RxIsCardInteractable.Value);
		if (IsNegotiatingState(_artifactDealManager.RxDealState.Value))
		{
			RebuildMiniGameNav(ResolvePreferredCardFocus(hand.Count));
		}
	}

	private void SetCardsInteractable(bool interactable)
	{
		foreach (ArtifactDealCardPlayPresenter value in _cardPlaySlotsByCardId.Values)
		{
			value.SetInteractable(interactable);
		}
	}

	private Selectable ResolvePreferredCardFocus(int newHandCount)
	{
		int lastPlayedCardIndex = _lastPlayedCardIndex;
		_lastPlayedCardIndex = -1;
		if (newHandCount == 0)
		{
			return _view.CardPlayZone_Reroll_Button;
		}
		if (lastPlayedCardIndex < 0)
		{
			return null;
		}
		GameObject cardSlot = ((lastPlayedCardIndex < newHandCount) ? CardSlots[lastPlayedCardIndex] : CardSlots[newHandCount - 1]);
		return GetCardSelectable(cardSlot);
	}

	private Selectable GetCardSelectable(GameObject cardSlot)
	{
		ArtifactDealCardPlayPresenter componentInChildren = cardSlot.GetComponentInChildren<ArtifactDealCardPlayPresenter>(true);
		if (componentInChildren == null)
		{
			return null;
		}
		return componentInChildren.NavButton;
	}

	private void SetRemainPlayCount()
	{
		SetRemainCount(_artifactDealManager.RemainPlayCount);
		if (IsNegotiatingState(_artifactDealManager.RxDealState.Value))
		{
			RebuildMiniGameNav(FocusedSelectable);
		}
	}

	private void SetRemainRerollCount()
	{
		_view.CardPlayZone_Reroll_Button_TextSub.text = string.Format("({0}/{1})", _artifactDealManager.RemainRerollCount, _artifactDealManager.MaxRerollCount);
	}

	private void SetDealPrice(int dealPrice, int originalPrice, float duration = 0f)
	{
		_dealCoinTweener?.Kill();
		_dealCoinTweener = null;
		ApplyMatchBonusBubble();
		int num = dealPrice - originalPrice;
		int num2 = ToBalancePercent(dealPrice, originalPrice);
		int num3 = dealPrice / TConst.CoinToGoldRate.IntValue;
		_view.DealingPrice_Body_Coin_TextBalanceValue.text = ((num == 0) ? string.Empty : string.Format("{0}%", num2));
		_view.DealingPrice_Body_Coin_TextBalanceValue.color = ((num >= 0) ? new Color32(90, 113, 74, byte.MaxValue) : new Color32(148, 77, 78, byte.MaxValue));
		_view.DealingPrice_Body_Coin_TextBalanceValue_BalanceUp.gameObject.SetActive(num > 0);
		_view.DealingPrice_Body_Coin_TextBalanceValue_BalanceDown.gameObject.SetActive(num < 0);
		_view.DealingPrice_Body_Coin_TextBalanceValue_BalanceNone.gameObject.SetActive(num == 0);
		if (duration <= 0f)
		{
			_view.DealingPrice_Body_Coin_TextBalanceValue_TextValue.text = num3.ToString();
			return;
		}
		PlayRollingSfx();
		int current = (int.TryParse(_view.DealingPrice_Body_Coin_TextBalanceValue_TextValue.text, out var result) ? result : 0);
		_dealCoinTweener = DOTween.To(() => current, delegate(int x)
		{
			current = x;
			_view.DealingPrice_Body_Coin_TextBalanceValue_TextValue.text = x.ToString();
		}, num3, duration).SetEase(Ease.OutQuad).SetLink(base.gameObject);
	}

	private void ApplyMatchBonusBubble()
	{
		EArtifactDealTagMatchTier tagMatchTier = _artifactDealManager.GetTagMatchTier();
		_view.DealingPrice_Body_DealingPriceBubble.SetActive(tagMatchTier != EArtifactDealTagMatchTier.None);
		if (tagMatchTier != EArtifactDealTagMatchTier.None)
		{
			float setMatchBonusMultiplier = _artifactDealManager.GetSetMatchBonusMultiplier(_artifactDealManager.MatchedPreferCount);
			_view.DealingPrice_Body_DealingPriceBubble_Normal_BG_TextTagMatch.text = string.Format(TLocalizedText.ArtifactDealMatchedPreferTagCountFormat.LocalText, _artifactDealManager.MatchedPreferCount);
			_view.DealingPrice_Body_DealingPriceBubble_Normal_BG_Text.text = string.Format("x{0:F2}", setMatchBonusMultiplier);
		}
	}

	private void SetDealExp(int dealExp, float duration = 0f)
	{
		_dealExpTweener?.Kill();
		_dealExpTweener = null;
		if (duration <= 0f)
		{
			_view.DealingPrice_Body_Exp_TextExp_TextValue.text = dealExp.ToString();
			return;
		}
		PlayRollingSfx();
		int current = (int.TryParse(_view.DealingPrice_Body_Exp_TextExp_TextValue.text, out var result) ? result : 0);
		_dealExpTweener = DOTween.To(() => current, delegate(int x)
		{
			current = x;
			_view.DealingPrice_Body_Exp_TextExp_TextValue.text = x.ToString();
		}, dealExp, duration).SetEase(Ease.OutQuad).SetLink(base.gameObject);
	}

	private void SetAllToDomain()
	{
		SetRequirementSlots();
		SetDealPrice(_artifactDealManager.GetDealPrice(), _artifactDealManager.GetOriginalPrice());
		SetDealExp((int)_artifactDealManager.GetDealExp());
		SetRemainPlayCount();
		SetRemainRerollCount();
		SetCards();
	}

	private void ClearDealPrice()
	{
		_view.DealingPrice_Body_Coin_TextBalanceValue_TextValue.text = "0";
		_view.DealingPrice_Body_Coin_TextBalanceValue.text = string.Empty;
		_view.DealingPrice_Body_Change_TextChangeUp.gameObject.SetActive(false);
		_view.DealingPrice_Body_Change_TextChangeDown.gameObject.SetActive(false);
		_view.DealingPrice_Body_Change.SetActive(false);
		_view.DealingPrice_Body_Coin_TextBalanceValue_BalanceUp.gameObject.SetActive(false);
		_view.DealingPrice_Body_Coin_TextBalanceValue_BalanceDown.gameObject.SetActive(false);
		_view.DealingPrice_Body_Coin_TextBalanceValue_BalanceNone.gameObject.SetActive(false);
	}

	private void ClearDealExp()
	{
		_view.DealingPrice_Body_Exp_TextExp_TextValue.text = "0";
	}

	private void ClearRemainPlayCount()
	{
		_remainCountTweener?.Kill();
		_remainCountTweener = null;
		_view.RemainPlayCount_Count.text = 0.ToString();
	}

	private void Back()
	{
		EArtifactDealState value = _artifactDealManager.RxDealState.Value;
		if (value == EArtifactDealState.AwaitSelectArtifact)
		{
			_artifactDealManager.RequestStateNegotiationStopping();
		}
		else if ((value >= EArtifactDealState.AwaitAction && value <= EArtifactDealState.PlayDealSetup) || value == EArtifactDealState.PlaySwapSetup)
		{
			if (DealData.AllowGiveUp)
			{
				OnClickGiveUpAsync().Forget();
			}
		}
		else if (value == EArtifactDealState.AwaitSwapArtifact)
		{
			_artifactDealManager.RequestStateNegotiatingCancel();
		}
	}

	private bool ShouldAutoSwitchToStash()
	{
		int num = _gameManager.MyInventory.Slots.Count((KeyValuePair<int, GoodsSlot> kv) => IsDealableArtifactSlot(kv.Value));
		int num2 = _playerManager.StashSlots.Count((KeyValuePair<int, GoodsSlot> kv) => IsDealableArtifactSlot(kv.Value));
		return num == 0 && num2 > 0;
	}

	private bool IsDealableArtifactSlot(GoodsSlot slot)
	{
		if (slot.Type != EGoodsType.Artifact)
		{
			return false;
		}
		return _artifactDealManager.Categories.Contains(slot.Goods.GetArtifactCategory());
	}

	private void SwapItemPanel()
	{
		EArtifactDealState value = _artifactDealManager.RxDealState.Value;
		if (value == EArtifactDealState.AwaitSelectArtifact || value == EArtifactDealState.AwaitSwapArtifact)
		{
			_itemPanelType = ((_itemPanelType == EItemPanelType.Inventory) ? EItemPanelType.Stash : EItemPanelType.Inventory);
			ApplyStateUi(value);
		}
	}

	private async UniTaskVoid OnSlotClick(InventoryGoodsSlotModel slotModel)
	{
		if (slotModel.GoodsSlot.IsEmpty || slotModel.GoodsSlot.Type != EGoodsType.Artifact)
		{
			return;
		}
		_soundManager.PlaySfx("ButtonClick2");
		if (!(await _popupManager.ShowConfirmationTextPopupAsync(TLocalizedText.ConfirmArtifactDeal.LocalText)))
		{
			return;
		}
		Goods goods = slotModel.GoodsSlot.Goods;
		EArtifactDealState value = _artifactDealManager.RxDealState.Value;
		if (value != EArtifactDealState.AwaitSwapArtifact && _model.NpcObject != null)
		{
			_artifactDealManager.MarkDealDone(_model.ArtifactDealId);
		}
		_dealingSlotModel = slotModel;
		_view.DealingArtifact_GoodsSlot.Inject(new InventoryGoodsSlotModel(_dealingSlotModel.GoodsSlot, InventoryGoodsSlotPresenter.ELayout.Normal));
		_dealingTooltipModel = new GoodsTooltipModel
		{
			Goods = goods,
			HideEffectGroup = true
		};
		if (_view.DealingArtifactTooltip.gameObject.activeInHierarchy)
		{
			_view.DealingArtifactTooltip.Inject(_dealingTooltipModel);
		}
		if (value != EArtifactDealState.AwaitSwapArtifact)
		{
			_artifactDealManager.RequestStateNegotiatingInitial(goods);
		}
		else
		{
			_artifactDealManager.RequestStateNegotiatingAfterSwap(goods);
		}
	}

	private void OnEquipmentSlotClick(InventoryGoodsSlotModel slotModel)
	{
		if (!slotModel.GoodsSlot.IsEmpty && slotModel.GoodsSlot.Type == EGoodsType.Artifact)
		{
			_soundManager.PlaySfx("Fail");
			_toastManager.NoticeFail(TLocalizedText.CannotDealEquipedArtifact.LocalText);
		}
	}

	private void OnCardSlotClick(ArtifactDealCardPlayModel model)
	{
		if (!_artifactDealManager.RxIsCardInteractable.Value)
		{
			return;
		}
		ECardPlayability eCardPlayability = _artifactDealManager.GetCardPlayability(model.Id);
		string message;
		switch (eCardPlayability)
		{
		case ECardPlayability.Playable:
		{
			_soundManager.PlaySfx("ArtifactDeal_Card_Use");
			_lastPlayedCardIndex = _artifactDealManager.GetCardIndex(model.Id);
			if (_cardPlaySlotsByCardId.TryGetValue(model.Id, out var value))
			{
				value.PlayCardUp();
				PlayCardCompleteAfterDelayAsync(value);
			}
			_artifactDealManager.RequestPlayCard(model.Id);
			return;
		}
		case ECardPlayability.NoPlayCount:
			message = TLocalizedText.ArtifactDealNoMorePlayCount.LocalText;
			break;
		case ECardPlayability.AlreadyHavePreferTag:
			message = TLocalizedText.ArtifactDealCannotPlayAlreadyHavePreferTag.LocalText;
			break;
		case ECardPlayability.PreferTagsAtMaxCount:
			message = string.Format(TLocalizedText.ArtifactDealCannotPlayPreferTagsAtMaxCountFormat.LocalText, TConst.ArtifactDealMaxPreferTagCount.IntValue);
			break;
		case ECardPlayability.NoDislikeTagsToRemove:
			message = TLocalizedText.ArtifactDealCannotPlayNoDislikeTagsToRemove.LocalText;
			break;
		default:
			throw new System.Exception(string.Format("Unexpected playability: {0}", eCardPlayability));
		}
		_soundManager.PlaySfx("Fail");
		_toastManager.NoticeFail(message);
	}

	private void OnClickReroll()
	{
		if (_artifactDealManager.RxDealState.Value == EArtifactDealState.AwaitAction)
		{
			if (_artifactDealManager.RemainRerollCount > 0)
			{
				_artifactDealManager.RequestReroll();
			}
			else
			{
				_soundManager.PlaySfx("Fail");
				_toastManager.NoticeFail(TLocalizedText.ArtifactDealNoMoreRerollCount.LocalText);
			}
		}
	}

	private void OnClickAcceptDeal()
	{
		OnClickAcceptDealAsync().Forget();
	}

	private async UniTask OnClickAcceptDealAsync()
	{
		EArtifactDealState value = _artifactDealManager.RxDealState.Value;
		if ((value == EArtifactDealState.AwaitAction || value == EArtifactDealState.AwaitActionWithNoCount) && await _popupManager.ShowConfirmationTextPopupAsync(TLocalizedText.ConfirmArtifactDeal.LocalText))
		{
			_artifactDealManager.RequestStateNegotiationConcluding();
		}
	}

	private void OnClickSwapArtifact()
	{
		if (_artifactDealManager.RxDealState.Value == EArtifactDealState.AwaitAction && _artifactDealManager.RemainPlayCount >= 1)
		{
			_artifactDealManager.RequestStateSwappingArtifact();
		}
	}

	private async UniTask OnClickGiveUpAsync()
	{
		if (await _popupManager.ShowConfirmationTextPopupAsync(TLocalizedText.ArtifactDealGiveUpAskMassage.LocalText))
		{
			_artifactDealManager.RequestStateNegotiationGivingUp();
		}
	}

	private void PlayStopping()
	{
		Close(false);
		_artifactDealManager.NotifyStateFinished(EArtifactDealState.StopBeforeDeal);
	}

	private async UniTaskVoid PlayGivingUpAsync()
	{
		CancellationToken ct = this.GetCancellationTokenOnDestroy();
		TriggerNegotiationVipAchievement(isAccepted: false, 0);
		ShowTextBubble(PickBubbleText(DealData.BubbleTextsOnDealGiveUp), -1f);
		if (!(await WaitSequenceStepAsync(EArtifactDealStepDelay.DealGiveUpBubbleWait, ct)))
		{
			await UniTask.NextFrame();
			Close();
			_artifactDealManager.NotifyStateFinished(EArtifactDealState.GiveUpDeal);
			OpenBackOutPopupAfterCloseAsync();
		}
	}

	private void TriggerNegotiationVipAchievement(bool isAccepted, int coin)
	{
		ulong sourceClientId = ((_model.NpcObject == null) ? 0 : _model.NpcObject.NetworkObjectId);
		int goodsId = _dealingSlotModel.GoodsId;
		int npcId = DealData.NpcId;
		_eventSystem.Trigger(new ArtifactDealEvent(sourceClientId, goodsId, coin, npcId, isAccepted));
	}

	private async UniTaskVoid OpenBackOutPopupAfterCloseAsync()
	{
		await WaitUntilSelfRemovedAsync();
		_popupManager.Open<ArtifactDealBackOutPopupPresenter>(new ArtifactDealBackOutPopupModel
		{
			ArtifactDealId = _model.ArtifactDealId
		});
	}

	private async UniTaskVoid PlayConcludingAsync()
	{
		EGoodsType goodsType = _dealingSlotModel.GoodsType;
		Goods goods = GoodsCreator.MakeGoods(goodsType, _dealingSlotModel.GoodsId, 1L);
		int dealCoin = _artifactDealManager.GetDealPrice() / TConst.CoinToGoldRate.IntValue;
		int gainExp = (int)_artifactDealManager.GetDealExp();
		int originalPrice = _artifactDealManager.GetOriginalPrice();
		int initialCoin = originalPrice / TConst.CoinToGoldRate.IntValue;
		int balancePercent = ((originalPrice == 0) ? 0 : ToBalancePercent(_artifactDealManager.GetDealPrice(), originalPrice));
		Goods displayGoods = _dealingSlotModel.GoodsSlot.Goods.Clone();
		ResDealArtifact resDealArtifact = await _gameChannel.RequestAsync<ResDealArtifact>(new ReqDealArtifact
		{
			SlotCategory = _dealingSlotModel.SlotCategory,
			SlotIndex = _dealingSlotModel.SlotIndex,
			DealGoods = goods,
			GainCoin = dealCoin,
			GainExp = _artifactDealManager.GetDealExp()
		});
		if (resDealArtifact != null && resDealArtifact.IsSuccess)
		{
			TriggerNegotiationVipAchievement(isAccepted: true, dealCoin);
			_soundManager.PlaySfx("Casher");
			await UniTask.NextFrame();
			_uiEventSystem.FlushLevelAndExp();
			ShowTextBubble(PickBubbleText(DealData.BubbleTextsOnDealAccepted), -1f);
			if (await WaitSequenceStepAsync(_sequenceTimeConfig.GetSeconds(EArtifactDealStepDelay.DealAcceptedBubbleWait), this.GetCancellationTokenOnDestroy()))
			{
				return;
			}
			Close();
			_artifactDealManager.NotifyStateFinished(EArtifactDealState.AcceptDeal);
			OpenCompletePopupAfterCloseAsync(gainExp, initialCoin, dealCoin, balancePercent, displayGoods);
		}
		else
		{
			Close();
			_artifactDealManager.NotifyStateFinished(EArtifactDealState.AcceptDeal);
		}
	}

	private async UniTask WaitUntilSelfRemovedAsync()
	{
		await UniTask.WaitUntil(() => !_popupManager.ExistPopup<ArtifactDealPopupPresenter>());
	}

	private async UniTaskVoid OpenCompletePopupAfterCloseAsync(int gainExp, int initialCoin, int finalCoin, int balancePercent, Goods dealGoods)
	{
		await WaitUntilSelfRemovedAsync();
		_popupManager.Open<ArtifactDealCompletePopupPresenter>(new ArtifactDealCompletePopupModel
		{
			ArtifactDealId = _model.ArtifactDealId,
			GainExp = gainExp,
			InitialCoin = initialCoin,
			FinalCoin = finalCoin,
			BalancePercent = balancePercent,
			DealGoods = dealGoods
		});
	}
}
