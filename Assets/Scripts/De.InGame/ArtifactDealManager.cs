using System;
using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
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

	public IReadOnlyList<int> Hand => null;

	public int MaxRerollCount { get; private set; }

	public IReadOnlyCollection<EArtifactCategory> Categories => null;

	public IReadOnlyList<int> PreferPrefixIds => null;

	public IReadOnlyList<int> DislikePrefixIds => null;

	public Goods SelectedGoods { get; private set; }

	public int RemainPlayCount { get; private set; }

	public int RemainRerollCount { get; private set; }

	public int BonusSuccessPercent { get; private set; }

	public IReadOnlyList<IArtifactDealEvent> PendingEvents => null;

	public int MatchedPreferCount => 0;

	protected override void Awake()
	{
	}

	public void Initialize(int artifactDealId, IReadOnlyList<(EArtifactCategory Category, int[] Prefixes)> ownedArtifacts)
	{
	}

	public void RequestStateSelectingArtifact()
	{
	}

	public void RequestStateNegotiatingInitial(Goods goods)
	{
	}

	public void RequestStateNegotiatingAfterSwap(Goods goods)
	{
	}

	public void RequestStateSwappingArtifact()
	{
	}

	public void RequestStateNegotiatingCancel()
	{
	}

	public void RequestStateNegotiationStopping()
	{
	}

	public void RequestStateNegotiationGivingUp()
	{
	}

	public void RequestStateNegotiationConcluding()
	{
	}

	public void NotifyStateFinished(EArtifactDealState finished)
	{
	}

	private void SetDealState(EArtifactDealState next)
	{
	}

	private DealInitialInfo InitializeInfo(int artifactDealId, IReadOnlyList<(EArtifactCategory Category, int[] Prefixes)> ownedArtifacts)
	{
		return null;
	}

	private HashSet<int> AssignPrefixSlots(int slotCount, IReadOnlyList<(EArtifactCategory Category, int[] Prefixes)> ownedArtifacts, int[] fallbackPool, bool forPrefer)
	{
		return null;
	}

	public void ClearInitialInfos()
	{
	}

	public bool IsDealDone(int artifactDealId)
	{
		return false;
	}

	public void MarkDealDone(int artifactDealId)
	{
	}

	public void ClearDoneStates()
	{
	}

	private static HashSet<EArtifactCategory> PickPreferCategories(EArtifactCategory[] pool, IEnumerable<EArtifactCategory> ownedCategories)
	{
		return null;
	}

	public void RequestStartDeal()
	{
	}

	public void RequestPlayCard(int cardId)
	{
	}

	public void RequestReroll()
	{
	}

	private void DrawCards(int count)
	{
	}

	private void ReturnCardToDeck(int cardId)
	{
	}

	private HashSet<int> GetArtifactPrefixSet()
	{
		return null;
	}

	private bool IsCardDeckCandidate(TArtifactDealCard data)
	{
		return false;
	}

	private bool HasAddSpecificPreferTagEffect(ArtifactDealCardEffect[] effects)
	{
		return false;
	}

	private bool IsCardRelevant(int cardId)
	{
		return false;
	}

	private int GetEffectiveCardWeight(int cardId)
	{
		return 0;
	}

	private int DrawCardId()
	{
		return 0;
	}

	public int GetCardIndex(int cardId)
	{
		return 0;
	}

	public ECardPlayability GetCardPlayability(int cardId)
	{
		return ECardPlayability.Playable;
	}

	public void OnSelectGoods(Goods goods)
	{
	}

	public int GetOriginalPrice()
	{
		return 0;
	}

	public int GetDealPrice()
	{
		return 0;
	}

	public int GetCardPlaySuccessPercent(int id)
	{
		return 0;
	}

	public long GetDealExp()
	{
		return 0L;
	}

	private int GetPriceMultiplierPercent()
	{
		return 0;
	}

	private void UpdateBonusPriceReasons()
	{
	}

	private static List<int> PickWeightedTags(IEnumerable<int> source, int count, Func<int, int> weightSelector)
	{
		return null;
	}

	private static List<int> PickTagsWithMatchGuarantee(IEnumerable<int> source, ISet<int> matchedTags, int count, int matchPercent)
	{
		return null;
	}

	private void ReplaceTagsInPlace(List<int> current, IReadOnlyList<int> toRemove, IReadOnlyList<int> newTags)
	{
	}

	private IEnumerable<int> GetTagDrawPool(bool forPrefer)
	{
		return null;
	}

	private void ApplyEffect(ArtifactDealCardEffect effect)
	{
	}

	private static EArtifactDealSuccessRateTier ToSuccessTier(int successPercent)
	{
		return EArtifactDealSuccessRateTier.High;
	}

	public EArtifactDealTagMatchTier GetTagMatchTier()
	{
		return EArtifactDealTagMatchTier.None;
	}

	public float GetSetMatchBonusMultiplier(int preferCount)
	{
		return 0f;
	}

	public float GetTagPriceMultiplier()
	{
		return 0f;
	}

	private (int[], int[]) GetMatchedPrefixIds()
	{
		return default;
	}

	private void EnqueueEffectEvent(ArtifactDealCardEffect effect, IReadOnlyList<int> preferBefore, IReadOnlyList<int> dislikeBefore)
	{
	}
}
