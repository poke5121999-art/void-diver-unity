using GameCommon.Data;

public class AppCheckCheatEvent : IBaseEvent
{
	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return currentProgress;
	}
}
