using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;

public static class BagSlotExtensions
{
	public static bool TryPushIntoBag(this IDictionary<int, GoodsSlot> bagHolderSlots, int bagHolderSlotIndex, Goods goods, long itemMaxStackCount, int innerSlotIndex = -1)
	{
		Goods swappedOutGoods;
		return bagHolderSlots.TryPushIntoBag(bagHolderSlotIndex, goods, itemMaxStackCount, innerSlotIndex, out swappedOutGoods);
	}

	public static bool TryPushIntoBag(this IDictionary<int, GoodsSlot> bagHolderSlots, int bagHolderSlotIndex, Goods goods, long itemMaxStackCount, int innerSlotIndex, out Goods swappedOutGoods)
	{
		swappedOutGoods = null;
		if (goods == null)
		{
			return false;
		}
		if (goods.Category == EGoodsCategory.Bag)
		{
			return false;
		}
		if (!bagHolderSlots.TryGetValue(bagHolderSlotIndex, out var value))
		{
			return false;
		}
		if (value.Goods == null)
		{
			return false;
		}
		if (value.Goods.Category != EGoodsCategory.Bag)
		{
			return false;
		}
		GoodsSlot goodsSlot = value.Clone();
		if (goodsSlot.Goods == null)
		{
			return false;
		}
		BagGoods bagGoods = goodsSlot.Goods as BagGoods;
		if (bagGoods == null)
		{
			return false;
		}
		if (TBag.Table[bagGoods.Id].Type != goods.PushableBagType)
		{
			return false;
		}
		GoodsSlot[] goodsSlots = bagGoods.GoodsSlots;
		if (goodsSlots == null || goodsSlots.Length == 0)
		{
			return false;
		}
		int num = FindPushTargetInnerSlot(goodsSlots, goods, itemMaxStackCount, innerSlotIndex);
		if (num < 0)
		{
			return false;
		}
		GoodsSlot goodsSlot2 = goodsSlots[num];
		bool flag = false;
		if (goods.Category == EGoodsCategory.Item)
		{
			if (goodsSlot2 != null && goodsSlot2.Goods != null)
			{
				if (goodsSlot2.Goods.Id == goods.Id)
				{
					Goods goods2 = GoodsCreator.MakeGoods(goods.Type, goods.Id, goods.BaseCount + goodsSlot2.Goods.BaseCount, goods.GainCount + goodsSlot2.Goods.GainCount);
					goodsSlots[num] = new GoodsSlot
					{
						SlotIndex = num,
						Goods = goods2
					};
					flag = true;
				}
				else
				{
					swappedOutGoods = goodsSlot2.Goods;
				}
			}
		}
		else if (goodsSlot2 != null && goodsSlot2.Goods != null)
		{
			swappedOutGoods = goodsSlot2.Goods;
		}
		if (!flag)
		{
			goodsSlots[num] = new GoodsSlot
			{
				SlotIndex = num,
				Goods = goods
			};
		}
		bagHolderSlots[bagHolderSlotIndex] = goodsSlot;
		return true;
	}

	public static bool CanPushIntoBag(this IDictionary<int, GoodsSlot> bagHolderSlots, int bagHolderSlotIndex, Goods goods, long itemMaxStackCount, int innerSlotIndex = -1)
	{
		if (goods != null && goods.Category != EGoodsCategory.Bag && bagHolderSlots.TryGetValue(bagHolderSlotIndex, out var value) && value.Goods != null && value.Goods.Category == EGoodsCategory.Bag)
		{
			BagGoods bagGoods = value.Goods as BagGoods;
			if (bagGoods != null)
			{
				if (TBag.Table[bagGoods.Id].Type == goods.PushableBagType)
				{
					GoodsSlot[] goodsSlots = bagGoods.GoodsSlots;
					if (goodsSlots != null && goodsSlots.Length != 0)
					{
						return FindPushTargetInnerSlot(goodsSlots, goods, itemMaxStackCount, innerSlotIndex) >= 0;
					}
				}
			}
		}
		return false;
	}

	private static int FindPushTargetInnerSlot(GoodsSlot[] innerSlots, Goods goods, long itemMaxStackCount, int innerSlotIndex)
	{
		EGoodsCategory category = goods.Category;
		if (category == EGoodsCategory.Item)
		{
			int id = goods.Id;
			long count = goods.Count;
			for (int i = 0; i < innerSlots.Length; i++)
			{
				GoodsSlot goodsSlot = innerSlots[i];
				if (goodsSlot != null && goodsSlot.Goods != null && goodsSlot.Goods.Id == id && goodsSlot.Goods.Count + count <= itemMaxStackCount)
				{
					return i;
				}
			}
		}
		if (innerSlotIndex < 0)
		{
			for (int j = 0; j < innerSlots.Length; j++)
			{
				if (innerSlots[j] == null || innerSlots[j].Goods == null)
				{
					return j;
				}
			}
			return -1;
		}
		if (innerSlotIndex < innerSlots.Length)
		{
			GoodsSlot goodsSlot2 = innerSlots[innerSlotIndex];
			if (category != EGoodsCategory.Item || goodsSlot2 == null || goodsSlot2.Goods == null || goodsSlot2.Goods.Id != goods.Id || goodsSlot2.Goods.Count + goods.Count <= itemMaxStackCount)
			{
				return innerSlotIndex;
			}
		}
		return -1;
	}

	public static bool TryPullFromBag(this IDictionary<int, GoodsSlot> bagHolderSlots, int bagHolderSlotIndex, int innerSlotIndex, long count, out Goods pullGoods)
	{
		pullGoods = null;
		if (count > 0 && bagHolderSlots.TryGetValue(bagHolderSlotIndex, out var value) && value.Goods != null && value.Goods.Category == EGoodsCategory.Bag)
		{
			GoodsSlot goodsSlot = value.Clone();
			if (goodsSlot.Goods != null)
			{
				BagGoods bagGoods = goodsSlot.Goods as BagGoods;
				if (bagGoods != null && bagGoods.GoodsSlots != null && innerSlotIndex >= 0 && innerSlotIndex < bagGoods.GoodsSlots.Length)
				{
					GoodsSlot[] goodsSlots = bagGoods.GoodsSlots;
					GoodsSlot goodsSlot2 = goodsSlots[innerSlotIndex];
					if (goodsSlot2 == null || goodsSlot2.Goods == null)
					{
						return false;
					}
					Goods goods = goodsSlot2.Goods;
					long num = Math.Min(count, goods.Count);
					if (num < goods.Count)
					{
						long num2 = Math.Min(num, goods.BaseCount);
						pullGoods = GoodsCreator.MakeGoods(goods.Type, goods.Id, num2, num - num2);
						Goods goods2 = GoodsCreator.MakeGoods(goods.Type, goods.Id, goods.BaseCount - num2, goods.GainCount - (num - num2));
						goodsSlots[innerSlotIndex] = new GoodsSlot
						{
							SlotIndex = innerSlotIndex,
							Goods = goods2
						};
					}
					else
					{
						pullGoods = goods;
						goodsSlots[innerSlotIndex] = new GoodsSlot
						{
							SlotIndex = innerSlotIndex
						};
					}
					bagHolderSlots[bagHolderSlotIndex] = goodsSlot;
					return true;
				}
			}
		}
		return false;
	}

	public static bool TrySwapInnerSlots(this IDictionary<int, GoodsSlot> bagHolderSlots, int bagHolderSlotIndex, int fromInnerSlotIndex, int toInnerSlotIndex, long itemMaxStackCount)
	{
		if (fromInnerSlotIndex == toInnerSlotIndex)
		{
			return false;
		}
		if (!bagHolderSlots.TryGetValue(bagHolderSlotIndex, out var value))
		{
			return false;
		}
		if (value.Goods == null)
		{
			return false;
		}
		if (value.Goods.Category != EGoodsCategory.Bag)
		{
			return false;
		}
		GoodsSlot goodsSlot = value.Clone();
		if (goodsSlot.Goods == null)
		{
			return false;
		}
		BagGoods bagGoods = goodsSlot.Goods as BagGoods;
		if (bagGoods == null)
		{
			return false;
		}
		GoodsSlot[] goodsSlots = bagGoods.GoodsSlots;
		if (goodsSlots == null)
		{
			return false;
		}
		if (fromInnerSlotIndex < 0 || fromInnerSlotIndex >= goodsSlots.Length)
		{
			return false;
		}
		if (toInnerSlotIndex < 0 || toInnerSlotIndex >= goodsSlots.Length)
		{
			return false;
		}
		GoodsSlot goodsSlot2 = goodsSlots[fromInnerSlotIndex];
		GoodsSlot goodsSlot3 = goodsSlots[toInnerSlotIndex];
		if (goodsSlot2 == null || goodsSlot2.Goods == null)
		{
			return false;
		}
		Goods goods = goodsSlot2.Goods;
		if (goodsSlot3 == null || goodsSlot3.Goods == null)
		{
			goodsSlots[toInnerSlotIndex] = new GoodsSlot
			{
				SlotIndex = toInnerSlotIndex,
				Goods = goods
			};
			goodsSlots[fromInnerSlotIndex] = new GoodsSlot
			{
				SlotIndex = fromInnerSlotIndex
			};
		}
		else
		{
			Goods goods2 = goodsSlot3.Goods;
			if (goods.Category == EGoodsCategory.Item && goods2.Category == EGoodsCategory.Item && goods.Id == goods2.Id)
			{
				long num = Math.Max(0L, itemMaxStackCount - goods2.Count);
				long num2 = Math.Min(goods.Count, num);
				if (num2 < 1)
				{
					goodsSlots[toInnerSlotIndex] = new GoodsSlot
					{
						SlotIndex = toInnerSlotIndex,
						Goods = goods
					};
					goodsSlots[fromInnerSlotIndex] = new GoodsSlot
					{
						SlotIndex = fromInnerSlotIndex,
						Goods = goods2
					};
				}
				else
				{
					long num3 = Math.Min(num2, goods.BaseCount);
					long num4 = goods.BaseCount - num3;
					long num5 = num2 - num3;
					long num6 = goods.GainCount - num5;
					Goods goods3 = GoodsCreator.MakeGoods(goods.Type, goods.Id, goods2.BaseCount + num3, num5 + goods2.GainCount);
					goodsSlots[toInnerSlotIndex] = new GoodsSlot
					{
						SlotIndex = toInnerSlotIndex,
						Goods = goods3
					};
					if (num6 + num4 < 1)
					{
						goodsSlots[fromInnerSlotIndex] = new GoodsSlot
						{
							SlotIndex = fromInnerSlotIndex
						};
					}
					else
					{
						Goods goods4 = GoodsCreator.MakeGoods(goods.Type, goods.Id, num4, num6);
						goodsSlots[fromInnerSlotIndex] = new GoodsSlot
						{
							SlotIndex = fromInnerSlotIndex,
							Goods = goods4
						};
					}
				}
			}
			else
			{
				goodsSlots[toInnerSlotIndex] = new GoodsSlot
				{
					SlotIndex = toInnerSlotIndex,
					Goods = goods
				};
				goodsSlots[fromInnerSlotIndex] = new GoodsSlot
				{
					SlotIndex = fromInnerSlotIndex,
					Goods = goods2
				};
			}
		}
		bagHolderSlots[bagHolderSlotIndex] = goodsSlot;
		return true;
	}

	public static long GetAddableCountInBagStacks(this IDictionary<int, GoodsSlot> bagHolderSlots, int goodsId, long itemMaxStackCount)
	{
		long num = 0L;
		foreach (GoodsSlot value in bagHolderSlots.Values)
		{
			if (value != null && value.Goods != null)
			{
				BagGoods bagGoods = value.Goods as BagGoods;
				if (bagGoods != null && bagGoods.GoodsSlots != null)
				{
					GoodsSlot[] goodsSlots = bagGoods.GoodsSlots;
					for (int i = 0; i < goodsSlots.Length; i++)
					{
						GoodsSlot goodsSlot = goodsSlots[i];
						if (goodsSlot != null && goodsSlot.Goods != null && goodsSlot.Goods.Id == goodsId)
						{
							num += Math.Max(0L, itemMaxStackCount - goodsSlot.Goods.Count);
						}
					}
				}
			}
		}
		return num;
	}

	public static Goods StackIntoExistingBagSlots(this IDictionary<int, GoodsSlot> bagHolderSlots, Goods goods, long itemMaxStackCount)
	{
		if (goods == null || goods.Category != EGoodsCategory.Item)
		{
			return goods;
		}
		long num = goods.BaseCount;
		long num2 = goods.GainCount;
		foreach (int item in bagHolderSlots.Keys.ToList())
		{
			if (num2 + num < 1)
			{
				break;
			}
			if (!bagHolderSlots.TryGetValue(item, out var value) || value == null || value.Goods == null || !(value.Goods is BagGoods))
			{
				continue;
			}
			GoodsSlot goodsSlot = value.Clone();
			if (goodsSlot.Goods == null)
			{
				continue;
			}
			BagGoods bagGoods = goodsSlot.Goods as BagGoods;
			if (bagGoods == null || bagGoods.GoodsSlots == null)
			{
				continue;
			}
			GoodsSlot[] goodsSlots = bagGoods.GoodsSlots;
			bool flag = false;
			for (int i = 0; i < goodsSlots.Length; i++)
			{
				if (num2 + num < 1)
				{
					break;
				}
				GoodsSlot goodsSlot2 = goodsSlots[i];
				if (goodsSlot2 == null || goodsSlot2.Goods == null || goodsSlot2.Goods.Id != goods.Id)
				{
					continue;
				}
				long num3 = Math.Max(0L, itemMaxStackCount - goodsSlot2.Goods.Count);
				if (num3 > 0)
				{
					long num4 = Math.Min(num3, num);
					long num5 = Math.Min(num3 - num4, num2);
					if (num5 + num4 > 0)
					{
						Goods goods2 = GoodsCreator.MakeGoods(goods.Type, goods.Id, goodsSlot2.Goods.BaseCount + num4, goodsSlot2.Goods.GainCount + num5);
						goodsSlots[i] = new GoodsSlot
						{
							SlotIndex = i,
							Goods = goods2
						};
						num -= num4;
						num2 -= num5;
						flag = true;
					}
				}
			}
			if (flag)
			{
				bagHolderSlots[item] = goodsSlot;
			}
		}
		if (num2 + num < 1)
		{
			return null;
		}
		return GoodsCreator.MakeGoods(goods.Type, goods.Id, num, num2);
	}

	private static int FindFirstEmptyInnerSlot(GoodsSlot[] innerSlots)
	{
		for (int i = 0; i < innerSlots.Length; i++)
		{
			if (innerSlots[i] == null || innerSlots[i].Goods == null)
			{
				return i;
			}
		}
		return -1;
	}

	private static int FindStackableInnerSlot(GoodsSlot[] innerSlots, int goodsId, long incomingCount, long itemMaxStackCount)
	{
		for (int i = 0; i < innerSlots.Length; i++)
		{
			GoodsSlot goodsSlot = innerSlots[i];
			if (goodsSlot != null && goodsSlot.Goods != null && goodsSlot.Goods.Id == goodsId && goodsSlot.Goods.Count + incomingCount <= itemMaxStackCount)
			{
				return i;
			}
		}
		return -1;
	}
}
