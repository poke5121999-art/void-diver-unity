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
		InitialHand = null;
		InitialPlayCount = 0;
		OriginalPrice = 0;
		DealPrice = 0;
		MatchTier = EArtifactDealTagMatchTier.None;
		MatchedPreferIds = null;
		MatchedDislikeIds = null;
	}
}
