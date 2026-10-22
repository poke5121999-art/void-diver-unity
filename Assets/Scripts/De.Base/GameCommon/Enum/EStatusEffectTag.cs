using System;

namespace GameCommon.Enum
{
	[Flags]
	[LocalizableEnum(new string[] { })]
	public enum EStatusEffectTag
	{
		None = 0,
		Bleed = 1,
		Burn = 2,
		Blind = 4,
		Root = 8,
		Light = 0x10,
		Silence = 0x20,
		Slow = 0x40,
		Weaken = 0x80,
		DefReduction = 0x100,
		Confuse = 0x200,
		Poison = 0x400,
		Madness = 0x800,
		FirePower = 0x1000,
		Stun = 0x2000000,
		Knockback = 0x4000000,
		Pull = 0x8000000,
		Freeze = 0x10000000,
		All = -1
	}
}
