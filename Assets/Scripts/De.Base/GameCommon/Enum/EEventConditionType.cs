namespace GameCommon.Enum
{
	[LocalizableEnum(new string[] { })]
	public enum EEventConditionType
	{
		None = 0,
		MonsterKill = 1001,
		WaveClear = 1002,
		OpenRewardBox = 1003,
		StageEscape = 1004,
		CampaignClear = 1005,
		MissionClear = 1006,
		ItemUse = 1007,
		ArtifactDealFinish = 1008,
		EmployeeHire = 1009,
		EmployeeFire = 1010,
		GainedCoin = 1011,
		AccountRecord = 1012,
		LuaProgress = 2001,
		MyItemCountInInventory = 2002,
		TeamItemCountInInventory = 2003,
		GainedWorth = 2004,
		TeamTotalWorth = 2005,
		MyItemCountTotal = 2006,
		UserLevel = 3001,
		CampaignCleared = 3002,
		MissionCleared = 3003,
		LoungeQuestCleared = 3004,
		ItemCollected = 3005,
		Talent = 3006,
		ItemUsed = 3007,
		AreaUnlocked = 3008,
		AreaDecorationUnlocked = 3009,
		NpcLevel = 3010
	}
}
