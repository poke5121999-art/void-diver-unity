public readonly struct ArtifactDealDislikeTagsRerolledEvent : IArtifactDealEvent
{
	public int[] BeforeSlots { get; }

	public int[] AfterSlots { get; }

	public ArtifactDealDislikeTagsRerolledEvent(int[] beforeSlots, int[] afterSlots)
	{
		BeforeSlots = beforeSlots;
		AfterSlots = afterSlots;
	}
}
