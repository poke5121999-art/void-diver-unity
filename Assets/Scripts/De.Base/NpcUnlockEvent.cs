using GameCommon.Data;

public class NpcUnlockEvent : IBaseEvent
{
	public int[] NpcIds { get; private set; }

	public NpcUnlockEvent(int[] npcIds)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
