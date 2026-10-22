public readonly struct ArtifactDealCardThrownEvent : IArtifactDealEvent
{
	public int CardId { get; }

	public int SlotIndex { get; }

	public ArtifactDealCardThrownEvent(int cardId, int slotIndex)
	{
		CardId = 0;
		SlotIndex = 0;
	}
}
