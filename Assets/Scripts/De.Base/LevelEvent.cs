using GameCommon.Enum;
using GameCommon.Data;

public class LevelEvent : IBaseEvent
{
	public int Level { get; private set; }

	public LevelEvent(int level)
	{
		Level = level;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		if (eventCondition.Type == EEventConditionType.UserLevel && eventCondition.Value1 <= Level)
		{
			currentProgress = 1L;
		}
		return currentProgress;
	}
}
