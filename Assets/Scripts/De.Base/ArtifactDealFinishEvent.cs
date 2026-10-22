using System;
using GameCommon.Data;

public class ArtifactDealFinishEvent : IBaseEvent
{
	public int ArtifactDealId { get; private set; }

	public int EquipmentId { get; private set; }

	public ArtifactDealFinishEvent(int artifactDealId, int equipmentId)
	{
		ArtifactDealId = artifactDealId;
		EquipmentId = equipmentId;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		if (eventCondition.CanCalculateProgress(currentProgress, goal) && eventCondition.Type == GameCommon.Enum.EEventConditionType.ArtifactDealFinish)
		{
			bool flag = eventCondition.Value1 < 1 || ArtifactDealId == eventCondition.Value1;
			bool flag2 = true;
			if (eventCondition.Value2 > 0)
			{
				flag2 = EquipmentId == eventCondition.Value2;
			}
			if (flag && flag2)
			{
				return Math.Min(currentProgress + 1, goal);
			}
		}
		return currentProgress;
	}
}
