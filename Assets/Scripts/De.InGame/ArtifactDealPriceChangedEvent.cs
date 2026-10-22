public readonly struct ArtifactDealPriceChangedEvent : IArtifactDealEvent
{
	public int Before { get; }

	public int After { get; }

	public int OriginalPrice { get; }

	public int AfterExp { get; }

	public ArtifactDealPriceChangedEvent(int before, int after, int originalPrice, int afterExp)
	{
		Before = 0;
		After = 0;
		OriginalPrice = 0;
		AfterExp = 0;
	}
}
