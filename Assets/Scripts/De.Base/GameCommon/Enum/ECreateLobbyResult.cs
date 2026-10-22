namespace GameCommon.Enum
{
	[LocalizableEnum(new string[] { })]
	public enum ECreateLobbyResult
	{
		Success = 0,
		NotInitialized = 1,
		AlreadyJoined = 2,
		Timeout = 3,
		Failed = 4
	}
}
