using GameCommon.Enum;
using GameCommon.Data;

public class TalentEvent : IBaseEvent
{
	public int TalentId { get; private set; }

	public TalentEvent(int talentId)
	{
		TalentId = talentId;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		if (eventCondition.Type == EEventConditionType.Talent && TalentId == eventCondition.Value1)
		{
			currentProgress = 1L;
		}
		return currentProgress;
	}
}
