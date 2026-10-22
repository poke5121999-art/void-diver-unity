public readonly struct ArtifactDealCardConsumedEvent : IArtifactDealEvent
{
	public int CardId { get; }

	public int SlotIndex { get; }

	public int RemainPlayCount { get; }

	public ArtifactDealCardConsumedEvent(int cardId, int slotIndex, int remainPlayCount)
	{
		CardId = cardId;
		SlotIndex = slotIndex;
		RemainPlayCount = remainPlayCount;
	}
}
