using GameCommon.Data;

public class LevelEvent : IBaseEvent
{
	public int Level { get; private set; }

	public LevelEvent(int level)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
