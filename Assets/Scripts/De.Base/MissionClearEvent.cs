using GameCommon.Data;

public class MissionClearEvent : IBaseEvent
{
	public int MissionId { get; private set; }

	public MissionClearEvent(int missionId)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
