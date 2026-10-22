using GameCommon.Data;

public class LuaProgressEvent : IBaseEvent
{
	public int Key { get; private set; }

	public int Progress { get; private set; }

	public LuaProgressEvent(int key, int progress)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
