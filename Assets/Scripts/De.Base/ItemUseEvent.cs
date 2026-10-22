using GameCommon.Data;

public class ItemUseEvent : IBaseEvent
{
	public int ItemId { get; private set; }

	public ItemUseEvent(int itemId)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
