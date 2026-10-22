using UnityEngine;
using GameCommon.Table;
using System.Linq;
using System;
using System.Collections.Generic;
using GameCommon.Enum;

namespace GameCommon.Data
{
	public static class BagConsumeRule
	{
		public static EBagConsumeFailReason Check(long required, long emptyCount, long totalCount)
		{
			if (emptyCount < required)
			{
				return (totalCount < required) ? EBagConsumeFailReason.Insufficient : EBagConsumeFailReason.RequireEmpty;
			}
			return EBagConsumeFailReason.None;
		}

		public static EBagConsumeFailReason Check(IEnumerable<Goods> consumeGoods, Func<int, long> emptyCountByBagId, Func<int, long> totalCountByBagId)
		{
			if (consumeGoods == null)
			{
				return EBagConsumeFailReason.None;
			}
			foreach (IGrouping<int, Goods> item in from g in consumeGoods
				where g != null && g.Type == EGoodsType.Bag
				group g by g.Id)
			{
				long num = item.Sum((Goods g) => g.GainCount + g.BaseCount);
				long num2 = emptyCountByBagId(item.Key);
				long num3 = totalCountByBagId(item.Key);
				if (num > num2)
				{
					return (num3 < num) ? EBagConsumeFailReason.Insufficient : EBagConsumeFailReason.RequireEmpty;
				}
			}
			return EBagConsumeFailReason.None;
		}

		public static string GetFailMessage(EBagConsumeFailReason reason)
		{
			string id;
			switch (reason)
			{
			case EBagConsumeFailReason.RequireEmpty:
				id = TLocalizedText.BagConsumeRequireEmpty.Id;
				break;
			case EBagConsumeFailReason.Insufficient:
				id = TLocalizedText.BagConsumeNotEnough.Id;
				break;
			default:
				return null;
			}
			if (!LocalizationUtility.TryGetLocalizedText(id, out var text))
			{
				Debug.LogWarning("Localized Text not found. Key: " + id);
			}
			return text;
		}
	}
}
