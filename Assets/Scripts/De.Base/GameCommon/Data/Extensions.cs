using System;
using GameCommon.Enum;
using GameCommon.Table;

namespace GameCommon.Data
{
	public static class Extensions
	{
		public static string ToDescription(this EventCondition eventCondition)
		{
			return null;
		}

		public static string ToDescription(this EventCondition[] unlockConditions)
		{
			return null;
		}

		public static bool CanCalculateProgress(this EventCondition eventCondition, long currentProgress, long goal)
		{
			return false;
		}

		public static Goods ToGoodsAsBase(this GoodsData data)
		{
			return null;
		}

		public static Goods ToGoodsAsGain(this GoodsData data)
		{
			return null;
		}

		public static string GetName(this GoodsData data)
		{
			return null;
		}

		public static EGoodsCategory GoodsCategory(this EGoodsType goodsType)
		{
			switch (goodsType)
			{
			case EGoodsType.None:
				return EGoodsCategory.None;
			case EGoodsType.Gold:
			case EGoodsType.Coin:
			case EGoodsType.Exp:
				return EGoodsCategory.Currency;
			case EGoodsType.Consumable:
			case EGoodsType.Valuable:
			case EGoodsType.Misc:
			case EGoodsType.Note:
			case EGoodsType.Blueprint:
			case EGoodsType.MusicDisc:
				return EGoodsCategory.Item;
			case EGoodsType.Weapon:
			case EGoodsType.Accessory:
			case EGoodsType.Artifact:
				return EGoodsCategory.Equipment;
			case EGoodsType.Bag:
				return EGoodsCategory.Bag;
			default:
				throw new InvalidOperationException($"Unknown EGoodsType: {goodsType}");
			}
		}

		public static EGoodsDataType ToEGoodsDataType(this EGoodsType goodsType)
		{
			switch (goodsType.GoodsCategory())
			{
			case EGoodsCategory.Currency:
				return (EGoodsDataType)(int)goodsType;
			case EGoodsCategory.Item:
				return EGoodsDataType.Item;
			case EGoodsCategory.Equipment:
				return EGoodsDataType.Equipment;
			case EGoodsCategory.Bag:
				return EGoodsDataType.Bag;
			default:
				return EGoodsDataType.None;
			}
		}

		public static EGoodsType ToEGoodsType(this EGoodsDataType dataType, int id)
		{
			if (!dataType.TryToEGoodsType(id, out EGoodsType goodsType))
			{
				throw new InvalidOperationException($"unknown goods {dataType}:{id}");
			}
			return goodsType;
		}

		// Item and Equipment carry their EGoodsType in the table row, so an id missing from it fails.
		public static bool TryToEGoodsType(this EGoodsDataType dataType, int id, out EGoodsType goodsType)
		{
			goodsType = EGoodsType.None;
			switch (dataType)
			{
			case EGoodsDataType.None:
				return true;
			case EGoodsDataType.Gold:
				goodsType = EGoodsType.Gold;
				return true;
			case EGoodsDataType.Coin:
				goodsType = EGoodsType.Coin;
				return true;
			case EGoodsDataType.Exp:
				goodsType = EGoodsType.Exp;
				return true;
			case EGoodsDataType.Bag:
				goodsType = EGoodsType.Bag;
				return true;
			case EGoodsDataType.Item:
				if (TItem.Table != null && TItem.Table.TryGetValue(id, out TItem item))
				{
					goodsType = item.GoodsType;
					return true;
				}
				return false;
			case EGoodsDataType.Equipment:
				if (TEquipment.Table != null && TEquipment.Table.TryGetValue(id, out TEquipment equipment))
				{
					goodsType = equipment.GoodsType;
					return true;
				}
				return false;
			default:
				return false;
			}
		}

		public static bool IsEventItem(this GoodsData goodsData)
		{
			return false;
		}

		public static EGoodsGradeType GetGrade(this GoodsData goodsData)
		{
			return EGoodsGradeType.None;
		}
	}
}
