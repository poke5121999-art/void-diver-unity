public readonly struct ArtifactDealMatchingBonusChangedEvent : IArtifactDealEvent
{
	public EArtifactDealTagMatchTier BeforeTier { get; }

	public EArtifactDealTagMatchTier AfterTier { get; }

	public ArtifactDealMatchingBonusChangedEvent(EArtifactDealTagMatchTier beforeTier, EArtifactDealTagMatchTier afterTier)
	{
		BeforeTier = beforeTier;
		AfterTier = afterTier;
	}
}
