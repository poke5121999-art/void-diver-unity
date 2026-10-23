using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using UniRx;
using UnityEngine;

public class LootingInventory : Inventory
{
	[SerializeField]
	private int _index;

	private CompositeDisposable _lootingDisposable;

	private readonly HashSet<ulong> _activeLooters = new HashSet<ulong>();

	private int _slotCount;

	public bool IsIndividual => _index >= 0;

	public ulong IndividualClientId { get; private set; }

	public override int SlotCount => _slotCount;

	private void Start()
	{
		if (_index >= 0)
		{
			List<ulong> list = GameContext.Players.Keys.OrderBy((ulong k) => k).ToList();
			if (list.Count <= _index)
			{
				return;
			}
			IndividualClientId = list[_index];
		}
		if (GameManager.IsHost)
		{
			GameHost.Receive<ReqSyncInventory>().Where((ReqSyncInventory req) => req.NetworkObjectId == NetworkBase.NetworkObjectId).Where((ReqSyncInventory req) => _index < 0 || req.SenderId == IndividualClientId).Subscribe(OnSyncInventory)
				.AddTo(this);
			return;
		}
		GameChannel.Receive<ResSyncInventory>().Where((ResSyncInventory res) => res.NetworkObjectId == NetworkBase.NetworkObjectId).Where((ResSyncInventory res) => _index < 0 || res.IndividualClientId == IndividualClientId).Subscribe(OnResSyncInventory)
			.AddTo(this);
		GameChannel.Receive<NtfUpdateInventory>().Where((NtfUpdateInventory ntf) => ntf.NetworkObjectId == NetworkBase.NetworkObjectId).Where((NtfUpdateInventory ntf) => _index < 0 || ntf.IndividualClientId == IndividualClientId).Subscribe(OnUpdateInventory)
			.AddTo(this);
		GameChannel.SendToHost(new ReqSyncInventory
		{
			NetworkObjectId = NetworkBase.NetworkObjectId
		});
	}

	protected override void OnResSyncInventory(ResSyncInventory message)
	{
		_slotCount = message.SlotCount;
		base.OnResSyncInventory(message);
	}

	private void OnDestroy()
	{
		_lootingDisposable?.Dispose();
	}

	public override void Init(ICollection<GoodsSlot> goodsSlots, int slotCount)
	{
		GameManager.AssertIfNotHost();
		InitSlots(goodsSlots);
		_slotCount = slotCount;
	}

	public override void Init(ICollection<GoodsSlot> slots, ICollection<GoodsSlot> subSlots)
	{
		throw new NotImplementedException();
	}

	protected override void Synchronize()
	{
		if (!HasSlotsChange)
		{
			return;
		}
		GameManager.AssertIfNotHost();
		GoodsSlot[] array = FlushSlots();
		ApplySlots(array);
		GameHost.SendToAllExceptHost(new NtfUpdateInventory
		{
			Tick = NetworkManager.ServerTime.Tick,
			NetworkObjectId = NetworkBase.NetworkObjectId,
			IndividualClientId = IndividualClientId,
			ChangeSlots = array
		});
	}

	public override long GetItemCount(int itemId)
	{
		return Slots.GetItemCount(itemId);
	}

	public override long GetEquipmentCount(int equipmentId)
	{
		return Slots.GetEquipmentCount(equipmentId);
	}

	public override long GetGoodsCount(EGoodsType goodsType, int goodsId)
	{
		switch (goodsType)
		{
		case EGoodsType.Consumable:
		case EGoodsType.Valuable:
		case EGoodsType.Misc:
		case EGoodsType.Note:
		case EGoodsType.Blueprint:
		case EGoodsType.MusicDisc:
			return GetItemCount(goodsId);
		case EGoodsType.Weapon:
		case EGoodsType.Accessory:
		case EGoodsType.Artifact:
			return GetEquipmentCount(goodsId);
		case EGoodsType.Bag:
			return Slots.GetBagCount(goodsId);
		default:
			throw new ArgumentOutOfRangeException("goodsType", goodsType, null);
		}
	}

	public override bool CanMoveGoods(Goods goods)
	{
		if (!IsSlotFull())
		{
			return true;
		}
		if (goods.IsEquipment())
		{
			return false;
		}
		if (Slots.HasItem(goods.Id))
		{
			return true;
		}
		return false;
	}

	public bool IsAllRevealed()
	{
		return Slots.Values.All((GoodsSlot _) => _.IsRevealed);
	}

	public void StartLooting(CharacterModel sourceModel)
	{
		GameManager.AssertIfNotHost();
		float delayMultiplier = GetRevealDelayMultiplier(sourceModel);
		_activeLooters.Add(sourceModel.ClientId);
		if (_activeLooters.Count < 2)
		{
			TryReveal(delayMultiplier);
		}
	}

	private float GetRevealDelayMultiplier(CharacterModel model)
	{
		if (model == null)
		{
			return 0f;
		}
		IList<BuffEffectInfo<LootingSpeedAmplifierEffect>> buffEffects = model.GetBuffEffects<LootingSpeedAmplifierEffect>();
		if (!buffEffects.Any())
		{
			return 1f;
		}
		float num = 1f - buffEffects.Sum((BuffEffectInfo<LootingSpeedAmplifierEffect> b) => (float)b.Stack * b.BuffEffect.Percent) / 100f;
		if (num <= 0f)
		{
			return 0f;
		}
		return num;
	}

	private bool TryReveal(float delayMultiplier)
	{
		_lootingDisposable?.Dispose();
		_lootingDisposable = new CompositeDisposable();
		GoodsSlot targetSlot = Slots.Values.OrderBy((GoodsSlot s) => s.SlotIndex).FirstOrDefault((GoodsSlot s) => !s.IsRevealed);
		if (targetSlot == null || targetSlot.IsEmpty)
		{
			return false;
		}
		float revealTime = targetSlot.Grade.GetRevealTime();
		int slotIndex = targetSlot.SlotIndex;
		Observable.Timer(TimeSpan.FromSeconds(delayMultiplier * revealTime)).Subscribe(delegate
		{
			GoodsSlot goodsSlot = targetSlot.Clone();
			goodsSlot.IsUnrevealed = false;
			Slots[slotIndex] = goodsSlot;
			TryReveal(delayMultiplier);
		}).AddTo(_lootingDisposable);
		return true;
	}

	public void EndLooting(ulong clientId)
	{
		GameManager.AssertIfNotHost();
		_activeLooters.Remove(clientId);
		if (_activeLooters.Count < 1)
		{
			_lootingDisposable?.Dispose();
		}
	}

	public override void PushGoods(Goods goods, int slotIndex)
	{
		GameManager.AssertIfNotHost();
		if (goods == null)
		{
			return;
		}
		EGoodsCategory category = goods.Category;
		if (category == EGoodsCategory.Item)
		{
			PushItem();
		}
		else if (category == EGoodsCategory.Equipment)
		{
			if (IsSlotFull())
			{
				Logger.Error("Safe slot is full; cannot add equipment.");
			}
			else if (!Slots.TryPushGoods(goods, slotIndex))
			{
				Logger.Error("Failed to push equipment into safe slot.");
			}
		}
		else if (category == EGoodsCategory.Bag)
		{
			if (IsSlotFull())
			{
				Logger.Error("Looting slot is full; cannot add bag.");
			}
			else if (!Slots.TryPushGoods(goods, slotIndex))
			{
				Logger.Error("Failed to push bag into looting slot.");
			}
		}
		void PushItem()
		{
			GoodsSlot goodsSlot = Slots.FindItemSlot(goods.Id);
			if (goodsSlot == null && IsSlotFull())
			{
				Logger.Error("No available slot to add item to safe inventory.");
			}
			else
			{
				long num = Math.Min(goods.Count, goods.BaseCount);
				Goods goods2 = GoodsCreator.MakeItemGoods(goods.Id, num, goods.Count - num);
				Slots.TryPushGoods(goods2, slotIndex);
			}
		}
	}

	public override bool TryPushGoods(Goods goods, out Goods remainGoods, int slotIndex = -1)
	{
		GameManager.AssertIfNotHost();
		remainGoods = goods;
		if (goods == null || goods.Count < 1)
		{
			return false;
		}
		Goods remain = goods;
		bool result;
		switch (goods.Category)
		{
		case EGoodsCategory.Item:
			result = TryPushItem();
			break;
		case EGoodsCategory.Equipment:
		case EGoodsCategory.Bag:
			result = TryPushEquipment();
			break;
		default:
			return false;
		}
		remainGoods = remain;
		return result;
		bool TryPushEquipment()
		{
			remain = goods;
			if (IsSlotFull())
			{
				return false;
			}
			if (Slots.TryPushGoods(goods, slotIndex))
			{
				remain = null;
				return true;
			}
			return false;
		}
		bool TryPushItem()
		{
			remain = goods;
			GoodsSlot goodsSlot = Slots.FindItemSlot(goods.Id);
			if (goodsSlot == null && IsSlotFull())
			{
				return false;
			}
			if (Slots.TryPushGoods(goods, slotIndex))
			{
				remain = null;
				return true;
			}
			return false;
		}
	}

	public override bool TryPullGoods(Goods goods, out Goods[] pullGoods)
	{
		return Slots.TryPullGoods(goods, out pullGoods);
	}
}
