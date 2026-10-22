using GameCommon.Data;

public class ItemCollectedEvent : IBaseEvent
{
	public int ItemId { get; private set; }

	public ItemCollectedEvent(int itemId)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
