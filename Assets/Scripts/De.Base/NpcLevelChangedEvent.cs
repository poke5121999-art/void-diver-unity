using GameCommon.Enum;
using GameCommon.Data;

public class NpcLevelChangedEvent : IBaseEvent
{
	public int NpcId { get; private set; }

	public int Level { get; private set; }

	public NpcLevelChangedEvent(int npcId, int level)
	{
		NpcId = npcId;
		Level = level;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		if (eventCondition.CanCalculateProgress(currentProgress, goal) && eventCondition.Type == EEventConditionType.NpcLevel && (int)eventCondition.Value1 == NpcId && (int)eventCondition.Value2 <= Level)
		{
			return goal;
		}
		return currentProgress;
	}
}
