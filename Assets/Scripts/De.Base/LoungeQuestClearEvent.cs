using GameCommon.Enum;
using GameCommon.Data;

public class LoungeQuestClearEvent : IBaseEvent
{
	public int LoungeQuestId { get; private set; }

	public LoungeQuestClearEvent(int loungeQuestId)
	{
		LoungeQuestId = loungeQuestId;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		if (eventCondition.CanCalculateProgress(currentProgress, goal) && eventCondition.Type == EEventConditionType.LoungeQuestCleared && LoungeQuestId == eventCondition.Value1)
		{
			return 1L;
		}
		return currentProgress;
	}
}
