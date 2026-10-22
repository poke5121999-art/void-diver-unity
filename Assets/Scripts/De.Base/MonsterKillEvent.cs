using GameCommon.Data;

public class MonsterKillEvent : IBaseEvent
{
	public ulong SourceClientId { get; private set; }

	public int MonsterId { get; private set; }

	public MonsterKillEvent(ulong sourceClientId, int monsterId)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
