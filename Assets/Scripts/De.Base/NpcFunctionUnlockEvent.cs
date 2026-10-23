using GameCommon.Data;

public class NpcFunctionUnlockEvent : IBaseEvent
{
	public int[] NpcFunctionIds { get; private set; }

	public NpcFunctionUnlockEvent(int[] npcFunctionIds)
	{
		NpcFunctionIds = npcFunctionIds;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
