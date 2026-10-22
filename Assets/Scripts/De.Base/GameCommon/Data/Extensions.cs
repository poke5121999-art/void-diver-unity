using GameCommon.Enum;

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
			return EGoodsCategory.None;
		}

		public static EGoodsDataType ToEGoodsDataType(this EGoodsType goodsType)
		{
			return EGoodsDataType.None;
		}

		public static EGoodsType ToEGoodsType(this EGoodsDataType dataType, int id)
		{
			return EGoodsType.None;
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
