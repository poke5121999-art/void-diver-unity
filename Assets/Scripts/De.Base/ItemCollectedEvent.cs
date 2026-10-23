using GameCommon.Enum;
using GameCommon.Data;

public class ItemCollectedEvent : IBaseEvent
{
	public int ItemId { get; private set; }

	public ItemCollectedEvent(int itemId)
	{
		ItemId = itemId;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		if (eventCondition.Type == EEventConditionType.ItemCollected && ItemId == eventCondition.Value1)
		{
			currentProgress = 1L;
		}
		return currentProgress;
	}
}
