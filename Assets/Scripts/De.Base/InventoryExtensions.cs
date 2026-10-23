using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using SomeNet.Utility;

public static class InventoryExtensions
{
	public static int AllocateIndex(this IDictionary<int, GoodsSlot> goodsSlots)
	{
		return Enumerable.Range(0, goodsSlots.Count + 1).FirstOrDefault(index => !goodsSlots.ContainsKey(index) || goodsSlots[index].Goods == null);
	}

	public static bool TryPushGoods(this IDictionary<int, GoodsSlot> goodsSlots, Goods goods, int slotIndex)
	{
		int allocatedSlotIndex = 0;

		void PushItem()
		{
			GoodsSlot existing = goodsSlots.Values.FirstOrDefault(slot => slot.Goods != null && slot.Goods.Category == EGoodsCategory.Item && slot.Goods.Id == goods.Id);
			if (existing == null)
			{
				allocatedSlotIndex = goodsSlots.AllocateIndex();
				goodsSlots[allocatedSlotIndex] = new GoodsSlot
				{
					SlotIndex = allocatedSlotIndex,
					Goods = goods
				};
			}
			else
			{
				allocatedSlotIndex = existing.SlotIndex;
				Goods merged = GoodsCreator.MakeGoods(goods.Type, goods.Id, goods.BaseCount + existing.Goods.BaseCount, goods.GainCount + existing.Goods.GainCount);
				goodsSlots[allocatedSlotIndex] = new GoodsSlot
				{
					SlotIndex = allocatedSlotIndex,
					Goods = merged
				};
			}
		}

		void PushAllocated()
		{
			allocatedSlotIndex = goodsSlots.AllocateIndex();
			goodsSlots[allocatedSlotIndex] = new GoodsSlot
			{
				SlotIndex = allocatedSlotIndex,
				Goods = goods
			};
		}

		switch (goods.Category)
		{
		case EGoodsCategory.Item:
			PushItem();
			break;
		case EGoodsCategory.Equipment:
		case EGoodsCategory.Bag:
			PushAllocated();
			break;
		}
		if (slotIndex >= 0 && slotIndex != allocatedSlotIndex)
		{
			goodsSlots.SwapIndex(allocatedSlotIndex, slotIndex);
		}
		return true;
	}

	public static bool TryPullGoods(this IDictionary<int, GoodsSlot> goodsSlots, Goods goods, out Goods[] pullGoods)
	{
		var pullGoodsList = new List<Goods>();
		bool result = false;

		bool TryPullItem()
		{
			GoodsSlot slot = goodsSlots.Values
				.Where(s => s.Goods != null && s.Goods.Category == EGoodsCategory.Item)
				.FirstOrDefault(s => (s.Goods != null ? s.Goods.Id : 0) == goods.Id);
			long topCount = slot != null ? slot.Goods.Count : 0L;
			var bagItems = new List<(int bagSlotIndex, int innerIndex)>();
			long bagTotal = 0L;
			foreach (GoodsSlot holder in goodsSlots.Values.Where(s => s.Goods != null && s.Goods.Category == EGoodsCategory.Bag))
			{
				if (holder.Goods is BagGoods bag)
				{
					for (int y = 0; y < bag.GoodsSlots.Length; y++)
					{
						GoodsSlot inner = bag.GoodsSlots[y];
						if (inner != null && inner.Goods != null && inner.Goods.Category == EGoodsCategory.Item && inner.Goods.Id == goods.Id)
						{
							bagItems.Add((holder.SlotIndex, y));
							bagTotal += inner.Goods.Count;
						}
					}
				}
			}
			if (topCount + bagTotal < goods.Count)
			{
				return false;
			}
			long remaining = goods.Count;
			if (slot != null && remaining > 0)
			{
				long take = Math.Min(remaining, slot.Goods.Count);
				long baseTake = Math.Min(take, slot.Goods.BaseCount);
				pullGoodsList.Add(GoodsCreator.MakeItemGoods(goods.Id, baseTake, take - baseTake));
				if (take < slot.Goods.Count)
				{
					goodsSlots[slot.SlotIndex] = new GoodsSlot
					{
						SlotIndex = slot.SlotIndex,
						Goods = GoodsCreator.MakeItemGoods(goods.Id, slot.Goods.BaseCount - baseTake, slot.Goods.GainCount - (take - baseTake))
					};
				}
				else
				{
					goodsSlots.Remove(slot.SlotIndex);
				}
				remaining -= take;
			}
			foreach ((int bagSlotIndex, int innerIndex) in bagItems)
			{
				if (remaining < 1)
				{
					break;
				}
				if (goodsSlots.TryPullFromBag(bagSlotIndex, innerIndex, remaining, out Goods pulled))
				{
					pullGoodsList.Add(pulled);
					remaining -= pulled.Count;
				}
			}
			return remaining == 0;
		}

		bool TryPullEquipment()
		{
			if (goods.Category != EGoodsCategory.Equipment)
			{
				return false;
			}
			List<int> topIndexes = goodsSlots.Values
				.Where(s => s.Goods != null && s.Goods.Category == EGoodsCategory.Equipment)
				.Where(s => (s.Goods != null ? s.Goods.Id : 0) == goods.Id)
				.Select(s => s.SlotIndex)
				.ToList();
			var bagEquipments = new List<(int bagSlotIndex, int innerIndex)>();
			foreach (GoodsSlot holder in goodsSlots.Values.Where(s => s.Goods != null && s.Goods.Category == EGoodsCategory.Bag))
			{
				if (holder.Goods is BagGoods bag)
				{
					for (int i = 0; i < bag.GoodsSlots.Length; i++)
					{
						GoodsSlot inner = bag.GoodsSlots[i];
						if (inner != null && inner.Goods != null && inner.Goods.Category == EGoodsCategory.Equipment && inner.Goods.Id == goods.Id)
						{
							bagEquipments.Add((holder.SlotIndex, i));
						}
					}
				}
			}
			if (topIndexes.Count + bagEquipments.Count < goods.Count)
			{
				return false;
			}
			int remaining = (int)goods.Count;
			foreach (int index in topIndexes)
			{
				if (remaining < 1)
				{
					break;
				}
				goodsSlots.TryGetValue(index, out GoodsSlot removed);
				goodsSlots.Remove(index);
				pullGoodsList.Add(removed.Goods);
				remaining--;
			}
			foreach ((int bagSlotIndex, int innerIndex) in bagEquipments)
			{
				if (remaining < 1)
				{
					break;
				}
				if (goodsSlots.TryPullFromBag(bagSlotIndex, innerIndex, 1L, out Goods pulled))
				{
					pullGoodsList.Add(pulled);
					remaining--;
				}
			}
			return remaining == 0;
		}

		bool TryPullBag()
		{
			List<int> indexes = goodsSlots.Values
				.Where(s => s.Goods != null && s.Goods.Category == EGoodsCategory.Bag)
				.Where(s => (s.Goods != null ? s.Goods.Id : 0) == goods.Id)
				.Where(s => s.Goods is BagGoods emptyBag && emptyBag.OccupiedSlotCount == 0)
				.Take((int)goods.Count)
				.Select(s => s.SlotIndex)
				.ToList();
			bool enough = indexes.Count == goods.Count;
			if (enough)
			{
				indexes.ForEach(index =>
				{
					goodsSlots.TryGetValue(index, out GoodsSlot removed);
					goodsSlots.Remove(index);
					pullGoodsList.Add(removed.Goods);
				});
			}
			return enough;
		}

		switch (goods.Category)
		{
		case EGoodsCategory.Item:
			result = TryPullItem();
			break;
		case EGoodsCategory.Equipment:
			result = TryPullEquipment();
			break;
		case EGoodsCategory.Bag:
			result = TryPullBag();
			break;
		}
		pullGoods = pullGoodsList.ToArray();
		return result;
	}

	public static bool TryPullSlot(this IDictionary<int, GoodsSlot> slots, int slotIndex, out Goods pullGoods)
	{
		pullGoods = null;
		if (!slots.TryGetValue(slotIndex, out GoodsSlot slot))
		{
			return false;
		}
		slots.Remove(slotIndex);
		pullGoods = slot.Goods;
		return true;
	}

	public static bool TryPullSlotGoods(this IDictionary<int, GoodsSlot> slots, int slotIndex, long count, out Goods pullGoods)
	{
		pullGoods = null;
		if (!slots.TryGetValue(slotIndex, out GoodsSlot slot))
		{
			return false;
		}
		if (slot != null)
		{
			if (slot.Goods != null && slot.Goods.Category == EGoodsCategory.Equipment
				|| slot.Goods != null && slot.Goods.Category == EGoodsCategory.Bag)
			{
				slots.Remove(slotIndex);
				pullGoods = slot.Goods;
				return true;
			}
			if (slot.Goods == null || slot.Goods.Category != EGoodsCategory.Item || count < 1)
			{
				return false;
			}
			Goods goods = slot.Goods;
			if (goods.Count < count)
			{
				return false;
			}
			long baseTake = Math.Min(count, goods.BaseCount);
			long gainTake = count - baseTake;
			pullGoods = GoodsCreator.MakeItemGoods(goods.Id, baseTake, gainTake);
			if (goods.Count <= count)
			{
				slots.Remove(slotIndex);
				return true;
			}
			slots[slotIndex] = new GoodsSlot
			{
				SlotIndex = slotIndex,
				Goods = GoodsCreator.MakeItemGoods(goods.Id, goods.BaseCount - baseTake, goods.GainCount - gainTake)
			};
			return true;
		}
		return false;
	}

	public static void SwapIndex(this IDictionary<int, GoodsSlot> slots, int sourceIndex, int targetIndex)
	{
		if (sourceIndex == targetIndex)
		{
			return;
		}
		slots.TryGetValue(sourceIndex, out GoodsSlot source);
		slots.TryGetValue(targetIndex, out GoodsSlot target);
		if (source == null)
		{
			slots.Remove(targetIndex);
		}
		else
		{
			GoodsSlot moved = source.Clone();
			moved.SlotIndex = targetIndex;
			slots[targetIndex] = moved;
		}
		if (target == null)
		{
			slots.Remove(sourceIndex);
		}
		else
		{
			GoodsSlot moved = target.Clone();
			moved.SlotIndex = sourceIndex;
			slots[sourceIndex] = moved;
		}
	}

	public static void Organize(this IDictionary<int, GoodsSlot> slots)
	{
		List<GoodsSlot> sorted = slots.Values
			.OrderBy(s => s.Goods == null)
			.ThenBy(s => s.Goods == null ? EGoodsType.None : s.Goods.Type)
			.ThenByDescending(s => s.Goods == null ? EGoodsGradeType.None : s.Goods.Grade)
			.ThenBy(s => s.Goods == null ? 0 : s.Goods.Id)
			.ToList();
		for (int i = 0; i < sorted.Count; i++)
		{
			GoodsSlot slot = sorted[i].Clone();
			if (sorted.Count <= slot.SlotIndex)
			{
				slots.Remove(slot.SlotIndex);
			}
			slot.SlotIndex = i;
			slots[i] = slot;
		}
	}

	public static GoodsSlot FindItemSlot(this IDictionary<int, GoodsSlot> slots, int itemId)
	{
		return slots.Values
			.Where(s => s.Goods != null && s.Goods.Category == EGoodsCategory.Item)
			.FirstOrDefault(s => s.Goods == null ? itemId == 0 : s.Goods.Id == itemId);
	}

	public static bool HasItem(this IDictionary<int, GoodsSlot> slots, int itemId)
	{
		return slots.FindItemSlot(itemId) != null;
	}

	public static long GetItemCount(this IDictionary<int, GoodsSlot> slots, int itemId)
	{
		GoodsSlot slot = slots.FindItemSlot(itemId);
		long topCount = slot == null || slot.Goods == null ? 0L : slot.Goods.Count;
		return topCount + slots.GetItemCountInBags(itemId);
	}

	public static long GetItemCountInBags(this IDictionary<int, GoodsSlot> slots, int itemId)
	{
		long total = 0L;
		foreach (GoodsSlot holder in slots.Values)
		{
			if (holder.Goods != null && holder.Goods.Category == EGoodsCategory.Bag && holder.Goods is BagGoods bag)
			{
				for (int i = 0; i < bag.GoodsSlots.Length; i++)
				{
					GoodsSlot inner = bag.GoodsSlots[i];
					if (inner != null && inner.Goods != null && inner.Goods.Category == EGoodsCategory.Item && inner.Goods.Id == itemId)
					{
						total += inner.Goods.Count;
					}
				}
			}
		}
		return total;
	}

	public static long GetEquipmentCount(this IDictionary<int, GoodsSlot> slots, int equipmentId)
	{
		int topCount = slots.Values.Count(s => s.Goods != null && s.Goods.Category == EGoodsCategory.Equipment && s.Goods.Id == equipmentId);
		return slots.GetEquipmentCountInBags(equipmentId) + topCount;
	}

	public static long GetEquipmentCountInBags(this IDictionary<int, GoodsSlot> slots, int equipmentId)
	{
		long total = 0L;
		foreach (GoodsSlot holder in slots.Values)
		{
			if (holder.Goods != null && holder.Goods.Category == EGoodsCategory.Bag && holder.Goods is BagGoods bag)
			{
				for (int i = 0; i < bag.GoodsSlots.Length; i++)
				{
					GoodsSlot inner = bag.GoodsSlots[i];
					if (inner != null && inner.Goods != null && inner.Goods.Category == EGoodsCategory.Equipment && inner.Goods.Id == equipmentId)
					{
						total++;
					}
				}
			}
		}
		return total;
	}

	public static long GetBagCount(this IDictionary<int, GoodsSlot> slots, int bagId)
	{
		return slots.Values.Count(s => s.Goods != null && s.Goods.Category == EGoodsCategory.Bag && s.Goods.Id == bagId);
	}

	public static long GetEmptyBagCount(this IDictionary<int, GoodsSlot> slots, int bagId)
	{
		return slots.Values.Count(s => s.Goods != null && s.Goods.Category == EGoodsCategory.Bag && s.Goods.Id == bagId && s.Goods is BagGoods bag && bag.OccupiedSlotCount == 0);
	}

	public static List<Goods> ApplyStageDead(this IDictionary<int, GoodsSlot> inventorySlots, ERoomDifficulty difficulty)
	{
		float removalPercent = difficulty.GetGoodsRemovalPercent();
		var removed = new List<Goods>();
		List<int> keys = inventorySlots.Keys.Where(_ => RandomUtility.GetRandomBoolFromPercentage(removalPercent)).ToList();
		foreach (int key in keys)
		{
			if (inventorySlots.TryGetValue(key, out GoodsSlot slot) && slot.Goods != null)
			{
				removed.Add(slot.Goods);
			}
			inventorySlots.Remove(key);
		}
		return removed;
	}

	public static void NormalizeCount(this IDictionary<int, GoodsSlot> slots)
	{
		slots.Values
			.Where(s => s.Goods != null && s.Goods.Category == EGoodsCategory.Item)
			.Where(s => GameCommon.Table.TItem.Table[s.Goods.Id].IsEventItem)
			.Select(s => s.SlotIndex)
			.ToList()
			.ForEach(key => slots.Remove(key));
		slots.Values
			.Where(s => s.Goods.GainCount > 0)
			.Select(s => new GoodsSlot
			{
				SlotIndex = s.SlotIndex,
				Goods = s.Goods.CloneWithoutGain()
			})
			.ToList()
			.ForEach(slot => slots[slot.SlotIndex] = slot);
	}
}
