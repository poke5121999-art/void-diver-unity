using System.Collections.Generic;
using GameCommon.Enum;

public class ActionStatsData : NativeBase
{
	public readonly Dictionary<EStatType, float> StatDic;

	public ActionStatsData(IActionStats actionStat)
	{
		StatDic = new Dictionary<EStatType, float>();
		for (byte i = 1; i <= 37; i++)
		{
			StatDic[(EStatType)i] = actionStat.GetStat((EStatType)i);
		}
	}

	public float GetStat(EStatType statType)
	{
		if (!StatDic.TryGetValue(statType, out var value))
		{
			return 0f;
		}
		return value;
	}
}
