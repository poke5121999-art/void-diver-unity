using System.Collections.Generic;
using System.Linq;
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

	private readonly TrackableDictionary<int, GoodsSlot> _trackableSlots = new TrackableDictionary<int, GoodsSlot>();

	private readonly Dictionary<int, GoodsSlot> _slots = new Dictionary<int, GoodsSlot>();

	public readonly Subject<IList<GoodsSlotChangeData>> OnInventoryChanged = new Subject<IList<GoodsSlotChangeData>>();

	private int _syncTick;

	public abstract int SlotCount { get; }

	public IDictionary<int, GoodsSlot> Slots
	{
		get
		{
			if (GameManager.IsHost)
			{
				return _trackableSlots;
			}
			return _slots;
		}
	}

	protected bool IsHost => GameManager.IsHost;

	public bool IsEmpty
	{
		get
		{
			if (Slots.Count == 0)
			{
				return true;
			}
			return Slots.Values.Where((GoodsSlot _) => _ != null).All((GoodsSlot _) => _.IsEmpty || _.Count == 0);
		}
	}

	protected bool HasSlotsChange => _trackableSlots.HasChange;

	public ulong NetworkObjectId => NetworkBase.NetworkObjectId;

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
		if (GameManager.IsHost)
		{
			Synchronize();
		}
	}

	public void SynchronizeImmediate()
	{
		GameManager.AssertIfNotHost();
		Synchronize();
	}

	protected void InitSlots(ICollection<GoodsSlot> goodsSlots)
	{
		GameManager.AssertIfNotHost();
		_trackableSlots.Clear();
		_slots.Clear();
		if (goodsSlots != null)
		{
			foreach (GoodsSlot goodsSlot in goodsSlots)
			{
				_trackableSlots.Add(goodsSlot.Key, goodsSlot);
				_slots.Add(goodsSlot.Key, goodsSlot);
			}
			_trackableSlots.ClearDirty();
		}
	}

	protected GoodsSlot[] FlushSlots()
	{
		GameManager.AssertIfNotHost();
		return _trackableSlots.Flush();
	}

	public bool IsSlotFull()
	{
		return SlotCount <= Slots.Count;
	}

	public int GetEmptySlotCount()
	{
		return SlotCount - Slots.Count;
	}

	protected void ApplySlots(GoodsSlot[] slots)
	{
		if (slots == null)
		{
			return;
		}
		List<GoodsSlotChangeData> changeDatas = new List<GoodsSlotChangeData>();
		foreach (GoodsSlot slot in slots)
		{
			GoodsSlot value = null;
			if (slot.Operation == EDataOperation.Remove)
			{
				_slots.TryGetValue(slot.Key, out value);
				_slots.Remove(slot.Key);
			}
			GoodsSlot newSlot = null;
			if (slot.Operation == EDataOperation.Add)
			{
				_slots.Add(slot.Key, slot);
				newSlot = slot;
			}
			if (slot.Operation == EDataOperation.Replace)
			{
				_slots.TryGetValue(slot.Key, out value);
				_slots[slot.Key] = slot;
				newSlot = slot;
			}
			changeDatas.Add(new GoodsSlotChangeData
			{
				Operation = slot.Operation,
				Key = slot.Key,
				OldSlot = value,
				NewSlot = newSlot
			});
		}
		OnInventoryChanged.OnNext(changeDatas);
	}

	public bool TryGainGoods(Goods goods)
	{
		GameManager.AssertIfNotHost();
		if (goods == null || goods.Count < 1)
		{
			return true;
		}
		if (GameManager.IsInLounge && goods.GainCount > 0)
		{
			goods = goods.CloneWithoutGain();
		}
		if (TryPushGoods(goods, out var remainGoods))
		{
			return true;
		}
		ObjectManager.SpawnDropGoods(remainGoods, base.transform.position);
		return false;
	}

	public bool TryGainGoods(Goods[] goodsArray)
	{
		if (goodsArray != null && goodsArray.Any())
		{
			bool flag = true;
			foreach (Goods goods in goodsArray)
			{
				bool flag2 = TryGainGoods(goods);
				flag = flag && flag2;
			}
			return flag;
		}
		return true;
	}

	public void ConsumeGoods(GoodsData goodsData)
	{
		Goods goods = GoodsCreator.MakeFromGoodsData(goodsData, goodsData.Count, 0L);
		if (!TryPullGoods(goods, out var _))
		{
			Logger.Error(string.Format("Failed: {0}", goodsData));
		}
	}

	public void ConsumeGoods(GoodsData[] goodsDatas)
	{
		if (goodsDatas != null && goodsDatas.Any())
		{
			foreach (GoodsData item in goodsDatas.Where((GoodsData _) => 0 < _.Count))
			{
				ConsumeGoods(item);
			}
		}
	}

	public void DropGoods(Goods goods)
	{
		if (!TryPullGoods(goods, out var pullGoods))
		{
			Logger.Error(string.Format("Failed: Type={0}, Id={1}, Count={2}", goods.Type, goods.Id, goods.Count));
		}
		else
		{
			ObjectManager.SpawnDropGoods(pullGoods, base.transform.position);
		}
	}

	protected void OnSyncInventory(ReqSyncInventory message)
	{
		ulong senderId = message.SenderId;
		ResSyncInventory resSyncInventory = new ResSyncInventory();
		resSyncInventory.Tick = NetworkManager.ServerTime.Tick;
		resSyncInventory.NetworkObjectId = message.NetworkObjectId;
		resSyncInventory.IndividualClientId = message.SenderId;
		resSyncInventory.Slots = Slots.Values.ToArray();
		resSyncInventory.SlotCount = SlotCount;
		GameHost.SendToClient(senderId, resSyncInventory);
	}

	protected void OnUpdateInventory(NtfUpdateInventory message)
	{
		if (0 < _syncTick && _syncTick < message.Tick)
		{
			ApplySlots(message.ChangeSlots);
		}
	}

	protected virtual void OnResSyncInventory(ResSyncInventory message)
	{
		Slots.Clear();
		if (message.Slots != null)
		{
			foreach (GoodsSlot slot in message.Slots)
			{
				Slots[slot.Key] = slot;
			}
		}
		_syncTick = message.Tick;
	}
}
