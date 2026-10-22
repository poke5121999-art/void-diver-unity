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
		ArtifactDealId = artifactDealId;
		OriginalPrice = originalPrice;
		FinalPrice = finalPrice;
		MatchTier = matchTier;
		MatchBroken = matchBroken;
		GainExp = gainExp;
		GainCoin = gainCoin;
		IsSkipNegotiation = isSkipNegotiation;
	}
}
