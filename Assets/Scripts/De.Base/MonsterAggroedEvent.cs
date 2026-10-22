using GameCommon.Data;

public class MonsterAggroedEvent : IBaseEvent
{
	public ulong TargetClientId { get; private set; }

	public int MonsterId { get; private set; }

	public MonsterAggroedEvent(ulong targetClientId, int monsterId)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
