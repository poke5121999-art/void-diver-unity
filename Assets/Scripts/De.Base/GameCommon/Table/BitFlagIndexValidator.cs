using System;
using System.Collections.Generic;

namespace GameCommon.Table
{
	public static class BitFlagIndexValidator
	{
		private const string FieldName = "BitFlagIndex";

		public static bool HasBitFlagIndex(Type dataType)
		{
			return false;
		}

		public static bool Validate(Type dataType, out string error)
		{
			error = null;
			return false;
		}

		private static string FormatRanges(IReadOnlyList<int> sortedNumbers)
		{
			return null;
		}
	}
}
