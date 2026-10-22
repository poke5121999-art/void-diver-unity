using GameCommon.Data;

public class LoungeQuestClearEvent : IBaseEvent
{
	public int LoungeQuestId { get; private set; }

	public LoungeQuestClearEvent(int loungeQuestId)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
