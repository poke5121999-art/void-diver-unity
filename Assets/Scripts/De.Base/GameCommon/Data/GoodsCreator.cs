using GameCommon.Table;
using System.Linq;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	public static class GoodsCreator
	{
		public static Goods MakeItemGoods(int id, long baseCount = 0L, long gainCount = 0L)
		{
			TItem tItem = TItem.Table[id];
			switch (tItem.GoodsType)
			{
			case EGoodsType.Consumable:
				return new ConsumableGoods(id, baseCount, gainCount);
			case EGoodsType.Valuable:
				return new ValuableGoods(id, baseCount, gainCount);
			case EGoodsType.Misc:
				return new MiscGoods(id, baseCount, gainCount);
			case EGoodsType.Note:
				return new NoteGoods(id, baseCount, gainCount);
			case EGoodsType.Blueprint:
				return new BlueprintGoods(id, baseCount, gainCount);
			case EGoodsType.MusicDisc:
				return new MusicDiscGoods(id, baseCount, gainCount);
			default:
				throw new InvalidOperationException(string.Format("Invalid ItemGoods for id={0}, GoodsType={1}", id, tItem.GoodsType));
			}
		}

		public static Goods MakeEquipmentGoods(int id, long baseCount = 0L, long gainCount = 0L, int[] prefixes = null, int? maxDurability = null, float? currentDurability = null, byte campaignLevel = 1)
		{
			TEquipment tEquipment = TEquipment.Table[id];
			switch (tEquipment.GoodsType)
			{
			case EGoodsType.Weapon:
				return MakeWeapon(id, baseCount, gainCount, maxDurability, currentDurability);
			case EGoodsType.Accessory:
				return MakeAccessory(id, baseCount, gainCount, maxDurability, currentDurability);
			case EGoodsType.Artifact:
				return MakeArtifact(id, baseCount, gainCount, prefixes, campaignLevel);
			default:
				throw new InvalidOperationException(string.Format("Invalid EquipmentGoods for id={0}, GoodsType={1}", id, tEquipment.GoodsType));
			}
		}

		private static ArtifactGoods MakeArtifact(int id, long baseCount, long gainCount, int[] prefixes, byte campaignLevel)
		{
			if (prefixes == null)
			{
				prefixes = TEquipment.Table[id].PickPrefixes();
			}
			return new ArtifactGoods(id, baseCount, gainCount, prefixes, campaignLevel);
		}

		private static WeaponGoods MakeWeapon(int id, long baseCount, long gainCount, int? maxDurability, float? currentDurability)
		{
			int maxDurability2 = maxDurability ?? TEquipment.Table[id].GetMaxDurability();
			float currentDurability2 = currentDurability ?? (float)maxDurability2;
			return new WeaponGoods(id, baseCount, gainCount, maxDurability2, currentDurability2);
		}

		private static AccessoryGoods MakeAccessory(int id, long baseCount, long gainCount, int? maxDurability, float? currentDurability)
		{
			int maxDurability2 = maxDurability ?? TEquipment.Table[id].GetMaxDurability();
			float currentDurability2 = currentDurability ?? (float)maxDurability2;
			return new AccessoryGoods(id, baseCount, gainCount, maxDurability2, currentDurability2);
		}

		public static BagGoods MakeBagGoods(int id, long baseCount = 0L, long gainCount = 0L, GoodsSlot[] goodsSlots = null)
		{
			if (goodsSlots == null)
			{
				goodsSlots = Enumerable.Range(0, TBag.Table[id].SlotCount).Select((int i) => new GoodsSlot
				{
					SlotIndex = i
				}).ToArray();
			}
			return new BagGoods(id, baseCount, gainCount, goodsSlots);
		}

		private static Goods MakeCurrencyGoods(EGoodsType type, long baseCount, long gainCount)
		{
			switch (type)
			{
			case EGoodsType.Gold:
				return new GoldGoods(baseCount, gainCount);
			case EGoodsType.Coin:
				return new CoinGoods(baseCount, gainCount);
			case EGoodsType.Exp:
				return new ExpGoods(baseCount, gainCount);
			default:
				throw new InvalidOperationException(string.Format("Invalid CurrencyGoods type: {0}", type));
			}
		}

		public static Goods MakeGoods(EGoodsType type, int id = 0, long baseCount = 0L, long gainCount = 0L)
		{
			switch (type.GoodsCategory())
			{
			case EGoodsCategory.None:
				return null;
			case EGoodsCategory.Currency:
				return MakeCurrencyGoods(type, baseCount, gainCount);
			case EGoodsCategory.Item:
				return MakeItemGoods(id, baseCount, gainCount);
			case EGoodsCategory.Equipment:
				return MakeEquipmentGoods(id, baseCount, gainCount);
			case EGoodsCategory.Bag:
				return MakeBagGoods(id, baseCount, gainCount);
			default:
				throw new InvalidOperationException(string.Format("Unknown EGoodsCategory for {0}", type));
			}
		}

		public static Goods MakeFromGoodsData(GoodsData data, long baseCount, long gainCount)
		{
			return MakeGoods(data.Type, data.Id, baseCount, gainCount);
		}
	}
}
