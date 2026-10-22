using System.Linq;
using GameCommon.Enum;

public static class BuffStackEntryExtension
{
	public static float GetCasterStatSum(this BuffStackEntry[] entries, EStatType type)
	{
		return entries.Sum((BuffStackEntry e) => e.CasterStats?.FirstOrDefault((UnitStat s) => s.StatType == type)?.Value ?? 0f);
	}
}
