using System;
using GameCommon.Enum;

public static class ValueComparer
{
	// ValueComparer.Compare<int> @0x180eef930 (machine code checked): jump table maps Equal..LessEqual (1..6) to ==, !=, >, >=, <, <=; anything else throws
	public static bool Compare<T>(T a, ECompareType compareType, T b) where T : struct, IComparable<T>
	{
		int order = a.CompareTo(b);
		switch (compareType)
		{
		case ECompareType.Equal:
			return order == 0;
		case ECompareType.NotEqual:
			return order != 0;
		case ECompareType.Greater:
			return order > 0;
		case ECompareType.GreaterEqual:
			return order >= 0;
		case ECompareType.Less:
			return order < 0;
		case ECompareType.LessEqual:
			return order <= 0;
		default:
			throw new ArgumentOutOfRangeException("compareType", compareType, "Invalid comparison type.");
		}
	}
}
