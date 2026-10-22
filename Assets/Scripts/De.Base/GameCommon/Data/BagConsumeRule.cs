using System;
using System.Collections.Generic;
using GameCommon.Enum;

namespace GameCommon.Data
{
	public static class BagConsumeRule
	{
		public static EBagConsumeFailReason Check(long required, long emptyCount, long totalCount)
		{
			return EBagConsumeFailReason.None;
		}

		public static EBagConsumeFailReason Check(IEnumerable<Goods> consumeGoods, Func<int, long> emptyCountByBagId, Func<int, long> totalCountByBagId)
		{
			return EBagConsumeFailReason.None;
		}

		public static string GetFailMessage(EBagConsumeFailReason reason)
		{
			return null;
		}
	}
}
