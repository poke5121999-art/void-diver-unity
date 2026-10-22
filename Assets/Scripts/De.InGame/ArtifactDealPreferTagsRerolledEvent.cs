public readonly struct ArtifactDealPreferTagsRerolledEvent : IArtifactDealEvent
{
	public int[] BeforeSlots { get; }

	public int[] AfterSlots { get; }

	public ArtifactDealPreferTagsRerolledEvent(int[] beforeSlots, int[] afterSlots)
	{
		BeforeSlots = null;
		AfterSlots = null;
	}
}
