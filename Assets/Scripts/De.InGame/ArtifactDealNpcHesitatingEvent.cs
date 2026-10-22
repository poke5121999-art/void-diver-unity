public readonly struct ArtifactDealNpcHesitatingEvent : IArtifactDealEvent
{
	public EArtifactDealSuccessRateTier Tier { get; }

	public ArtifactDealNpcHesitatingEvent(EArtifactDealSuccessRateTier tier)
	{
		Tier = tier;
	}
}
