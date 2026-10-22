using GameCommon.Data;

public class ConsumeGoldEvent : IBaseEvent
{
	public EGoldConsumeType ConsumeType { get; private set; }

	public string ConsumeTypeValue { get; private set; }

	public long Amount { get; private set; }

	public ConsumeGoldEvent(EGoldConsumeType consumeType, string consumeTypeValue, long amount)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
