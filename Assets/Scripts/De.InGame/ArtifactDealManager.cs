using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using GameCommon.Utility;
using SomeNet.Utility;
using UniRx;

public class ArtifactDealManager : MonoBase
{
	public readonly ReactiveProperty<EArtifactDealState> RxDealState;

	public readonly ReactiveProperty<bool> RxIsCardInteractable;

	private readonly List<IArtifactDealEvent> _pendingEvents;

	private EArtifactDealState _stateBeforeSwap;

	private int _artifactDealId;

	private int _startHandCount;

	private int _maxPlayCount;

	private readonly List<int> _hand;

	private List<int> _deck;

	private HashSet<EArtifactCategory> _categories;

	private List<int> _preferPrefixIds;

	private List<int> _dislikePrefixIds;

	private readonly List<(ECardBonusPriceType Type, int Value)> _bonusPriceReasons;

	private readonly Dictionary<int, DealInitialInfo> _dealInitialInfos;

	private readonly HashSet<int> _doneDealIds;

	public IReadOnlyList<int> Hand => _hand;

	public int MaxRerollCount { get; private set; }

	public IReadOnlyCollection<EArtifactCategory> Categories => _categories;

	public IReadOnlyList<int> PreferPrefixIds => _preferPrefixIds;

	public IReadOnlyList<int> DislikePrefixIds => _dislikePrefixIds;

	public Goods SelectedGoods { get; private set; }

	public int RemainPlayCount { get; private set; }

	public int RemainRerollCount { get; private set; }

	public int BonusSuccessPercent { get; private set; }

	public IReadOnlyList<IArtifactDealEvent> PendingEvents => _pendingEvents;

	public int MatchedPreferCount => _bonusPriceReasons.Count(r => r.Type == ECardBonusPriceType.MatchPreferIdx);

	protected override void Awake()
	{
		base.Awake();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public void Initialize(int artifactDealId, IReadOnlyList<(EArtifactCategory Category, int[] Prefixes)> ownedArtifacts)
	{
		_artifactDealId = artifactDealId;
		DealInitialInfo info = null;
		_startHandCount = TConst.ArtifactDealHandsCount.IntValue;
		_maxPlayCount = TConst.ArtifactDealPlayCardCount.IntValue;
		MaxRerollCount = TConst.ArtifactDealRerollCount.IntValue;
		RemainRerollCount = MaxRerollCount;
		RemainPlayCount = _maxPlayCount;
		BonusSuccessPercent = 0;
		_bonusPriceReasons.Clear();
		SelectedGoods = null;
		_hand.Clear();
		_deck = TArtifactDealCard.Table.Values.Where(IsCardDeckCandidate).Select(c => c.Id).ToList();
		if (!_dealInitialInfos.TryGetValue(artifactDealId, out info))
		{
			info = InitializeInfo(artifactDealId, ownedArtifacts);
		}
		_categories = new HashSet<EArtifactCategory>(info.DealableCategories);
		_preferPrefixIds = info.PreferPrefixIds.OrderBy(i => i).ToList();
		_dislikePrefixIds = info.DislikePrefixIds.OrderBy(i => i).ToList();
		_stateBeforeSwap = EArtifactDealState.None;
		RxIsCardInteractable.Value = false;
		RxDealState.Value = EArtifactDealState.AwaitSelectArtifact;
	}

	public void RequestStateSelectingArtifact()
	{
		SetDealState(EArtifactDealState.AwaitSelectArtifact);
	}

	public void RequestStateNegotiatingInitial(Goods goods)
	{
		SelectedGoods = goods;
		UpdateBonusPriceReasons();
		DrawCards(_startHandCount);
		(int[] matchedPrefer, int[] matchedDislike) = GetMatchedPrefixIds();
		_pendingEvents.Clear();
		_pendingEvents.Add(new ArtifactDealStartedEvent(_hand.ToArray(), RemainPlayCount, SelectedGoods.GetArtifactPrice(), GetDealPrice(), GetTagMatchTier(), matchedPrefer, matchedDislike));
		SetDealState(EArtifactDealState.PlayDealSetup);
	}

	public void RequestStateNegotiatingAfterSwap(Goods goods)
	{
		int oldId = SelectedGoods.Id;
		int newId = goods.Id;
		int priceBefore = GetDealPrice();
		SelectedGoods = goods;
		UpdateBonusPriceReasons();
		_pendingEvents.Clear();
		_pendingEvents.Add(new ArtifactDealGoodsSwappedEvent(priceBefore, GetDealPrice()));
		SetDealState((oldId != newId) ? EArtifactDealState.PlaySwapSetup : EArtifactDealState.AwaitAction);
	}

	public void RequestStateSwappingArtifact()
	{
		_stateBeforeSwap = RxDealState.Value;
		SetDealState(EArtifactDealState.AwaitSwapArtifact);
	}

	public void RequestStateNegotiatingCancel()
	{
		SetDealState(_stateBeforeSwap);
	}

	public void RequestStateNegotiationStopping()
	{
		_pendingEvents.Clear();
		_pendingEvents.Add(new ArtifactDealStoppedEvent());
		SetDealState(EArtifactDealState.StopBeforeDeal);
	}

	public void RequestStateNegotiationGivingUp()
	{
		_pendingEvents.Clear();
		_pendingEvents.Add(new ArtifactDealForfeitedEvent());
		SetDealState(EArtifactDealState.GiveUpDeal);
	}

	public void RequestStateNegotiationConcluding()
	{
		int coinToGoldRate = TConst.CoinToGoldRate.IntValue;
		_pendingEvents.Clear();
		_pendingEvents.Add(new ArtifactDealConcludedEvent(_artifactDealId, SelectedGoods.GetArtifactPrice(), GetDealPrice(), GetTagMatchTier(), false, GetDealExp(), GetDealPrice() / coinToGoldRate, false));
		SetDealState(EArtifactDealState.AcceptDeal);
	}

	public void NotifyStateFinished(EArtifactDealState finished)
	{
		if (RxDealState.Value != finished)
		{
			return;
		}
		EArtifactDealState next;
		switch (finished)
		{
		case EArtifactDealState.PlayCard:
			next = (RemainPlayCount < 1) ? EArtifactDealState.AwaitActionWithNoCount : EArtifactDealState.AwaitAction;
			break;
		case EArtifactDealState.PlayReroll:
		case EArtifactDealState.PlayDealSetup:
		case EArtifactDealState.PlaySwapSetup:
			next = EArtifactDealState.AwaitAction;
			break;
		case EArtifactDealState.StopBeforeDeal:
		case EArtifactDealState.GiveUpDeal:
		case EArtifactDealState.AcceptDeal:
			next = EArtifactDealState.None;
			break;
		default:
			throw new InvalidOperationException(string.Format("Unhandled state finish: {0}", finished));
		}
		SetDealState(next);
	}

	private void SetDealState(EArtifactDealState next)
	{
		RxIsCardInteractable.Value = next == EArtifactDealState.AwaitAction;
		RxDealState.Value = next;
	}

	private DealInitialInfo InitializeInfo(int artifactDealId, IReadOnlyList<(EArtifactCategory Category, int[] Prefixes)> ownedArtifacts)
	{
		TArtifactDeal deal = TArtifactDeal.Table[artifactDealId];
		int preferSlotCount = RandomUtility.GetRandomInt(deal.PreferPrefixCountMin, deal.PreferPrefixCountMax + 1);
		int dislikeSlotCount = RandomUtility.GetRandomInt(deal.DislikePrefixCountMin, deal.DislikePrefixCountMax + 1);
		HashSet<EArtifactCategory> ownedCategories = ownedArtifacts.Select(a => a.Category).ToHashSet();
		DealInitialInfo info = new DealInitialInfo();
		info.DealableCategories = PickPreferCategories(deal.PreferCategories, ownedCategories);
		info.PreferPrefixIds = AssignPrefixSlots(preferSlotCount, ownedArtifacts, deal.PreferPrefixIds, true);
		info.DislikePrefixIds = AssignPrefixSlots(dislikeSlotCount, ownedArtifacts, deal.DislikePrefixIds, false);
		_dealInitialInfos[artifactDealId] = info;
		return info;
	}

	private HashSet<int> AssignPrefixSlots(int slotCount, IReadOnlyList<(EArtifactCategory Category, int[] Prefixes)> ownedArtifacts, int[] fallbackPool, bool forPrefer)
	{
		TArtifactDeal artifactDealData = TArtifactDeal.Table[_artifactDealId];
		int[] matchPercents = new int[3];
		if (forPrefer)
		{
			matchPercents[0] = TConst.ArtifactDealPreferMatchSlot1Percent.IntValue;
			matchPercents[1] = TConst.ArtifactDealPreferMatchSlot2Percent.IntValue;
			matchPercents[2] = TConst.ArtifactDealPreferMatchSlot3Percent.IntValue;
		}
		else
		{
			matchPercents[0] = TConst.ArtifactDealDislikeMatchSlot1Percent.IntValue;
			matchPercents[1] = TConst.ArtifactDealDislikeMatchSlot2Percent.IntValue;
			matchPercents[2] = TConst.ArtifactDealDislikeMatchSlot3Percent.IntValue;
		}
		bool poolIsNegative = forPrefer == artifactDealData.PrefersNegativeTags;
		List<List<int>> candidateTagsByArtifact = ownedArtifacts
			.Where(a => artifactDealData.PreferCategories.Contains(a.Category))
			.Select(a => a.Prefixes.Where(id => (TArtifactPrefix.Table[id].PriceMultiplier < 0f) == poolIsNegative).ToList())
			.Where(l => l.Count > 0)
			.ToList();
		HashSet<int> assignedPrefixes = new HashSet<int>();
		HashSet<int> matchedArtifactIndexes = new HashSet<int>();
		for (int slot = 0; slot < slotCount; slot++)
		{
			int matchPercent = (slot < matchPercents.Length) ? matchPercents[slot] : 0;
			List<int> availableArtifacts = Enumerable.Range(0, candidateTagsByArtifact.Count)
				.Where(i => !matchedArtifactIndexes.Contains(i))
				.Where(i => candidateTagsByArtifact[i].Any(id => !assignedPrefixes.Contains(id)))
				.ToList();
			if (RandomUtility.GetRandomInt(0, 100) < matchPercent && availableArtifacts.Count >= 1)
			{
				int artifactIndex = availableArtifacts[RandomUtility.GetRandomInt(0, availableArtifacts.Count)];
				List<int> tags = candidateTagsByArtifact[artifactIndex].Where(id => !assignedPrefixes.Contains(id)).ToList();
				assignedPrefixes.Add(tags[RandomUtility.GetRandomInt(0, tags.Count)]);
				matchedArtifactIndexes.Add(artifactIndex);
			}
			else
			{
				List<int> fallback = fallbackPool.Where(id => !assignedPrefixes.Contains(id)).ToList();
				if (fallback.Count > 0)
				{
					assignedPrefixes.Add(fallback[RandomUtility.GetRandomInt(0, fallback.Count)]);
				}
			}
		}
		return assignedPrefixes;
	}

	public void ClearInitialInfos()
	{
		_dealInitialInfos.Clear();
	}

	public bool IsDealDone(int artifactDealId)
	{
		return _doneDealIds.Contains(artifactDealId);
	}

	public void MarkDealDone(int artifactDealId)
	{
		_doneDealIds.Add(artifactDealId);
	}

	public void ClearDoneStates()
	{
		_doneDealIds.Clear();
	}

	private static HashSet<EArtifactCategory> PickPreferCategories(EArtifactCategory[] pool, IEnumerable<EArtifactCategory> ownedCategories)
	{
		int preferCount = TConst.ArtifactDealPreferCategoryCount.IntValue;
		int ownedWeightPercent = TConst.ArtifactDealOwnedCategoryWeightPercent.IntValue;
		HashSet<EArtifactCategory> owned = ownedCategories?.ToHashSet() ?? new HashSet<EArtifactCategory>();
		List<EArtifactCategory> ownedInPool = pool.Where(owned.Contains).ToList();
		HashSet<EArtifactCategory> result = new HashSet<EArtifactCategory>();
		List<EArtifactCategory> remaining = pool.ToList();
		if (ownedInPool.Count >= 1)
		{
			EArtifactCategory first = ownedInPool[RandomUtility.GetRandomInt(0, ownedInPool.Count)];
			result.Add(first);
			remaining.Remove(first);
		}
		while (result.Count < preferCount && remaining.Count >= 1)
		{
			EArtifactCategory picked = WeightedRandomSelector.SelectWeightedRandom(
				remaining.Select(c => (c, owned.Contains(c) ? ownedWeightPercent : 100)),
				remaining[0]);
			result.Add(picked);
			remaining.Remove(picked);
		}
		return result;
	}

	public void RequestStartDeal()
	{
		DrawCards(_startHandCount);
	}

	public void RequestPlayCard(int cardId)
	{
		if (RemainPlayCount < 1)
		{
			return;
		}
		int priceBefore = GetDealPrice();
		List<int> preferBefore = _preferPrefixIds.ToList();
		List<int> dislikeBefore = _dislikePrefixIds.ToList();
		EArtifactDealTagMatchTier tierBefore = GetTagMatchTier();
		int slotIndex = _hand.IndexOf(cardId);
		TArtifactDealCard card = TArtifactDealCard.Table[cardId];
		int successPercent = GetCardPlaySuccessPercent(cardId);
		int roll = RandomUtility.GetRandomInt(0, 100);
		bool isSuccess = roll < successPercent;
		ArtifactDealCardEffect[] effects = isSuccess ? card.SuccessEffects : card.FailEffects;
		if (effects != null && effects.Length > 0)
		{
			foreach (ArtifactDealCardEffect effect in effects)
			{
				ApplyEffect(effect);
			}
			UpdateBonusPriceReasons();
		}
		if (!isSuccess)
		{
			BonusSuccessPercent = 0;
		}
		RemainPlayCount--;
		if (slotIndex >= 0)
		{
			_hand[slotIndex] = -1;
		}
		_pendingEvents.Clear();
		EArtifactDealSuccessRateTier tier = EArtifactDealSuccessRateTier.High;
		if (successPercent < TConst.ArtifactDealSuccessTierHigh.IntValue)
		{
			tier = ToSuccessTier(successPercent);
			_pendingEvents.Add(new ArtifactDealNpcHesitatingEvent(tier));
		}
		_pendingEvents.Add(new ArtifactDealCardResolvedEvent(cardId, isSuccess, tier, effects));
		if (effects != null && effects.Length > 0)
		{
			foreach (ArtifactDealCardEffect effect in effects)
			{
				EnqueueEffectEvent(effect, preferBefore, dislikeBefore);
			}
		}
		int priceAfter = GetDealPrice();
		if (priceAfter != priceBefore)
		{
			_pendingEvents.Add(new ArtifactDealPriceChangedEvent(priceBefore, priceAfter, SelectedGoods.GetArtifactPrice(), (int)GetDealExp()));
		}
		if (!isSuccess)
		{
			_pendingEvents.Add(new ArtifactDealBonusSuccessResetEvent());
		}
		if (priceAfter < 2)
		{
			_pendingEvents.Add(new ArtifactDealPriceFloorReachedEvent());
		}
		EArtifactDealTagMatchTier tierAfter = GetTagMatchTier();
		if (tierAfter != tierBefore)
		{
			_pendingEvents.Add(new ArtifactDealMatchingBonusChangedEvent(tierBefore, tierAfter));
		}
		_pendingEvents.Add(new ArtifactDealCardConsumedEvent(cardId, slotIndex, RemainPlayCount));
		if (RemainPlayCount < 1)
		{
			_pendingEvents.Add(new ArtifactDealPlayCountExhaustedEvent());
		}
		SetDealState(EArtifactDealState.PlayCard);
	}

	public void RequestReroll()
	{
		RemainRerollCount--;
		for (int i = 0; i < _hand.Count; i++)
		{
			if (_hand[i] >= 0)
			{
				ReturnCardToDeck(_hand[i]);
			}
		}
		_hand.Clear();
		DrawCards(_startHandCount);
		_pendingEvents.Clear();
		_pendingEvents.Add(new ArtifactDealHandRerolledEvent(_hand.ToArray(), RemainRerollCount));
		SetDealState(EArtifactDealState.PlayReroll);
	}

	private void DrawCards(int count)
	{
		for (int i = 0; i < count; i++)
		{
			int cardId = DrawCardId();
			if (cardId < 0)
			{
				return;
			}
			_hand.Add(cardId);
		}
	}

	private void ReturnCardToDeck(int cardId)
	{
		_deck.Add(cardId);
	}

	private HashSet<int> GetArtifactPrefixSet()
	{
		int[] prefixes = SelectedGoods.GetPrefixes();
		if (prefixes == null)
		{
			return new HashSet<int>();
		}
		return prefixes.ToHashSet();
	}

	private bool IsCardDeckCandidate(TArtifactDealCard data)
	{
		TArtifactDeal deal = TArtifactDeal.Table[_artifactDealId];
		if (!deal.PrefersNegativeTags)
		{
			return true;
		}
		if (HasAddSpecificPreferTagEffect(data.SuccessEffects))
		{
			return false;
		}
		return !HasAddSpecificPreferTagEffect(data.FailEffects);
	}

	private bool HasAddSpecificPreferTagEffect(ArtifactDealCardEffect[] effects)
	{
		if (effects == null || effects.Length < 1)
		{
			return false;
		}
		return effects.Any(e => e.EffectType == EArtifactDealCardEffectType.AddSpecificPreferTag);
	}

	private bool IsCardRelevant(int cardId)
	{
		ArtifactDealCardEffect[] effects = TArtifactDealCard.Table[cardId].SuccessEffects;
		if (effects != null && effects.Length > 0)
		{
			HashSet<int> artifactPrefixes = GetArtifactPrefixSet();
			if (artifactPrefixes.Count != 0)
			{
				for (int i = 0; i < effects.Length; i++)
				{
					ArtifactDealCardEffect effect = effects[i];
					if (effect.EffectType == EArtifactDealCardEffectType.RemoveDislikeTags)
					{
						if (artifactPrefixes.Overlaps(_dislikePrefixIds))
						{
							return true;
						}
					}
					else if (effect.EffectType == EArtifactDealCardEffectType.AddSpecificPreferTag && artifactPrefixes.Contains(effect.Value) && !_preferPrefixIds.Contains(effect.Value))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	private int GetEffectiveCardWeight(int cardId)
	{
		TArtifactDealCard card = TArtifactDealCard.Table[cardId];
		int weight = card.Weight;
		int percent = IsCardRelevant(cardId) ? TConst.ArtifactDealCardRelevanceWeightPercent.IntValue : 100;
		return weight * percent / 100;
	}

	private int DrawCardId()
	{
		if (_deck.Count == 0)
		{
			return -1;
		}
		List<int> weights = _deck.Select(GetEffectiveCardWeight).ToList();
		int totalWeight = weights.Sum();
		int roll = RandomUtility.GetRandomInt(0, totalWeight);
		int deckCount = _deck.Count;
		int chosenIndex = deckCount - 1;
		int cumulative = 0;
		for (int i = 0; i < _deck.Count; i++)
		{
			cumulative += weights[i];
			if (roll < cumulative)
			{
				chosenIndex = i;
				break;
			}
		}
		int cardId = _deck[chosenIndex];
		_deck.RemoveAt(chosenIndex);
		return cardId;
	}

	public int GetCardIndex(int cardId)
	{
		return _hand.IndexOf(cardId);
	}

	public ECardPlayability GetCardPlayability(int cardId)
	{
		if (RemainPlayCount < 1)
		{
			return ECardPlayability.NoPlayCount;
		}
		ArtifactDealCardEffect[] effects = TArtifactDealCard.Table[cardId].SuccessEffects;
		if (effects == null || effects.Length < 1)
		{
			return ECardPlayability.Playable;
		}
		int maxPreferTagCount = TConst.ArtifactDealMaxPreferTagCount.IntValue;
		for (int i = 0; i < effects.Length; i++)
		{
			ArtifactDealCardEffect effect = effects[i];
			switch (effect.EffectType)
			{
			case EArtifactDealCardEffectType.AddPreferTags:
				if (_preferPrefixIds.Count >= maxPreferTagCount)
				{
					return ECardPlayability.PreferTagsAtMaxCount;
				}
				break;
			case EArtifactDealCardEffectType.RemoveDislikeTags:
				if (_dislikePrefixIds.Count == 0)
				{
					return ECardPlayability.NoDislikeTagsToRemove;
				}
				break;
			case EArtifactDealCardEffectType.AddSpecificPreferTag:
				if (_preferPrefixIds.Contains(effect.Value))
				{
					return ECardPlayability.AlreadyHavePreferTag;
				}
				if (_preferPrefixIds.Count >= maxPreferTagCount)
				{
					return ECardPlayability.PreferTagsAtMaxCount;
				}
				break;
			}
		}
		return ECardPlayability.Playable;
	}

	public void OnSelectGoods(Goods goods)
	{
		SelectedGoods = goods;
		UpdateBonusPriceReasons();
	}

	public int GetOriginalPrice()
	{
		return SelectedGoods.GetArtifactPrice();
	}

	public int GetDealPrice()
	{
		int price = SelectedGoods.GetArtifactPrice();
		int percent = GetPriceMultiplierPercent();
		int dealPrice = (int)((float)(percent * price) / 100f);
		if (dealPrice < 1)
		{
			dealPrice = 1;
		}
		return dealPrice;
	}

	public int GetCardPlaySuccessPercent(int id)
	{
		TArtifactDealCard card = TArtifactDealCard.Table[id];
		int minPercent = TConst.ArtifactDealMinSuccessPercent.IntValue;
		int maxPercent = TConst.ArtifactDealMaxSuccessPercent.IntValue;
		int percent = card.SuccessPercent + BonusSuccessPercent;
		if (percent < minPercent)
		{
			return minPercent;
		}
		if (percent > maxPercent)
		{
			percent = maxPercent;
		}
		return percent;
	}

	public long GetDealExp()
	{
		EGoodsGradeType grade = TEquipment.Table[SelectedGoods.Id].Grade;
		float expMultiplier = grade.GetArtifactDealExpMultiplier();
		if (expMultiplier > 0f)
		{
			int price = SelectedGoods.GetArtifactPrice();
			int percent = GetPriceMultiplierPercent();
			int dealPrice = (int)((float)(percent * price) / 100f);
			if (dealPrice < 1)
			{
				dealPrice = 1;
			}
			return (long)((float)dealPrice * expMultiplier);
		}
		return 0L;
	}

	private int GetPriceMultiplierPercent()
	{
		int cardEffectPercent = _bonusPriceReasons.Where(r => r.Type == ECardBonusPriceType.CardEffect).Sum(r => r.Value);
		int preferPercent = _bonusPriceReasons.Where(r => r.Type == ECardBonusPriceType.MatchPreferIdx).Sum(r => r.Value);
		int dislikePercent = _bonusPriceReasons.Where(r => r.Type == ECardBonusPriceType.MatchDislikeIdx).Sum(r => r.Value);
		int preferCount = _bonusPriceReasons.Count(r => r.Type == ECardBonusPriceType.MatchPreferIdx);
		float setMatchBonus = GetSetMatchBonusMultiplier(preferCount);
		return (int)(setMatchBonus * (float)(cardEffectPercent + preferPercent + 100 + dislikePercent));
	}

	private void UpdateBonusPriceReasons()
	{
		_bonusPriceReasons.RemoveAll(r => r.Type == ECardBonusPriceType.MatchPreferIdx || r.Type == ECardBonusPriceType.MatchDislikeIdx);
		int[] prefixes = SelectedGoods.GetPrefixes();
		if (prefixes == null)
		{
			return;
		}
		for (int i = 0; i < prefixes.Length; i++)
		{
			int prefixId = prefixes[i];
			if (_preferPrefixIds.Contains(prefixId))
			{
				_bonusPriceReasons.Add((ECardBonusPriceType.MatchPreferIdx, TArtifactPrefix.Table[prefixId].PreferPriceBonusPercent));
			}
			else if (_dislikePrefixIds.Contains(prefixId))
			{
				_bonusPriceReasons.Add((ECardBonusPriceType.MatchDislikeIdx, TArtifactPrefix.Table[prefixId].DislikePricePenaltyPercent));
			}
		}
	}

	private static List<int> PickWeightedTags(IEnumerable<int> source, int count, Func<int, int> weightSelector)
	{
		List<int> pool = source.ToList();
		List<int> result = new List<int>();
		while (result.Count < count && pool.Count >= 1)
		{
			int picked = WeightedRandomSelector.SelectWeightedRandom(pool.Select(id => (id, weightSelector(id))), pool[0]);
			result.Add(picked);
			pool.Remove(picked);
		}
		return result;
	}

	private static List<int> PickTagsWithMatchGuarantee(IEnumerable<int> source, ISet<int> matchedTags, int count, int matchPercent)
	{
		List<int> pool = source.ToList();
		List<int> result = new List<int>();
		while (result.Count < count && pool.Count >= 1)
		{
			List<int> matched = pool.Where(matchedTags.Contains).ToList();
			List<int> pickFrom = pool;
			if (matched.Count >= 1 && RandomUtility.GetRandomInt(0, 100) < matchPercent)
			{
				pickFrom = matched;
			}
			int picked = pickFrom[RandomUtility.GetRandomInt(0, pickFrom.Count)];
			result.Add(picked);
			pool.Remove(picked);
		}
		return result;
	}

	private void ReplaceTagsInPlace(List<int> current, IReadOnlyList<int> toRemove, IReadOnlyList<int> newTags)
	{
		for (int i = 0; i < toRemove.Count; i++)
		{
			current[current.IndexOf(toRemove[i])] = newTags[i];
		}
	}

	private IEnumerable<int> GetTagDrawPool(bool forPrefer)
	{
		TArtifactDeal deal = TArtifactDeal.Table[_artifactDealId];
		bool poolIsNegative = forPrefer == deal.PrefersNegativeTags;
		return TArtifactPrefix.Table.Values
			.Where(p => (p.PriceMultiplier < 0f) == poolIsNegative)
			.Select(p => p.Id);
	}

	private void ApplyEffect(ArtifactDealCardEffect effect)
	{
		switch (effect.EffectType)
		{
		case EArtifactDealCardEffectType.IncreasePricePercent:
			_bonusPriceReasons.Add((ECardBonusPriceType.CardEffect, effect.Value));
			break;
		case EArtifactDealCardEffectType.DecreasePricePercent:
			_bonusPriceReasons.Add((ECardBonusPriceType.CardEffect, -effect.Value));
			break;
		case EArtifactDealCardEffectType.AddPreferTags:
		{
			int addCount = Math.Min(Math.Max(0, TConst.ArtifactDealMaxPreferTagCount.IntValue - _preferPrefixIds.Count), effect.Value);
			IEnumerable<int> pool = GetTagDrawPool(true).Except(_preferPrefixIds);
			HashSet<int> artifactPrefixes = GetArtifactPrefixSet();
			List<int> added = PickTagsWithMatchGuarantee(pool, artifactPrefixes, addCount, TConst.ArtifactDealAddPreferMatchPercent.IntValue);
			foreach (int id in added)
			{
				_preferPrefixIds.Add(id);
			}
			break;
		}
		case EArtifactDealCardEffectType.RerollPreferTags:
		{
			HashSet<int> artifactPrefixes = GetArtifactPrefixSet();
			IEnumerable<int> pool = GetTagDrawPool(true).Except(_preferPrefixIds);
			List<int> newTags = PickTagsWithMatchGuarantee(pool, artifactPrefixes, effect.Value, TConst.ArtifactDealRerollPreferAddMatchPercent.IntValue);
			int matchedWeight = TConst.ArtifactDealRerollPreferMatchedWeightPercent.IntValue;
			List<int> toRemove = PickWeightedTags(_preferPrefixIds, newTags.Count, id => artifactPrefixes.Contains(id) ? matchedWeight : 100);
			ReplaceTagsInPlace(_preferPrefixIds, toRemove, newTags);
			break;
		}
		case EArtifactDealCardEffectType.RerollDislikeTags:
		{
			IEnumerable<int> pool = GetTagDrawPool(false).Except(_dislikePrefixIds);
			List<int> newTags = pool.OrderBy(id => RandomUtility.GetRandomFloat()).Take(effect.Value).ToList();
			List<int> currentDislike = _dislikePrefixIds;
			HashSet<int> artifactPrefixes = GetArtifactPrefixSet();
			List<int> toRemove = PickTagsWithMatchGuarantee(currentDislike, artifactPrefixes, newTags.Count, TConst.ArtifactDealRerollDislikeRemoveMatchPercent.IntValue);
			ReplaceTagsInPlace(_dislikePrefixIds, toRemove, newTags);
			break;
		}
		case EArtifactDealCardEffectType.RemoveDislikeTags:
		{
			HashSet<int> artifactPrefixes = GetArtifactPrefixSet();
			int matchedWeight = TConst.ArtifactDealRemoveDislikeMatchedWeightPercent.IntValue;
			List<int> picked = PickWeightedTags(_dislikePrefixIds, effect.Value, id => artifactPrefixes.Contains(id) ? matchedWeight : 100);
			foreach (int id in picked.ToHashSet())
			{
				_dislikePrefixIds.Remove(id);
			}
			break;
		}
		case EArtifactDealCardEffectType.AddSpecificPreferTag:
			if (_preferPrefixIds.Count < TConst.ArtifactDealMaxPreferTagCount.IntValue && !_preferPrefixIds.Contains(effect.Value))
			{
				_preferPrefixIds.Add(effect.Value);
			}
			break;
		case EArtifactDealCardEffectType.IncreaseBonusSuccessPercent:
			BonusSuccessPercent += effect.Value;
			break;
		}
	}

	private static EArtifactDealSuccessRateTier ToSuccessTier(int successPercent)
	{
		if (successPercent >= TConst.ArtifactDealSuccessTierHigh.IntValue)
		{
			return EArtifactDealSuccessRateTier.High;
		}
		return (successPercent < TConst.ArtifactDealSuccessTierMid.IntValue) ? EArtifactDealSuccessRateTier.Low : EArtifactDealSuccessRateTier.Mid;
	}

	public EArtifactDealTagMatchTier GetTagMatchTier()
	{
		int matchedPreferCount = MatchedPreferCount;
		if (matchedPreferCount > 7)
		{
			return EArtifactDealTagMatchTier.Match8Plus;
		}
		switch (matchedPreferCount)
		{
		case 2:
			return EArtifactDealTagMatchTier.Match2;
		case 3:
			return EArtifactDealTagMatchTier.Match3;
		case 4:
			return EArtifactDealTagMatchTier.Match4;
		case 5:
			return EArtifactDealTagMatchTier.Match5;
		case 6:
			return EArtifactDealTagMatchTier.Match6;
		case 7:
			return EArtifactDealTagMatchTier.Match7;
		default:
			return EArtifactDealTagMatchTier.None;
		}
	}

	public float GetSetMatchBonusMultiplier(int preferCount)
	{
		if (preferCount >= 8)
		{
			return TConst.ArtifactDealMatchBonusMultiplierOnAndAboveEight.FloatValue;
		}
		switch (preferCount)
		{
		case 2:
			return TConst.ArtifactDealMatchBonusMultiplierOnTwo.FloatValue;
		case 3:
			return TConst.ArtifactDealMatchBonusMultiplierOnThree.FloatValue;
		case 4:
			return TConst.ArtifactDealMatchBonusMultiplierOnFour.FloatValue;
		case 5:
			return TConst.ArtifactDealMatchBonusMultiplierOnFive.FloatValue;
		case 6:
			return TConst.ArtifactDealMatchBonusMultiplierOnSix.FloatValue;
		case 7:
			return TConst.ArtifactDealMatchBonusMultiplierOnSeven.FloatValue;
		default:
			return 1f;
		}
	}

	public float GetTagPriceMultiplier()
	{
		int preferPercent = _bonusPriceReasons.Where(r => r.Type == ECardBonusPriceType.MatchPreferIdx).Sum(r => r.Value);
		int dislikePercent = _bonusPriceReasons.Where(r => r.Type == ECardBonusPriceType.MatchDislikeIdx).Sum(r => r.Value);
		int preferCount = _bonusPriceReasons.Count(r => r.Type == ECardBonusPriceType.MatchPreferIdx);
		return GetSetMatchBonusMultiplier(preferCount) * ((float)(preferPercent + 100 + dislikePercent) / 100f);
	}

	private (int[], int[]) GetMatchedPrefixIds()
	{
		int[] prefixes = SelectedGoods.GetPrefixes();
		if (prefixes == null)
		{
			return (Array.Empty<int>(), Array.Empty<int>());
		}
		int[] matchedPrefer = prefixes.Where(_preferPrefixIds.Contains).ToArray();
		int[] matchedDislike = prefixes.Where(_dislikePrefixIds.Contains).ToArray();
		return (matchedPrefer, matchedDislike);
	}

	private void EnqueueEffectEvent(ArtifactDealCardEffect effect, IReadOnlyList<int> preferBefore, IReadOnlyList<int> dislikeBefore)
	{
		switch (effect.EffectType)
		{
		case EArtifactDealCardEffectType.AddPreferTags:
			_pendingEvents.Add(new ArtifactDealPreferTagsAddedEvent(_preferPrefixIds.Except(preferBefore).ToArray()));
			break;
		case EArtifactDealCardEffectType.RerollPreferTags:
			_pendingEvents.Add(new ArtifactDealPreferTagsRerolledEvent(preferBefore.ToArray(), _preferPrefixIds.ToArray()));
			break;
		case EArtifactDealCardEffectType.RerollDislikeTags:
			_pendingEvents.Add(new ArtifactDealDislikeTagsRerolledEvent(dislikeBefore.ToArray(), _dislikePrefixIds.ToArray()));
			break;
		case EArtifactDealCardEffectType.RemoveDislikeTags:
			_pendingEvents.Add(new ArtifactDealDislikeTagsRemovedEvent(dislikeBefore.Except(_dislikePrefixIds).ToArray()));
			break;
		case EArtifactDealCardEffectType.AddSpecificPreferTag:
		{
			int[] added = _preferPrefixIds.Except(preferBefore).ToArray();
			if (added.Length != 0)
			{
				_pendingEvents.Add(new ArtifactDealSpecificPreferTagAddedEvent(added[0]));
			}
			break;
		}
		case EArtifactDealCardEffectType.IncreaseBonusSuccessPercent:
			_pendingEvents.Add(new ArtifactDealBonusSuccessIncreasedEvent(effect.Value, BonusSuccessPercent));
			break;
		}
	}
}
