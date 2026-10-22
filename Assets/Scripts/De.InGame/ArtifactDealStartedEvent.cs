public readonly struct ArtifactDealStartedEvent : IArtifactDealEvent
{
	public int[] InitialHand { get; }

	public int InitialPlayCount { get; }

	public int OriginalPrice { get; }

	public int DealPrice { get; }

	public EArtifactDealTagMatchTier MatchTier { get; }

	public int[] MatchedPreferIds { get; }

	public int[] MatchedDislikeIds { get; }

	public ArtifactDealStartedEvent(int[] initialHand, int initialPlayCount, int originalPrice, int dealPrice, EArtifactDealTagMatchTier matchTier, int[] matchedPreferIds, int[] matchedDislikeIds)
	{
		InitialHand = initialHand;
		InitialPlayCount = initialPlayCount;
		OriginalPrice = originalPrice;
		DealPrice = dealPrice;
		MatchTier = matchTier;
		MatchedPreferIds = matchedPreferIds;
		MatchedDislikeIds = matchedDislikeIds;
	}
}
