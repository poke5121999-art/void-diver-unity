using GameCommon.Data;

public class PlayCampaignStartEvent : IBaseEvent
{
	public int CampaignId { get; }

	public int Round { get; }

	public int CharacterId { get; }

	public int PlayerCount { get; }

	public int? AnomalyId { get; }

	public string UseEquipment { get; }

	public PlayCampaignStartEvent(int campaignId, int round, int characterId, int playerCount = 1, int? anomalyId = null, string useEquipment = null)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
