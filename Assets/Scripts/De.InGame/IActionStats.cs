using GameCommon.Enum;

public interface IActionStats
{
	EElementalType ElementalType { get; }

	float WeaponAtkMultiplier { get; }

	float GetStat(EStatType statType);
}
