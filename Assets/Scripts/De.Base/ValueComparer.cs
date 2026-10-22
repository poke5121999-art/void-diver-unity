using System;
using GameCommon.Enum;

public static class ValueComparer
{
	public static bool Compare<T>(T a, ECompareType compareType, T b) where T : struct, IComparable<T>
	{
		return false;
	}
}
