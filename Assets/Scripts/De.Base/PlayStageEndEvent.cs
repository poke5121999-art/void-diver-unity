using GameCommon.Data;

public class PlayStageEndEvent : IBaseEvent
{
	public int CampaignId { get; }

	public int Round { get; }

	public int CharacterId { get; }

	public bool IsEscaped { get; }

	public bool IsMissionClear { get; }

	public bool IsAborted { get; }

	public int PlayerCount { get; }

	public int? AnomalyId { get; }

	public int? ArtifactCount { get; }

	public int? GetSoftCurrency { get; }

	public int? GetHardCurrency { get; }

	public PlayStageEndEvent(int campaignId, int round, int characterId, bool isEscaped = false, bool isMissionClear = false, bool isAborted = false, int playerCount = 1, int? anomalyId = null, int? artifactCount = null, int? getSoftCurrency = null, int? getHardCurrency = null)
	{
		CampaignId = campaignId;
		Round = round;
		CharacterId = characterId;
		IsEscaped = isEscaped;
		IsMissionClear = isMissionClear;
		IsAborted = isAborted;
		PlayerCount = playerCount;
		AnomalyId = anomalyId;
		ArtifactCount = artifactCount;
		GetSoftCurrency = getSoftCurrency;
		GetHardCurrency = getHardCurrency;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return currentProgress;
	}
}
