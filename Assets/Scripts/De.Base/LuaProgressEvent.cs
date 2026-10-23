using GameCommon.Enum;
using GameCommon.Data;

public class LuaProgressEvent : IBaseEvent
{
	public int Key { get; private set; }

	public int Progress { get; private set; }

	public LuaProgressEvent(int key, int progress)
	{
		Key = key;
		Progress = progress;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		if (eventCondition.CanCalculateProgress(currentProgress, goal) && eventCondition.Type == EEventConditionType.LuaProgress && Key == eventCondition.Value1)
		{
			return Progress;
		}
		return currentProgress;
	}
}
