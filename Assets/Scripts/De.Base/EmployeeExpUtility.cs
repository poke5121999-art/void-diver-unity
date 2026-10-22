using System;
using System.Linq;
using GameCommon.Table;
using UnityEngine.UI;

public static class EmployeeExpUtility
{
	public static void Apply(Slider slider, int level, long exp)
	{
		float value = 1f;
		if (level < TEmployeeLevel.Table.Keys.Max())
		{
			long needExp = TEmployeeLevel.Table[level + 1].NeedExp;
			if (needExp >= 1)
			{
				value = Math.Min((float)exp / (float)needExp, 1f);
			}
		}
		slider.value = value;
	}
}
