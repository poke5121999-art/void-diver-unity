public readonly struct ArtifactDealConcludedEvent : IArtifactDealEvent
{
	public int ArtifactDealId { get; }

	public int OriginalPrice { get; }

	public int FinalPrice { get; }

	public EArtifactDealTagMatchTier MatchTier { get; }

	public bool MatchBroken { get; }

	public long GainExp { get; }

	public int GainCoin { get; }

	public bool IsSkipNegotiation { get; }

	public ArtifactDealConcludedEvent(int artifactDealId, int originalPrice, int finalPrice, EArtifactDealTagMatchTier matchTier, bool matchBroken, long gainExp, int gainCoin, bool isSkipNegotiation)
	{
		ArtifactDealId = 0;
		OriginalPrice = 0;
		FinalPrice = 0;
		MatchTier = EArtifactDealTagMatchTier.None;
		MatchBroken = false;
		GainExp = 0L;
		GainCoin = 0;
		IsSkipNegotiation = false;
	}
}
