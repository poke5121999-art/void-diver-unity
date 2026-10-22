using GameCommon.Data;

public class OpenRewardBoxEvent : IBaseEvent
{
	public ulong SourceClientId { get; private set; }

	public int RewardBoxId { get; private set; }

	public OpenRewardBoxEvent(ulong sourceClientId, int rewardBoxId)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
