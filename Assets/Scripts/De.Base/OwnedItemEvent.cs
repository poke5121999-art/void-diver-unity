using GameCommon.Data;

public class OwnedItemEvent : IBaseEvent
{
	public int ItemId { get; private set; }

	public long ItemCountInInventory { get; private set; }

	public long ItemCountInStash { get; private set; }

	public long TeamItemCountInInventory { get; private set; }

	public OwnedItemEvent(int itemId, long itemCountInInventory, long itemCountInStash, long teamItemCountInInventory)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
