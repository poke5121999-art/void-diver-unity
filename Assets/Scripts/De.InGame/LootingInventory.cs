using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;
using UniRx;
using UnityEngine;

public class LootingInventory : Inventory
{
	[SerializeField]
	private int _index;

	private CompositeDisposable _lootingDisposable;

	private readonly HashSet<ulong> _activeLooters;

	private int _slotCount;

	public bool IsIndividual => false;

	public ulong IndividualClientId { get; private set; }

	public override int SlotCount => 0;

	private void Start()
	{
	}

	protected override void OnResSyncInventory(ResSyncInventory message)
	{
	}

	private void OnDestroy()
	{
	}

	public override void Init(ICollection<GoodsSlot> goodsSlots, int slotCount)
	{
	}

	public override void Init(ICollection<GoodsSlot> slots, ICollection<GoodsSlot> subSlots)
	{
	}

	protected override void Synchronize()
	{
	}

	public override long GetItemCount(int itemId)
	{
		return 0L;
	}

	public override long GetEquipmentCount(int equipmentId)
	{
		return 0L;
	}

	public override long GetGoodsCount(EGoodsType goodsType, int goodsId)
	{
		return 0L;
	}

	public override bool CanMoveGoods(Goods goods)
	{
		return false;
	}

	public bool IsAllRevealed()
	{
		return false;
	}

	public void StartLooting(CharacterModel sourceModel)
	{
	}

	private float GetRevealDelayMultiplier(CharacterModel model)
	{
		return 0f;
	}

	private bool TryReveal(float delayMultiplier)
	{
		return false;
	}

	public void EndLooting(ulong clientId)
	{
	}

	public override void PushGoods(Goods goods, int slotIndex)
	{
	}

	public override bool TryPushGoods(Goods goods, out Goods remainGoods, int slotIndex = -1)
	{
		remainGoods = null;
		return false;
	}

	public override bool TryPullGoods(Goods goods, out Goods[] pullGoods)
	{
		pullGoods = null;
		return false;
	}
}
