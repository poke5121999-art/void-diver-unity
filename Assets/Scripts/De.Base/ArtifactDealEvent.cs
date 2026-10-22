using GameCommon.Data;

public class ArtifactDealEvent : IBaseEvent
{
	public ulong SourceClientId { get; private set; }

	public int ArtifactItemId { get; private set; }

	public int Price { get; private set; }

	public int NpcId { get; private set; }

	public bool IsAccepted { get; private set; }

	public ArtifactDealEvent(ulong sourceClientId, int artifactItemId, int price, int npcId, bool isAccepted)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
