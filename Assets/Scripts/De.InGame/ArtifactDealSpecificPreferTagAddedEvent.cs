public readonly struct ArtifactDealSpecificPreferTagAddedEvent : IArtifactDealEvent
{
	public int AddedId { get; }

	public ArtifactDealSpecificPreferTagAddedEvent(int addedId)
	{
		AddedId = addedId;
	}
}
