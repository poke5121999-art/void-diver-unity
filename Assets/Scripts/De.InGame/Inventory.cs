using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.Log;
using Tracker;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using VContainer;

public abstract class Inventory : MonoBase
{
	[SerializeField]
	protected NetworkBase NetworkBase;

	[Inject]
	protected GameManager GameManager;

	[Inject]
	protected GameHost GameHost;

	[Inject]
	protected GameChannel GameChannel;

	[Inject]
	protected SeriLogger Logger;

	[Inject]
	protected GameObjectManager ObjectManager;

	[Inject]
	protected GameContext GameContext;

	[Inject]
	protected NetworkManager NetworkManager;

	private readonly TrackableDictionary<int, GoodsSlot> _trackableSlots;

	private readonly Dictionary<int, GoodsSlot> _slots;

	public readonly Subject<IList<GoodsSlotChangeData>> OnInventoryChanged;

	private int _syncTick;

	public abstract int SlotCount { get; }

	public IDictionary<int, GoodsSlot> Slots => null;

	protected bool IsHost => false;

	public bool IsEmpty => false;

	protected bool HasSlotsChange => false;

	public ulong NetworkObjectId => 0uL;

	public abstract void Init(ICollection<GoodsSlot> slots, int slotCount);

	public abstract void Init(ICollection<GoodsSlot> slots, ICollection<GoodsSlot> subSlots);

	protected abstract void Synchronize();

	public abstract void PushGoods(Goods goods, int slotIndex);

	public abstract bool TryPushGoods(Goods goods, out Goods remainGoods, int slotIndex = -1);

	public abstract bool TryPullGoods(Goods goods, out Goods[] pullGoods);

	public abstract long GetItemCount(int itemId);

	public abstract long GetEquipmentCount(int equipmentId);

	public abstract long GetGoodsCount(EGoodsType goodsType, int goodsId);

	public abstract bool CanMoveGoods(Goods goods);

	protected virtual void FixedUpdate()
	{
	}

	public void SynchronizeImmediate()
	{
	}

	protected void InitSlots(ICollection<GoodsSlot> goodsSlots)
	{
	}

	protected GoodsSlot[] FlushSlots()
	{
		return null;
	}

	public bool IsSlotFull()
	{
		return false;
	}

	public int GetEmptySlotCount()
	{
		return 0;
	}

	protected void ApplySlots(GoodsSlot[] slots)
	{
	}

	public bool TryGainGoods(Goods goods)
	{
		return false;
	}

	public bool TryGainGoods(Goods[] goodsArray)
	{
		return false;
	}

	public void ConsumeGoods(GoodsData goodsData)
	{
	}

	public void ConsumeGoods(GoodsData[] goodsDatas)
	{
	}

	public void DropGoods(Goods goods)
	{
	}

	protected void OnSyncInventory(ReqSyncInventory message)
	{
	}

	protected void OnUpdateInventory(NtfUpdateInventory message)
	{
	}

	protected virtual void OnResSyncInventory(ResSyncInventory message)
	{
	}
}
