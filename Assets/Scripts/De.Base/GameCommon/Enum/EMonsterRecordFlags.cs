using System;

namespace GameCommon.Enum
{
	[Flags]
	public enum EMonsterRecordFlags : byte
	{
		None = 0,
		BaseInfo = 1,
		Weakness = 2,
		Description = 4
	}
}
