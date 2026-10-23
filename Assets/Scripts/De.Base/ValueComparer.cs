using System;
using GameCommon.Enum;

public static class ValueComparer
{
	// TODO(decomp): generic method has no pseudo-C in the binary; semantics taken from ECompareType names
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
			return false;
		}
	}
}
