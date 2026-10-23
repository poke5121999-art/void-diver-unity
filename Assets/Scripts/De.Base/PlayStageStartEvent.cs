using GameCommon.Data;

public class PlayStageStartEvent : IBaseEvent
{
	public int CampaignId { get; }

	public int Round { get; }

	public int CharacterId { get; }

	public int PlayerCount { get; }

	public int? AnomalyId { get; }

	public string UseSkill { get; }

	public string UseEquipment { get; }

	public PlayStageStartEvent(int campaignId, int round, int characterId, int playerCount = 1, int? anomalyId = null, string useSkill = null, string useEquipment = null)
	{
		CampaignId = campaignId;
		Round = round;
		CharacterId = characterId;
		PlayerCount = playerCount;
		AnomalyId = anomalyId;
		UseSkill = useSkill;
		UseEquipment = useEquipment;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return currentProgress;
	}
}
