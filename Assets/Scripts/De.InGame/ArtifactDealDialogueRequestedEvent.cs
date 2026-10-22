public readonly struct ArtifactDealDialogueRequestedEvent : IArtifactDealEvent
{
	public EArtifactDealDialogueSlot Slot { get; }

	public ArtifactDealDialogueRequestedEvent(EArtifactDealDialogueSlot slot)
	{
		Slot = EArtifactDealDialogueSlot.Open;
	}
}
