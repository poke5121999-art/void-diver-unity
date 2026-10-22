public readonly struct ArtifactDealPreferTagsAddedEvent : IArtifactDealEvent
{
	public int[] AddedIds { get; }

	public ArtifactDealPreferTagsAddedEvent(int[] addedIds)
	{
		AddedIds = null;
	}
}
