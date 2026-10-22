using GameCommon.Data;

public class TalentEvent : IBaseEvent
{
	public int TalentId { get; private set; }

	public TalentEvent(int talentId)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
