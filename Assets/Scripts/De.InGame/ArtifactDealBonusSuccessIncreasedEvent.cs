public readonly struct ArtifactDealBonusSuccessIncreasedEvent : IArtifactDealEvent
{
	public int Amount { get; }

	public int NewBonusPercent { get; }

	public ArtifactDealBonusSuccessIncreasedEvent(int amount, int newBonusPercent)
	{
		Amount = 0;
		NewBonusPercent = 0;
	}
}
