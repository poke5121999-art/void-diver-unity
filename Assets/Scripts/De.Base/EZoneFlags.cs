using System;

[Flags]
public enum EZoneFlags
{
	None = 0,
	Inside = 1,
	Outside = 2,
	PlayerSpawn = 4,
	Custom1 = 0x400,
	Custom2 = 0x800,
	Custom3 = 0x1000,
	Custom4 = 0x2000,
	Custom5 = 0x4000,
	Portal1 = 0x100000,
	Portal2 = 0x200000,
	Portal3 = 0x400000
}
