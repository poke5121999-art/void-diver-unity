using GameCommon.Data;

public class GainGoldEvent : IBaseEvent
{
	public EGoldGainType GainType { get; private set; }

	public string GainTypeValue { get; private set; }

	public long Amount { get; private set; }

	public GainGoldEvent(EGoldGainType gainType, string gainTypeValue, long amount)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
