public readonly struct ArtifactDealCardConsumedEvent : IArtifactDealEvent
{
	public int CardId { get; }

	public int SlotIndex { get; }

	public int RemainPlayCount { get; }

	public ArtifactDealCardConsumedEvent(int cardId, int slotIndex, int remainPlayCount)
	{
		CardId = 0;
		SlotIndex = 0;
		RemainPlayCount = 0;
	}
}
