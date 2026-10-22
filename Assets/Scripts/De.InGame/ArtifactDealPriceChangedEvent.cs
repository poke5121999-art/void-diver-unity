public readonly struct ArtifactDealPriceChangedEvent : IArtifactDealEvent
{
	public int Before { get; }

	public int After { get; }

	public int OriginalPrice { get; }

	public int AfterExp { get; }

	public ArtifactDealPriceChangedEvent(int before, int after, int originalPrice, int afterExp)
	{
		Before = before;
		After = after;
		OriginalPrice = originalPrice;
		AfterExp = afterExp;
	}
}
