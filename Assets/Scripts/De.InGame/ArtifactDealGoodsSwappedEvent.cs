public readonly struct ArtifactDealGoodsSwappedEvent : IArtifactDealEvent
{
	public int PriceBefore { get; }

	public int PriceAfter { get; }

	public ArtifactDealGoodsSwappedEvent(int priceBefore, int priceAfter)
	{
		PriceBefore = priceBefore;
		PriceAfter = priceAfter;
	}
}
