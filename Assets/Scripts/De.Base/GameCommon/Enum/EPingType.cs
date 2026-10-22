namespace GameCommon.Enum
{
	[LocalizableEnum(new string[] { })]
	public enum EPingType
	{
		Cancel = -1,
		None = 0,
		Ready = 1,
		ChangeRequest = 2,
		RestRequest = 3,
		WaitRequest = 4,
		Away = 5,
		Return = 6,
		ManagementContent = 7,
		ProceedQuest = 8,
		Gather = 11,
		MoveOrder = 12,
		Wait = 13,
		HelpRequest = 14,
		HpRecovery = 15,
		StressRecovery = 16,
		TargetFound = 17,
		PrepareBattle = 18,
		Notice = 21,
		Alert = 31,
		Item = 41,
		Skill = 42,
		Status = 43
	}
}
