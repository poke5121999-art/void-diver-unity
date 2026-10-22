using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;

public static class InventoryExtensions
{
	public static int AllocateIndex(this IDictionary<int, GoodsSlot> goodsSlots)
	{
		return 0;
	}

	public static bool TryPushGoods(this IDictionary<int, GoodsSlot> goodsSlots, Goods goods, int slotIndex)
	{
		return false;
	}

	public static bool TryPullGoods(this IDictionary<int, GoodsSlot> goodsSlots, Goods goods, out Goods[] pullGoods)
	{
		pullGoods = null;
		return false;
	}

	public static bool TryPullSlot(this IDictionary<int, GoodsSlot> slots, int slotIndex, out Goods pullGoods)
	{
		pullGoods = null;
		return false;
	}

	public static bool TryPullSlotGoods(this IDictionary<int, GoodsSlot> slots, int slotIndex, long count, out Goods pullGoods)
	{
		pullGoods = null;
		return false;
	}

	public static void SwapIndex(this IDictionary<int, GoodsSlot> slots, int sourceIndex, int targetIndex)
	{
	}

	public static void Organize(this IDictionary<int, GoodsSlot> slots)
	{
	}

	public static GoodsSlot FindItemSlot(this IDictionary<int, GoodsSlot> slots, int itemId)
	{
		return null;
	}

	public static bool HasItem(this IDictionary<int, GoodsSlot> slots, int itemId)
	{
		return false;
	}

	public static long GetItemCount(this IDictionary<int, GoodsSlot> slots, int itemId)
	{
		return 0L;
	}

	public static long GetItemCountInBags(this IDictionary<int, GoodsSlot> slots, int itemId)
	{
		return 0L;
	}

	public static long GetEquipmentCount(this IDictionary<int, GoodsSlot> slots, int equipmentId)
	{
		return 0L;
	}

	public static long GetEquipmentCountInBags(this IDictionary<int, GoodsSlot> slots, int equipmentId)
	{
		return 0L;
	}

	public static long GetBagCount(this IDictionary<int, GoodsSlot> slots, int bagId)
	{
		return 0L;
	}

	public static long GetEmptyBagCount(this IDictionary<int, GoodsSlot> slots, int bagId)
	{
		return 0L;
	}

	public static List<Goods> ApplyStageDead(this IDictionary<int, GoodsSlot> inventorySlots, ERoomDifficulty difficulty)
	{
		return null;
	}

	public static void NormalizeCount(this IDictionary<int, GoodsSlot> slots)
	{
	}
}
