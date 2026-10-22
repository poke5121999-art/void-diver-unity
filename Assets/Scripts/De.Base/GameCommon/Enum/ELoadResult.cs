namespace GameCommon.Enum
{
	[LocalizableEnum(new string[] { })]
	public enum ELoadResult
	{
		Success = 0,
		NotFound = 1,
		Corrupted = 2,
		Failed = 3,
		UserMismatch = 4,
		VersionMismatch = 5
	}
}
