using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public struct GoodsData
	{
		public EGoodsType Type;

		public int Id;

		public long Count;

		public bool IsEmpty => false;

		public EGoodsCategory Category => EGoodsCategory.None;

		public static GoodsData Empty()
		{
			return default;
		}

		public override string ToString()
		{
			return null;
		}

		public static bool TryParse(string str, out GoodsData goodsData)
		{
			goodsData = default;
			return false;
		}

		public static bool operator ==(GoodsData left, GoodsData right)
		{
			return false;
		}

		public static bool operator !=(GoodsData left, GoodsData right)
		{
			return false;
		}

		public override bool Equals(object obj)
		{
			return false;
		}

		public bool Equals(GoodsData other)
		{
			return false;
		}

		public override int GetHashCode()
		{
			return 0;
		}
	}
}
