using System.Collections.Generic;
using GameCommon.Enum;

public class ActionStatsData : NativeBase
{
	public readonly Dictionary<EStatType, float> StatDic;

	public ActionStatsData(IActionStats actionStat)
	{
	}

	public float GetStat(EStatType statType)
	{
		return 0f;
	}
}
