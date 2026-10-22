using GameCommon.Data;
using GameCommon.Enum;
using Spine;

public static class Extensions
{
	public static void ExecuteAction(this IActionExecutable actionExecutable, ActionEvent actionEvent, HitLayerInfo hitLayerInfo)
	{
	}

	public static ActionStatsData ToStatsData(this IActionStats actionStats)
	{
		return null;
	}

	public static void UpdateCharacterSkin(this Skeleton skeleton, int skinPresetId, int weaponId, bool isSuper = false)
	{
	}

	public static void SetMultiSkin(this Skeleton skeleton, int skinId)
	{
	}

	public static void AddStack(this UnitBuff unitBuff, UnitModel owner, IActionStats actionStats, float timeNow, int stack = 1)
	{
	}

	public static void RemoveStack(this UnitBuff unitBuff, int stack = 1)
	{
	}

	public static float GetStat(this UnitBuff unitBuff, EStatType statType)
	{
		return 0f;
	}

	public static bool HasDirectPathBetween(this UnitModel unitModel, UnitModel target)
	{
		return false;
	}

	public static HitLayerInfo GetHitLayerInfo(this IHitable hitable)
	{
		return default;
	}
}
