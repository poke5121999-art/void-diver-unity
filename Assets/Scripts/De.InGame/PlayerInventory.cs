using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using Tracker;
using UniRx;

public class PlayerInventory : Inventory
{
	private CharacterModel _characterModel;

	private Goods[] _initHighGradeGoodsCache = Array.Empty<Goods>();

	private bool _isFirstSyncReceived;

	private readonly TrackableDictionary<int, GoodsSlot> _trackableSafeSlots = new TrackableDictionary<int, GoodsSlot>();

	private readonly Dictionary<int, GoodsSlot> _safeSlots = new Dictionary<int, GoodsSlot>();

	public readonly Subject<IList<GoodsSlotChangeData>> OnSafeInventoryChanged = new Subject<IList<GoodsSlotChangeData>>();

	private int _syncSafeSlotsTick;

	private ulong ClientId => _characterModel.ClientId;

	public bool IsMine => ClientId == GameManager.LocalClientId;

	public GamePlayer Player => GameContext.Players[ClientId];

	public override int SlotCount => Player.SlotCount;

	private IDictionary<int, GoodsSlot> PlayerSlots
	{
		get
		{
			if (!GameContext.Players.TryGetValue(ClientId, out var value))
			{
				return null;
			}
			return value.InventorySlots;
		}
	}

	public int SafeSlotCount => Player.SafeSlotCount;

	public IDictionary<int, GoodsSlot> SafeSlots
	{
		get
		{
			if (GameManager.IsHost)
			{
				return _trackableSafeSlots;
			}
			return _safeSlots;
		}
	}

	private bool HasSafeSlotsChange => _trackableSafeSlots.HasChange;

	private IDictionary<int, GoodsSlot> PlayerSafeSlots
	{
		get
		{
			if (!GameContext.Players.TryGetValue(ClientId, out var value))
			{
				return null;
			}
			return value.SafeInventorySlots;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		_characterModel = GetComponent<CharacterModel>();
	}

	public override void Init(ICollection<GoodsSlot> goodsSlots, int slotCount)
	{
		throw new NotImplementedException();
	}

	public override void Init(ICollection<GoodsSlot> slots, ICollection<GoodsSlot> subSlots)
	{
		GameManager.AssertIfNotHost();
		InitSlots(slots);
		InitSafeSlots(subSlots);
		if (subSlots == null)
		{
			subSlots = Array.Empty<GoodsSlot>();
		}
		_initHighGradeGoodsCache = slots.Concat(subSlots).Where((GoodsSlot s) => s.Grade > EGoodsGradeType.Elite).Select((GoodsSlot s) => s.Goods)
			.ToArray();
	}

	protected override void Synchronize()
	{
		// TODO(decomp): the first (normal slots) branch was dropped by Ghidra; rebuilt from the disassembly call order @0x1806093E0
		if (HasSlotsChange)
		{
			GoodsSlot[] array = FlushSlots();
			ApplySlots(array);
			GameHost.SendToAllExceptHost(new NtfUpdateInventory
			{
				Tick = NetworkManager.ServerTime.Tick,
				NetworkObjectId = NetworkBase.NetworkObjectId,
				ChangeSlots = array
			});
		}
		if (HasSafeSlotsChange)
		{
			GoodsSlot[] array2 = FlushSafeSlots();
			ApplySafeSlots(array2);
			GameHost.SendToAllExceptHost(new NtfUpdateSafeInventory
			{
				Tick = NetworkManager.ServerTime.Tick,
				NetworkObjectId = NetworkBase.NetworkObjectId,
				ChangeSafeSlots = array2
			});
		}
	}

	public override long GetItemCount(int itemId)
	{
		return Slots.GetItemCount(itemId) + SafeSlots.GetItemCount(itemId);
	}

	public override long GetEquipmentCount(int equipmentId)
	{
		return Slots.GetEquipmentCount(equipmentId) + SafeSlots.GetEquipmentCount(equipmentId);
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
			return GetBagCount(goodsId);
		default:
			throw new ArgumentOutOfRangeException("goodsType", goodsType, null);
		}
	}

	public long GetBagCount(int bagId)
	{
		return Slots.Values.Count((GoodsSlot s) => s.Category == EGoodsCategory.Bag && s.Id == bagId) + SafeSlots.Values.Count((GoodsSlot s) => s.Category == EGoodsCategory.Bag && s.Id == bagId);
	}

	public long GetEmptyBagCount(int bagId)
	{
		return Slots.GetEmptyBagCount(bagId) + SafeSlots.GetEmptyBagCount(bagId);
	}

	public bool HasDuplicateBagType(EBagType type, EGoodsSlotCategory excludeSourceCategory, int excludeSourceIndex, EGoodsSlotCategory excludeDestCategory, int excludeDestIndex)
	{
		foreach (GoodsSlot value in Slots.Values)
		{
			if ((excludeSourceCategory != EGoodsSlotCategory.InventorySlot || value.SlotIndex != excludeSourceIndex) && (excludeDestCategory != EGoodsSlotCategory.InventorySlot || value.SlotIndex != excludeDestIndex) && value.Goods is BagGoods bagGoods && TBag.Table[bagGoods.Id].Type == type)
			{
				return true;
			}
		}
		foreach (GoodsSlot value2 in SafeSlots.Values)
		{
			if ((excludeSourceCategory != EGoodsSlotCategory.SafeSlot || value2.SlotIndex != excludeSourceIndex) && (excludeDestCategory != EGoodsSlotCategory.SafeSlot || value2.SlotIndex != excludeDestIndex) && value2.Goods is BagGoods bagGoods2 && TBag.Table[bagGoods2.Id].Type == type)
			{
				return true;
			}
		}
		return false;
	}

	public override bool CanMoveGoods(Goods goods)
	{
		if (goods.IsEquipment() && IsSlotFull())
		{
			return false;
		}
		if (goods.IsBag() && IsSlotFull())
		{
			return false;
		}
		if (!goods.IsItem())
		{
			return true;
		}
		if (GetAddableItemCount(goods.Id) > 0 && (Slots.HasItem(goods.Id) || !IsSlotFull()))
		{
			return true;
		}
		if (GetAddableCountInBagStacks(goods.Id) <= 0)
		{
			return false;
		}
		return true;
	}

	public bool CanAddGoods(Goods goods)
	{
		string reason;
		return CanAddGoods(goods, out reason);
	}

	public bool CanAddGoods(Goods goods, out string reason)
	{
		TLocalizedText tLocalizedText;
		if (goods.IsEquipment() && IsSlotFull())
		{
			tLocalizedText = TLocalizedText.NotEnoughInventorySlots;
		}
		else if (goods.IsBag() && IsSlotFull())
		{
			tLocalizedText = TLocalizedText.NotEnoughInventorySlots;
		}
		else if (goods is BagGoods bagGoods && HasDuplicateBagType(TBag.Table[bagGoods.Id].Type, EGoodsSlotCategory.None, -1, EGoodsSlotCategory.None, -1))
		{
			tLocalizedText = TLocalizedText.CannotCarrySameBagType;
		}
		else
		{
			if (!goods.IsItem())
			{
				reason = string.Empty;
				return true;
			}
			int id = goods.Id;
			long num = 0L;
			if (Slots.HasItem(id) || SafeSlots.HasItem(id) || !IsSlotFull() || !IsSafeSlotFull())
			{
				num = GetAddableItemCount(id);
			}
			long addableCountInBagStacks = GetAddableCountInBagStacks(id);
			if (goods.Count > addableCountInBagStacks + num)
			{
				tLocalizedText = TLocalizedText.AlreadyAtMaxCount;
			}
			else
			{
				if (GetAddableItemCount(id) > 0 && (Slots.HasItem(goods.Id) || !IsSlotFull()))
				{
					reason = string.Empty;
					return true;
				}
				if (GetAddableCountInBagStacks(goods.Id) > 0)
				{
					reason = string.Empty;
					return true;
				}
				tLocalizedText = TLocalizedText.NotEnoughInventorySlots;
			}
		}
		reason = tLocalizedText.LocalText;
		return false;
	}

	private void Start()
	{
		if (GameManager.IsHost)
		{
			OnInventoryChanged.Subscribe(OnSlotsChanged).AddTo(this);
			GameManager.RxStageState.Where((EStageState _) => _ == EStageState.Ended).Subscribe(delegate
			{
				OnStageEndedAsync().Forget();
			}).AddTo(this);
			GameHost.Receive<ReqSyncInventory>().Where((ReqSyncInventory _) => _.NetworkObjectId == NetworkBase.NetworkObjectId).Subscribe(OnSyncInventory)
				.AddTo(this);
			GameManager.OnPlayerInventoryInitialized.OnNext(this);
		}
		else
		{
			GameChannel.Receive<ResSyncInventory>().Where((ResSyncInventory _) => _.NetworkObjectId == NetworkBase.NetworkObjectId).Subscribe(OnResSyncInventory)
				.AddTo(this);
			GameChannel.Receive<NtfUpdateInventory>().Where((NtfUpdateInventory _) => _.NetworkObjectId == NetworkBase.NetworkObjectId).Subscribe(OnUpdateInventory)
				.AddTo(this);
			GameChannel.SendToHost(new ReqSyncInventory
			{
				NetworkObjectId = NetworkBase.NetworkObjectId
			});
		}
		InitSafeInventoryEvents();
	}

	protected override void OnResSyncInventory(ResSyncInventory message)
	{
		base.OnResSyncInventory(message);
		if (!_isFirstSyncReceived)
		{
			GameManager.OnPlayerInventoryInitialized.OnNext(this);
			_isFirstSyncReceived = true;
		}
	}

	public override void PushGoods(Goods goods, int slotIndex)
	{
		GameManager.AssertIfNotHost();
		if (goods == null)
		{
			return;
		}
		if (GameManager.IsInLounge && goods.GainCount > 0)
		{
			goods = goods.CloneWithoutGain();
		}
		if (slotIndex < 0)
		{
			goods = StackIntoExistingBags(goods);
		}
		if (goods == null)
		{
			return;
		}
		EGoodsCategory category = goods.Category;
		if (category == EGoodsCategory.Item)
		{
			PushItem();
			return;
		}
		string message;
		if (category == EGoodsCategory.Equipment)
		{
			if (IsSlotFull())
			{
				message = "Safe slot is full; cannot add equipment.";
			}
			else
			{
				if (Slots.TryPushGoods(goods, slotIndex))
				{
					return;
				}
				message = "Failed to push equipment into safe slot.";
			}
		}
		else
		{
			if (category != EGoodsCategory.Bag)
			{
				return;
			}
			if (IsSlotFull())
			{
				message = "Inventory slot is full; cannot add bag.";
			}
			else
			{
				if (Slots.TryPushGoods(goods, slotIndex))
				{
					return;
				}
				message = "Failed to push bag into inventory slot.";
			}
		}
		Logger.Error(message);
		void PushItem()
		{
			long addableItemCount = GetAddableItemCount(goods.Id);
			if (addableItemCount < goods.Count)
			{
				Logger.Error("Attempted to add more items than acceptable for safe slot.");
				return;
			}
			GoodsSlot goodsSlot = SafeSlots.FindItemSlot(goods.Id);
			if (goodsSlot == null)
			{
				if (Slots.FindItemSlot(goods.Id) == null && IsSlotFull())
				{
					Logger.Error("No available slot to add item to safe inventory.");
					return;
				}
				long num = Math.Min(goods.Count, goods.BaseCount);
				Goods goods2 = GoodsCreator.MakeItemGoods(goods.Id, num, goods.Count - num);
				Slots.TryPushGoods(goods2, slotIndex);
				return;
			}
			int slotIndex2 = goodsSlot.SlotIndex;
			if (goods.Count > 0 && !SafeSlots.TryPushGoods(goods, -1))
			{
				Logger.Error("Failed to push goods into inventory.");
			}
			else if (slotIndex >= 0)
			{
				if (SafeSlots.TryPullSlot(slotIndex2, out var pullGoods))
				{
					if (!Slots.TryPushGoods(pullGoods, slotIndex))
					{
						Logger.Error(string.Format("Failed to push pulled goods to safe slot {0}.", slotIndex));
					}
				}
				else
				{
					Logger.Error(string.Format("Failed to pull goods from inventory slot {0}.", slotIndex2));
				}
			}
		}
	}

	private Goods StackIntoExistingBags(Goods goods)
	{
		if (goods != null && goods.Category == EGoodsCategory.Item)
		{
			int inventoryItemCountMax = Player.GetInventoryItemCountMax(goods.Id);
			Goods goods2 = Slots.StackIntoExistingBagSlots(goods, inventoryItemCountMax);
			Goods result = null;
			if (goods2 != null)
			{
				result = SafeSlots.StackIntoExistingBagSlots(goods2, inventoryItemCountMax);
			}
			return result;
		}
		return goods;
	}

	public override bool TryPushGoods(Goods goods, out Goods remainGoods, int slotIndex = -1)
	{
		GameManager.AssertIfNotHost();
		remainGoods = goods;
		if (goods == null || goods.Count <= 0)
		{
			return false;
		}
		if (slotIndex < 0)
		{
			goods = StackIntoExistingBags(goods);
			if (goods == null)
			{
				remainGoods = null;
				return true;
			}
		}
		EGoodsCategory category = goods.Category;
		switch (category)
		{
		case EGoodsCategory.Item:
		{
			remainGoods = goods;
			return TryPushItem(ref remainGoods);
		}
		case EGoodsCategory.Equipment:
			remainGoods = goods;
			if (!IsSlotFull() && Slots.TryPushGoods(goods, slotIndex))
			{
				remainGoods = null;
				return true;
			}
			break;
		case EGoodsCategory.Bag:
			remainGoods = goods;
			if (goods is BagGoods bagGoods && HasDuplicateBagType(TBag.Table[bagGoods.Id].Type, EGoodsSlotCategory.None, -1, EGoodsSlotCategory.None, -1))
			{
				return false;
			}
			if (!IsSlotFull() && Slots.TryPushGoods(goods, slotIndex))
			{
				remainGoods = null;
				return true;
			}
			break;
		}
		return false;
		bool TryPushItem(ref Goods remain)
		{
			remain = goods;
			long num = Math.Min(GetAddableItemCount(goods.Id), goods.Count);
			if (num < 1)
			{
				return false;
			}
			long num2 = Math.Min(num, goods.BaseCount);
			Goods goods2 = GoodsCreator.MakeItemGoods(goods.Id, num2, num - num2);
			Goods goods3 = null;
			if (num < goods.Count)
			{
				goods3 = GoodsCreator.MakeItemGoods(goods.Id, goods.BaseCount - num2, goods.GainCount - (num - num2));
			}
			GoodsSlot goodsSlot = SafeSlots.FindItemSlot(goods.Id);
			if (goodsSlot == null)
			{
				if (Slots.FindItemSlot(goods.Id) == null && IsSlotFull())
				{
					return false;
				}
				if (!Slots.TryPushGoods(goods2, slotIndex))
				{
					return false;
				}
				remain = goods3;
				return remain == null;
			}
			int slotIndex2 = goodsSlot.SlotIndex;
			if (goods.Count > 0 && !SafeSlots.TryPushGoods(goods2, -1))
			{
				return false;
			}
			remain = goods3;
			if (slotIndex < 1)
			{
				return remain == null;
			}
			// TODO(decomp): result of the pull+push branch looks inverted in the binary (returns false on success); kept literally @0x18060AD30
			if (SafeSlots.TryPullSlot(slotIndex2, out var pullGoods))
			{
				if (!Slots.TryPushGoods(pullGoods, slotIndex))
				{
					return remain == null;
				}
			}
			return false;
		}
	}

	public override bool TryPullGoods(Goods goods, out Goods[] pullGoods)
	{
		if (Slots.TryPullGoods(goods, out pullGoods))
		{
			return true;
		}
		return SafeSlots.TryPullGoods(goods, out pullGoods);
	}

	public bool TryPullSlotGoods(EGoodsSlotCategory slotCategory, int slotIndex, long count, out Goods pullGoods)
	{
		IDictionary<int, GoodsSlot> dictionary;
		switch (slotCategory)
		{
		case EGoodsSlotCategory.InventorySlot:
			dictionary = Slots;
			break;
		case EGoodsSlotCategory.SafeSlot:
			dictionary = SafeSlots;
			break;
		default:
			pullGoods = null;
			return false;
		}
		return dictionary.TryPullSlotGoods(slotIndex, count, out pullGoods);
	}

	public GoodsSlot GetGoodsSlot(EGoodsSlotCategory slotCategory, int slotIndex)
	{
		GoodsSlot value;
		switch (slotCategory)
		{
		case EGoodsSlotCategory.InventorySlot:
			if (!Slots.TryGetValue(slotIndex, out value))
			{
				return null;
			}
			return value;
		case EGoodsSlotCategory.SafeSlot:
			if (!SafeSlots.TryGetValue(slotIndex, out value))
			{
				return null;
			}
			return value;
		default:
			return null;
		}
	}

	public bool TryConsumeGoods(GoodsData goodsData, out GoodsData remainGoodsData)
	{
		remainGoodsData = GoodsData.Empty();
		long goodsCount = GetGoodsCount(goodsData.Type, goodsData.Id);
		if (goodsCount < goodsData.Count)
		{
			if (goodsCount > 0)
			{
				ConsumeGoods(new GoodsData
				{
					Type = goodsData.Type,
					Id = goodsData.Id,
					Count = goodsCount
				});
			}
			remainGoodsData = new GoodsData
			{
				Type = goodsData.Type,
				Id = goodsData.Id,
				Count = goodsData.Count - goodsCount
			};
			return false;
		}
		ConsumeGoods(new GoodsData
		{
			Type = goodsData.Type,
			Id = goodsData.Id,
			Count = goodsData.Count
		});
		return true;
	}

	public bool TryConsumeSlotGoods(EGoodsSlotCategory slotCategory, int slotIndex, Goods consumeGoods)
	{
		GoodsSlot goodsSlot = GetGoodsSlot(slotCategory, slotIndex);
		if (goodsSlot != null && goodsSlot.Type == consumeGoods.Type && goodsSlot.Id == consumeGoods.Id && goodsSlot.Count >= consumeGoods.Count)
		{
			long count = consumeGoods.Count;
			IDictionary<int, GoodsSlot> dictionary;
			switch (slotCategory)
			{
			case EGoodsSlotCategory.InventorySlot:
				dictionary = Slots;
				break;
			case EGoodsSlotCategory.SafeSlot:
				dictionary = SafeSlots;
				break;
			default:
				return false;
			}
			Goods pullGoods;
			return dictionary.TryPullSlotGoods(slotIndex, count, out pullGoods);
		}
		return false;
	}

	private void OnSlotsChanged(IList<GoodsSlotChangeData> changeDatas)
	{
		foreach (GoodsSlotChangeData changeData in changeDatas)
		{
			if (changeData.Operation == EDataOperation.Add)
			{
				PlayerSlots.Add(changeData.Key, changeData.NewSlot.Clone());
			}
			else if (changeData.Operation == EDataOperation.Remove)
			{
				PlayerSlots.Remove(changeData.Key);
			}
			else if (changeData.Operation == EDataOperation.Replace)
			{
				PlayerSlots[changeData.Key] = changeData.NewSlot.Clone();
			}
		}
	}

	public long GetAddableItemCount(int itemId)
	{
		int inventoryItemCountMax = Player.GetInventoryItemCountMax(itemId);
		GoodsSlot goodsSlot = Slots.FindItemSlot(itemId);
		long num = ((goodsSlot == null) ? 0 : goodsSlot.Count);
		goodsSlot = SafeSlots.FindItemSlot(itemId);
		long num2 = 0L;
		if (goodsSlot != null)
		{
			num2 = goodsSlot.Count;
		}
		return Math.Max((long)inventoryItemCountMax - (num2 + num), 0L);
	}

	public long GetAddableCountInBagStacks(int itemId)
	{
		int inventoryItemCountMax = Player.GetInventoryItemCountMax(itemId);
		long num = Slots.GetAddableCountInBagStacks(itemId, inventoryItemCountMax);
		return SafeSlots.GetAddableCountInBagStacks(itemId, inventoryItemCountMax) + num;
	}

	public long GetAddableItemCountWithBags(int itemId)
	{
		long num = ((!Slots.HasItem(itemId) && !SafeSlots.HasItem(itemId) && IsSlotFull() && IsSafeSlotFull()) ? 0 : GetAddableItemCount(itemId));
		return GetAddableCountInBagStacks(itemId) + num;
	}

	public long GetAddableInventoryItemCount(int itemId)
	{
		if (!Slots.HasItem(itemId) && IsSlotFull())
		{
			return 0L;
		}
		int inventoryItemCountMax = Player.GetInventoryItemCountMax(itemId);
		GoodsSlot goodsSlot = Slots.FindItemSlot(itemId);
		long num = ((goodsSlot == null) ? 0 : goodsSlot.Count);
		return Math.Max(inventoryItemCountMax - num, 0L);
	}

	private int GetInventoryItemCountMax(int itemId)
	{
		return Player.GetInventoryItemCountMax(itemId);
	}

	private async UniTask OnStageEndedAsync()
	{
		await UniTask.DelayFrame(1);
		List<Goods> lostGoods = new List<Goods>();
		if (_characterModel.State == EActionState.Dead)
		{
			Character character = Player.Character;
			List<Goods> list = Slots.ApplyStageDead(GameContext.RoomDifficulty);
			var (character2, array) = character.ApplyStageDead(GameContext.RoomDifficulty);
			if (list != null && list.Count > 0)
			{
				lostGoods.AddRange(list);
			}
			if (array != null && array.Length > 0)
			{
				lostGoods.AddRange(array);
				Player.Character = character2;
			}
		}
		Dictionary<int, long> highGradeGoodsCount = new Dictionary<int, long>();
		HashSet<int> hashSet = new HashSet<int>(_initHighGradeGoodsCache.Select((Goods g) => g.Id));
		Dictionary<int, long> dictionary = new Dictionary<int, long>();
		foreach (KeyValuePair<int, GoodsSlot> slot in Slots)
		{
			GoodsSlot value = slot.Value;
			if (value != null && !value.IsEmpty && value.Id > 0 && TItem.Table.TryGetValue(value.Id, out var value2) && value2.Grade > EGoodsGradeType.Elite)
			{
				if (!dictionary.TryAdd(value.Id, value.Count))
				{
					dictionary[value.Id] += value.Count;
				}
			}
		}
		foreach (KeyValuePair<int, long> item in dictionary)
		{
			if (!hashSet.Contains(item.Key))
			{
				highGradeGoodsCount[item.Key] = item.Value;
			}
		}
		Goods[] highGradeGoods = highGradeGoodsCount.Select((KeyValuePair<int, long> kv) => GoodsCreator.MakeItemGoods(kv.Key, kv.Value)).ToArray();
		Goods[] lostGoodsArray = lostGoods.ToArray();
		GamePlayer player = Player;
		int[] array2 = Array.Empty<int>();
		int[] array3 = Array.Empty<int>();
		if (GameManager.StageResultInfo != null && GameManager.StageResultInfo.PlayerInfos != null)
		{
			int[] array4 = GameManager.StageResultInfo.PlayerInfos.FirstOrDefault((StageResultPlayerInfo pi) => pi.ClientId == ClientId)?.InitialParadoxIds ?? Array.Empty<int>();
			int[] array5 = ((player != null) ? player.GetParadoxIds() : null) ?? Array.Empty<int>();
			array2 = array5.Except(array4).ToArray();
			array3 = array4.Except(array5).ToArray();
		}
		StageResultPlayerInfo playerInfo = new StageResultPlayerInfo
		{
			ClientId = ClientId,
			State = _characterModel.State,
			CharacterId = ((player != null && player.Character != null) ? player.Character.Id : 0),
			UserName = player?.Name,
			IsLostItem = lostGoodsArray.Length > 0,
			HighGradeGoods = highGradeGoods,
			LostGoods = lostGoodsArray,
			GainedParadoxIds = array2,
			RemovedParadoxIds = array3
		};
		GameHost.SendToAll(new NtfStageResultPlayerInfo
		{
			PlayerInfo = playerInfo
		});
	}

	private void InitSafeSlots(ICollection<GoodsSlot> goodsSlots)
	{
		GameManager.AssertIfNotHost();
		_trackableSafeSlots.Clear();
		_safeSlots.Clear();
		if (goodsSlots == null)
		{
			return;
		}
		foreach (GoodsSlot goodsSlot in goodsSlots)
		{
			_trackableSafeSlots.Add(goodsSlot.Key, goodsSlot);
			_safeSlots.Add(goodsSlot.Key, goodsSlot);
		}
		_trackableSafeSlots.ClearDirty();
	}

	private void InitSafeInventoryEvents()
	{
		if (GameManager.IsHost)
		{
			OnSafeInventoryChanged.Subscribe(OnSafeSlotsChanged).AddTo(this);
			GameHost.Receive<ReqSyncSafeInventory>().Where((ReqSyncSafeInventory _) => _.NetworkObjectId == NetworkBase.NetworkObjectId).Subscribe(OnSyncSafeInventory)
				.AddTo(this);
			return;
		}
		GameChannel.Receive<ResSyncSafeInventory>().Where((ResSyncSafeInventory _) => _.NetworkObjectId == NetworkBase.NetworkObjectId).Subscribe(OnResSyncSafeInventory)
			.AddTo(this);
		GameChannel.Receive<NtfUpdateSafeInventory>().Where((NtfUpdateSafeInventory _) => _.NetworkObjectId == NetworkBase.NetworkObjectId).Subscribe(OnUpdateSafeInventory)
			.AddTo(this);
		GameChannel.SendToHost(new ReqSyncSafeInventory
		{
			NetworkObjectId = NetworkBase.NetworkObjectId
		});
	}

	private GoodsSlot[] FlushSafeSlots()
	{
		GameManager.AssertIfNotHost();
		return _trackableSafeSlots.Flush();
	}

	public bool IsSafeSlotFull()
	{
		return SafeSlots.Count >= Player.SafeSlotCount;
	}

	public bool CanAddGoodsToSafeSlot(Goods goods)
	{
		if (goods.IsEquipment() && IsSafeSlotFull())
		{
			return false;
		}
		if (goods.IsBag() && IsSafeSlotFull())
		{
			return false;
		}
		if (!goods.IsItem())
		{
			return true;
		}
		if (GetAddableItemCount(goods.Id) > 0 && (SafeSlots.HasItem(goods.Id) || !IsSafeSlotFull()))
		{
			return true;
		}
		if (GetAddableCountInBagStacks(goods.Id) <= 0)
		{
			return false;
		}
		return true;
	}

	private void ApplySafeSlots(GoodsSlot[] slots)
	{
		List<GoodsSlotChangeData> changeDatas = new List<GoodsSlotChangeData>();
		foreach (GoodsSlot slot in slots)
		{
			GoodsSlot value = null;
			if (slot.Operation == EDataOperation.Remove)
			{
				_safeSlots.TryGetValue(slot.Key, out value);
				_safeSlots.Remove(slot.Key);
			}
			GoodsSlot newSlot = null;
			if (slot.Operation == EDataOperation.Add)
			{
				_safeSlots.Add(slot.Key, slot);
				newSlot = slot;
			}
			if (slot.Operation == EDataOperation.Replace)
			{
				_safeSlots.TryGetValue(slot.Key, out value);
				_safeSlots[slot.Key] = slot;
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
		OnSafeInventoryChanged.OnNext(changeDatas);
	}

	private void OnSafeSlotsChanged(IList<GoodsSlotChangeData> changeDatas)
	{
		foreach (GoodsSlotChangeData changeData in changeDatas)
		{
			if (changeData.Operation == EDataOperation.Add)
			{
				PlayerSafeSlots.Add(changeData.Key, changeData.NewSlot.Clone());
			}
			else if (changeData.Operation == EDataOperation.Remove)
			{
				PlayerSafeSlots.Remove(changeData.Key);
			}
			else if (changeData.Operation == EDataOperation.Replace)
			{
				PlayerSafeSlots[changeData.Key] = changeData.NewSlot.Clone();
			}
		}
	}

	public void PushGoodsToSafeSlot(Goods goods, int slotIndex)
	{
		GameManager.AssertIfNotHost();
		if (goods == null)
		{
			return;
		}
		if (slotIndex < 0)
		{
			goods = StackIntoExistingBags(goods);
		}
		if (goods == null)
		{
			return;
		}
		EGoodsCategory category = goods.Category;
		if (category == EGoodsCategory.Item)
		{
			PushItem();
			return;
		}
		string message;
		if (category == EGoodsCategory.Equipment)
		{
			if (IsSafeSlotFull())
			{
				message = "Safe slot is full; cannot add equipment.";
			}
			else
			{
				if (SafeSlots.TryPushGoods(goods, slotIndex))
				{
					return;
				}
				message = "Failed to push equipment into safe slot.";
			}
		}
		else
		{
			if (category != EGoodsCategory.Bag)
			{
				return;
			}
			if (IsSafeSlotFull())
			{
				message = "Safe slot is full; cannot add bag.";
			}
			else
			{
				if (SafeSlots.TryPushGoods(goods, slotIndex))
				{
					return;
				}
				message = "Failed to push bag into safe slot.";
			}
		}
		Logger.Error(message);
		void PushItem()
		{
			GoodsSlot goodsSlot = Slots.FindItemSlot(goods.Id);
			if (goodsSlot == null)
			{
				if (SafeSlots.FindItemSlot(goods.Id) == null && IsSafeSlotFull())
				{
					Logger.Error("No available slot to add item to safe inventory.");
					return;
				}
				long addableItemCount = GetAddableItemCount(goods.Id);
				if (addableItemCount < goods.Count)
				{
					Logger.Error("Attempted to add more items than acceptable for safe slot.");
					return;
				}
				long num = Math.Min(goods.Count, goods.BaseCount);
				Goods goods2 = GoodsCreator.MakeItemGoods(goods.Id, num, goods.Count - num);
				SafeSlots.TryPushGoods(goods2, slotIndex);
				return;
			}
			int slotIndex2 = goodsSlot.SlotIndex;
			if (goods.Count > 0 && !Slots.TryPushGoods(goods, -1))
			{
				Logger.Error("Failed to push goods into inventory.");
			}
			else if (slotIndex >= 0)
			{
				if (Slots.TryPullSlot(slotIndex2, out var pullGoods))
				{
					if (!SafeSlots.TryPushGoods(pullGoods, slotIndex))
					{
						Logger.Error(string.Format("Failed to push pulled goods to safe slot {0}.", slotIndex));
					}
				}
				else
				{
					Logger.Error(string.Format("Failed to pull goods from inventory slot {0}.", slotIndex2));
				}
			}
		}
	}

	private void OnSyncSafeInventory(ReqSyncSafeInventory message)
	{
		GameHost.SendToClient(message.SenderId, new ResSyncSafeInventory
		{
			Tick = NetworkManager.ServerTime.Tick,
			NetworkObjectId = message.NetworkObjectId,
			SafeSlots = SafeSlots.Values.ToArray()
		});
	}

	private void OnUpdateSafeInventory(NtfUpdateSafeInventory message)
	{
		if (_syncSafeSlotsTick > 0 && _syncSafeSlotsTick < message.Tick)
		{
			ApplySafeSlots(message.ChangeSafeSlots);
		}
	}

	private void OnResSyncSafeInventory(ResSyncSafeInventory message)
	{
		SafeSlots.Clear();
		foreach (GoodsSlot safeSlot in message.SafeSlots)
		{
			SafeSlots.Add(safeSlot.Key, safeSlot);
		}
		_syncSafeSlotsTick = message.Tick;
	}
}
