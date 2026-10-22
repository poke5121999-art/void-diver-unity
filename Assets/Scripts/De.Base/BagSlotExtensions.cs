using System.Collections.Generic;
using GameCommon.Data;

public static class BagSlotExtensions
{
	public static bool TryPushIntoBag(this IDictionary<int, GoodsSlot> bagHolderSlots, int bagHolderSlotIndex, Goods goods, long itemMaxStackCount, int innerSlotIndex = -1)
	{
		return false;
	}

	public static bool TryPushIntoBag(this IDictionary<int, GoodsSlot> bagHolderSlots, int bagHolderSlotIndex, Goods goods, long itemMaxStackCount, int innerSlotIndex, out Goods swappedOutGoods)
	{
		swappedOutGoods = null;
		return false;
	}

	public static bool CanPushIntoBag(this IDictionary<int, GoodsSlot> bagHolderSlots, int bagHolderSlotIndex, Goods goods, long itemMaxStackCount, int innerSlotIndex = -1)
	{
		return false;
	}

	private static int FindPushTargetInnerSlot(GoodsSlot[] innerSlots, Goods goods, long itemMaxStackCount, int innerSlotIndex)
	{
		return 0;
	}

	public static bool TryPullFromBag(this IDictionary<int, GoodsSlot> bagHolderSlots, int bagHolderSlotIndex, int innerSlotIndex, long count, out Goods pullGoods)
	{
		pullGoods = null;
		return false;
	}

	public static bool TrySwapInnerSlots(this IDictionary<int, GoodsSlot> bagHolderSlots, int bagHolderSlotIndex, int fromInnerSlotIndex, int toInnerSlotIndex, long itemMaxStackCount)
	{
		return false;
	}

	public static long GetAddableCountInBagStacks(this IDictionary<int, GoodsSlot> bagHolderSlots, int goodsId, long itemMaxStackCount)
	{
		return 0L;
	}

	public static Goods StackIntoExistingBagSlots(this IDictionary<int, GoodsSlot> bagHolderSlots, Goods goods, long itemMaxStackCount)
	{
		return null;
	}

	private static int FindFirstEmptyInnerSlot(GoodsSlot[] innerSlots)
	{
		return 0;
	}

	private static int FindStackableInnerSlot(GoodsSlot[] innerSlots, int goodsId, long incomingCount, long itemMaxStackCount)
	{
		return 0;
	}
}
