using GameCommon.Data;

public class ArtifactDealFinishEvent : IBaseEvent
{
	public int ArtifactDealId { get; private set; }

	public int EquipmentId { get; private set; }

	public ArtifactDealFinishEvent(int artifactDealId, int equipmentId)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
