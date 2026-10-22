using GameCommon.Data;

public class GainWorthEvent : IBaseEvent
{
	public int GainWorth { get; private set; }

	public int TeamTotalWorth { get; private set; }

	public GainWorthEvent(int gainWorth, int teamTotalWorth)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
