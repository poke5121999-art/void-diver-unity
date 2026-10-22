using GameCommon.Data;

public class GainCoinEvent : IBaseEvent
{
	public ECoinGainType GainType { get; private set; }

	public long Amount { get; private set; }

	public GainCoinEvent(ECoinGainType gainType, long amount)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
