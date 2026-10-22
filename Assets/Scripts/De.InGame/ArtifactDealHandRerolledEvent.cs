public readonly struct ArtifactDealHandRerolledEvent : IArtifactDealEvent
{
	public int[] NewHand { get; }

	public int RemainRerollCount { get; }

	public ArtifactDealHandRerolledEvent(int[] newHand, int remainRerollCount)
	{
		NewHand = newHand;
		RemainRerollCount = remainRerollCount;
	}
}
