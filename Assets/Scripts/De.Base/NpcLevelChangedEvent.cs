using GameCommon.Data;

public class NpcLevelChangedEvent : IBaseEvent
{
	public int NpcId { get; private set; }

	public int Level { get; private set; }

	public NpcLevelChangedEvent(int npcId, int level)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
