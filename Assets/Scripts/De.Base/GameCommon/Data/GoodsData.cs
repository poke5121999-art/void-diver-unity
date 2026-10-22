using System;
using System.Globalization;
using GameCommon.Enum;
using GameCommon.Table;

namespace GameCommon.Data
{
	[Serializable]
	public struct GoodsData
	{
		public EGoodsType Type;

		public int Id;

		public long Count;

		public bool IsEmpty => Type == EGoodsType.None;

		public EGoodsCategory Category => Type.GoodsCategory();

		public static GoodsData Empty()
		{
			return default;
		}

		public override string ToString()
		{
			return $"{Type.ToEGoodsDataType()}:{Id}:{Count}";
		}

		// "Item:7000:4" is Type:Id:Count; "Gold:500" is Type:Count. Item and Equipment resolve their EGoodsType from the loaded tables.
		public static bool TryParse(string str, out GoodsData goodsData)
		{
			goodsData = default;
			if (string.IsNullOrEmpty(str))
			{
				return false;
			}
			string[] parts = str.Split(':');
			if (!System.Enum.TryParse(parts[0], out EGoodsDataType dataType))
			{
				return false;
			}
			int id = 0;
			long count;
			if (parts.Length == 2)
			{
				if (!long.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out count))
				{
					return false;
				}
			}
			else if (parts.Length == 3)
			{
				if (!int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out id) || !long.TryParse(parts[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out count))
				{
					return false;
				}
			}
			else
			{
				return false;
			}
			if (!dataType.TryToEGoodsType(id, out EGoodsType goodsType))
			{
				return false;
			}
			goodsData = new GoodsData
			{
				Type = goodsType,
				Id = id,
				Count = count,
			};
			return true;
		}

		public static bool operator ==(GoodsData left, GoodsData right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(GoodsData left, GoodsData right)
		{
			return !left.Equals(right);
		}

		public override bool Equals(object obj)
		{
			return obj is GoodsData other && Equals(other);
		}

		public bool Equals(GoodsData other)
		{
			return Type == other.Type && Id == other.Id && Count == other.Count;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Type, Id, Count);
		}
	}
}
