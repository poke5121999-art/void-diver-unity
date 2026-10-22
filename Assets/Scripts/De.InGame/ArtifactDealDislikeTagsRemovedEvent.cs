public readonly struct ArtifactDealDislikeTagsRemovedEvent : IArtifactDealEvent
{
	public int[] RemovedIds { get; }

	public ArtifactDealDislikeTagsRemovedEvent(int[] removedIds)
	{
		RemovedIds = removedIds;
	}
}
