using GameCommon.Enum;

namespace GameCommon.Data
{
	public static class GoodsCreator
	{
		public static Goods MakeItemGoods(int id, long baseCount = 0L, long gainCount = 0L)
		{
			return null;
		}

		public static Goods MakeEquipmentGoods(int id, long baseCount = 0L, long gainCount = 0L, int[] prefixes = null, int? maxDurability = null, float? currentDurability = null, byte campaignLevel = 1)
		{
			return null;
		}

		private static ArtifactGoods MakeArtifact(int id, long baseCount, long gainCount, int[] prefixes, byte campaignLevel)
		{
			return null;
		}

		private static WeaponGoods MakeWeapon(int id, long baseCount, long gainCount, int? maxDurability, float? currentDurability)
		{
			return null;
		}

		private static AccessoryGoods MakeAccessory(int id, long baseCount, long gainCount, int? maxDurability, float? currentDurability)
		{
			return null;
		}

		public static BagGoods MakeBagGoods(int id, long baseCount = 0L, long gainCount = 0L, GoodsSlot[] goodsSlots = null)
		{
			return null;
		}

		private static Goods MakeCurrencyGoods(EGoodsType type, long baseCount, long gainCount)
		{
			return null;
		}

		public static Goods MakeGoods(EGoodsType type, int id = 0, long baseCount = 0L, long gainCount = 0L)
		{
			return null;
		}

		public static Goods MakeFromGoodsData(GoodsData data, long baseCount, long gainCount)
		{
			return null;
		}
	}
}
