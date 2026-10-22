using GameCommon.Data;

public class LocalPlayerAttackEvent : IBaseEvent
{
	public int MonsterId { get; private set; }

	public bool IsPenalty { get; private set; }

	public LocalPlayerAttackEvent(int monsterId, bool isPenalty)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
