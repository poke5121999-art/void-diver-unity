namespace GameCommon.Enum
{
	public enum ELuaEvent
	{
		None = 0,
		NpcInteraction = 1,
		TriggerInteraction = 2,
		CollisionTriggerEnter = 101,
		MonsterKill = 201,
		WaveStarted = 202,
		WaveFailed = 203,
		WaveCleared = 204,
		ArtifactDeal = 205,
		CampaignStateChanged = 221,
		CampaignTaskAchieved = 222,
		MissionStateChanged = 231,
		MissionTaskAchieved = 232,
		LoungeQuestStateChanged = 241,
		Custom = 999
	}
}
