using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Table;
using GameCommon.Data;
using GameCommon.Enum;
using UniRx;
using VContainer;

public class PlayerContext : NativeBase, IDisposable
{
	[Inject]
	private GameEventSystem _eventSystem;

	private CompositeDisposable _disposable = new CompositeDisposable();

	public readonly ReactiveProperty<Profile> RxProfile = new ReactiveProperty<Profile>();

	public readonly ReactiveDictionary<int, Character> RxCharacters = new ReactiveDictionary<int, Character>();

	public readonly ReactiveProperty<Talent> RxTalent = new ReactiveProperty<Talent>();

	public readonly ReactiveDictionary<int, GoodsSlot> RxInventorySlots = new ReactiveDictionary<int, GoodsSlot>();

	public readonly ReactiveDictionary<int, GoodsSlot> RxSafeInventorySlots = new ReactiveDictionary<int, GoodsSlot>();

	public readonly ReactiveDictionary<int, GoodsSlot> RxStorageSlots = new ReactiveDictionary<int, GoodsSlot>();

	public readonly ReactiveDictionary<int, LostGoods> RxLostGoods = new ReactiveDictionary<int, LostGoods>();

	public readonly ReactiveDictionary<int, GoodsSlot> RxStashSlots = new ReactiveDictionary<int, GoodsSlot>();

	public readonly ReactiveCollection<LostItemSelectInfo> RxSelectLostGoodsInfos = new ReactiveCollection<LostItemSelectInfo>();

	public readonly ReactiveCollection<int> RxQuickSlotItemIds = new ReactiveCollection<int>();

	public readonly Dictionary<int, LoungeQuest> LoungeQuests = new Dictionary<int, LoungeQuest>();

	public readonly Dictionary<int, PlayerStore> PlayerStores = new Dictionary<int, PlayerStore>();

	public readonly Dictionary<int, MonsterRecord> MonsterRecords = new Dictionary<int, MonsterRecord>();

	public readonly Dictionary<int, Employee> Employees = new Dictionary<int, Employee>();

	public readonly Dictionary<int, SalesSlot> SalesSlots = new Dictionary<int, SalesSlot>();

	public readonly Dictionary<int, Employee> RecruitableEmployees = new Dictionary<int, Employee>();

	public readonly Dictionary<int, int> FiredEmployeeCooldowns = new Dictionary<int, int>();

	public readonly Dictionary<long, DialogReplayRecord> DialogReplayRecords = new Dictionary<long, DialogReplayRecord>();

	private bool _isInventoryDirty;

	private bool _isSafeInventoryDirty;

	private bool _isStashDirty;

	private bool _isStorageDirty;

	private bool _isLostItemDirty;

	public BitFlags ItemCollectionFlags;

	public BitFlags EquipmentCollectionFlags;

	public BitFlags ItemMarkFlags;

	public BitFlags EquipmentMarkFlags;

	public BitFlags BagMarkFlags;

	public BitFlags ClearedCampaignFlags;

	public BitFlags ClearedMissionFlags;

	public BitFlags ItemUsedFlags;

	public BitFlags ClearedLoungeQuestFlags;

	public BitFlags NpcUnlockedFlags;

	public BitFlags NpcFunctionUnlockedFlags;

	public BitFlags FeatureUnlockedFlags;

	public BitFlags UnlockedSkinFlags;

	public BitFlags ViewedNpcTalkFlags;

	public BitFlags AreaFlags;

	public BitFlags AreaDecorationUnlockedFlags;

	public BitFlags TycoonUnlockFlags;

	public BitFlags LoungeBgmUnlockedFlags;

	public readonly Dictionary<int, int> ActiveAreaDecorations = new Dictionary<int, int>();

	public readonly Dictionary<int, int> NpcLevels = new Dictionary<int, int>();

	public bool Initialized { get; private set; }

	public string AccountId { get; set; }

	public AppVersion Version { get; private set; }

	public long SaveTimeTicks { get; set; }

	public long CreateTimeTicks { get; private set; }

	public int SlotIndex { get; private set; }

	public long PlayTimeTicks { get; set; }

	public TimeSpan PlayTime => TimeSpan.FromTicks(PlayTimeTicks);

	public ERoomDifficulty Difficulty { get; set; }

	public ERoomDifficulty LastPlayedRoomDifficulty { get; set; }

	public bool PreventRepairMaxDurabilityLoss { get; set; }

	public Subject<Unit> OnInventoryChanged { get; } = new Subject<Unit>();

	public Subject<Unit> OnStashChanged { get; } = new Subject<Unit>();

	public Subject<Unit> OnSafeInventoryChanged { get; } = new Subject<Unit>();

	public Subject<Goods> OnUpdateAcquisition { get; } = new Subject<Goods>();

	public Subject<Unit> OnOwnedGoodsChanged { get; } = new Subject<Unit>();

	public Subject<Unit> OnStorageChanged { get; } = new Subject<Unit>();

	public Subject<Unit> OnLostItemChanged { get; } = new Subject<Unit>();

	public bool HasDirty { get; private set; }

	public PlayerContext(PlayerContextData data)
	{
		AccountId = data.AccountId;
		Version = data.Version;
		RxProfile.Value = data.Profile;
		CreateTimeTicks = data.CreateTimeTicks;
		SlotIndex = data.SlotIndex;
		PlayTimeTicks = data.PlayTimeTicks;
		SaveTimeTicks = data.SaveTimeTicks;
		RxTalent.Value = data.Talent;
		Difficulty = data.Difficulty;
		LastPlayedRoomDifficulty = data.LastPlayedRoomDifficulty;
		PreventRepairMaxDurabilityLoss = data.PreventRepairMaxDurabilityLoss;
		data.Characters?.ForEach(delegate(Character x)
		{
			RxCharacters.Add(x.Id, x);
		});
		data.StashSlots?.ForEach(delegate(GoodsSlot x)
		{
			RxStashSlots.Add(x.SlotIndex, x);
		});
		data.InventorySlots?.ForEach(delegate(GoodsSlot x)
		{
			RxInventorySlots.Add(x.SlotIndex, x);
		});
		data.SafeInventorySlots?.ForEach(delegate(GoodsSlot x)
		{
			RxSafeInventorySlots.Add(x.SlotIndex, x);
		});
		data.StorageSlots?.ForEach(delegate(GoodsSlot x)
		{
			RxStorageSlots.Add(x.SlotIndex, x);
		});
		data.QuickSlotItemIds?.ForEach(delegate(int x)
		{
			RxQuickSlotItemIds.Add(x);
		});
		data.SelectedLostItems?.ForEach(delegate(LostItemSelectInfo x)
		{
			RxSelectLostGoodsInfos.Add(x);
		});
		data.LostGoods?.ForEach(delegate(LostGoods x)
		{
			RxLostGoods.Add(x.SlotIndex, x);
		});
		data.LoungeQuests?.ForEach(delegate(LoungeQuest x)
		{
			LoungeQuests.Add(x.Id, x);
		});
		ItemCollectionFlags = data.ItemCollectionFlags;
		EquipmentCollectionFlags = data.EquipmentCollectionFlags;
		ItemMarkFlags = data.ItemMarkFlags ?? new BitFlags(TItem.Table.Values.Max((TItem x) => x.BitFlagIndex));
		EquipmentMarkFlags = data.EquipmentMarkFlags ?? new BitFlags(TEquipment.Table.Values.Max((TEquipment x) => x.BitFlagIndex));
		BagMarkFlags = data.BagMarkFlags ?? new BitFlags(TBag.Table.Values.Max((TBag x) => x.BitFlagIndex));
		ClearedCampaignFlags = data.ClearedCampaignFlags;
		ClearedMissionFlags = data.ClearedMissionFlags;
		ItemUsedFlags = data.ItemUsedFlags;
		ClearedLoungeQuestFlags = data.CompletedLoungeQuestFlags;
		NpcUnlockedFlags = data.NpcUnlockedFlags;
		NpcFunctionUnlockedFlags = data.NpcFunctionUnlockedFlags;
		FeatureUnlockedFlags = data.FeatureUnlockedFlags;
		UnlockedSkinFlags = data.UnlockedSkinFlags;
		AreaFlags = data.AreaFlags;
		AreaDecorationUnlockedFlags = data.AreaDecorationUnlockedFlags;
		ActiveAreaDecorations.Clear();
		data.ActiveAreaDecorations?.ForEach(delegate(ActiveAreaDecoration x)
		{
			ActiveAreaDecorations[x.GroupId] = x.AreaDecorationSettingId;
		});
		TycoonUnlockFlags = data.TycoonUnlockFlags;
		ViewedNpcTalkFlags = data.ViewedNpcTalkFlags;
		LoungeBgmUnlockedFlags = data.LoungeBgmUnlockedFlags;
		NpcLevels.Clear();
		data.NpcLevels?.ForEach(delegate(NpcLevelRecord x)
		{
			NpcLevels[x.NpcId] = x.Level;
		});
		data.PlayerStores?.ForEach(delegate(PlayerStore x)
		{
			PlayerStores.Add(x.Id, x);
		});
		data.MonsterRecords?.ForEach(delegate(MonsterRecord x)
		{
			MonsterRecords.Add(x.MonsterGroupId, x);
		});
		data.Employees?.ForEach(delegate(Employee x)
		{
			Employees.Add(x.Id, x);
		});
		data.SalesSlots?.ForEach(delegate(SalesSlot x)
		{
			SalesSlots.Add(x.Id, x);
		});
		data.RecruitableEmployees?.ForEach(delegate(Employee x)
		{
			RecruitableEmployees.Add(x.Id, x);
		});
		data.FiredEmployeeRecords?.ForEach(delegate(FiredEmployeeRecord x)
		{
			FiredEmployeeCooldowns[x.EmployeeId] = x.RemainingCooldown;
		});
		data.DialogReplayRecords?.ForEach(delegate(DialogReplayRecord x)
		{
			DialogReplayRecords.Add(x.Id, x);
		});
		RxStashSlots.ObserveAdd().Subscribe(delegate(DictionaryAddEvent<int, GoodsSlot> e)
		{
			OnUpdateAcquisitions(e.Value);
		}).AddTo(_disposable);
		RxStashSlots.ObserveReplace().Subscribe(delegate(DictionaryReplaceEvent<int, GoodsSlot> e)
		{
			OnUpdateAcquisitions(e.NewValue);
		}).AddTo(_disposable);
		RxInventorySlots.ObserveAdd().Subscribe(delegate(DictionaryAddEvent<int, GoodsSlot> e)
		{
			OnUpdateAcquisitions(e.Value);
		}).AddTo(_disposable);
		RxInventorySlots.ObserveReplace().Subscribe(delegate(DictionaryReplaceEvent<int, GoodsSlot> e)
		{
			OnUpdateAcquisitions(e.NewValue);
		}).AddTo(_disposable);
		RxStashSlots.ObserveChange().Subscribe(delegate
		{
			_isStashDirty = true;
			HasDirty = true;
		}).AddTo(_disposable);
		RxStorageSlots.ObserveChange().Subscribe(delegate
		{
			_isStorageDirty = true;
			HasDirty = true;
		}).AddTo(_disposable);
		RxLostGoods.ObserveChange().Subscribe(delegate
		{
			_isLostItemDirty = true;
			HasDirty = true;
		}).AddTo(_disposable);
		RxProfile.Pairwise().Subscribe(delegate(Pair<Profile> pair)
		{
			OnUserChanged(pair.Previous, pair.Current);
		}).AddTo(_disposable);
		HasDirty = false;
		Initialized = true;
	}

	public void Dispose()
	{
		_disposable?.Dispose();
	}

	public void ApplyInventorySlots(GoodsSlot[] inventorySlots)
	{
		if (inventorySlots != null)
		{
			inventorySlots.ForEach(delegate(GoodsSlot slot)
			{
				ApplySlot(RxInventorySlots, slot);
			});
			_isInventoryDirty = true;
			HasDirty = true;
		}
	}

	public void ApplySafeInventorySlots(GoodsSlot[] safeInventorySlots)
	{
		if (safeInventorySlots != null)
		{
			safeInventorySlots.ForEach(delegate(GoodsSlot slot)
			{
				ApplySlot(RxSafeInventorySlots, slot);
			});
			_isSafeInventoryDirty = true;
			HasDirty = true;
		}
	}

	private static void ApplySlot(ReactiveDictionary<int, GoodsSlot> slots, GoodsSlot slot)
	{
		if (slot == null)
		{
			return;
		}
		if (slot.Operation == Tracker.EDataOperation.Remove)
		{
			slots.Remove(slot.SlotIndex);
		}
		if (slot.Operation == Tracker.EDataOperation.Add)
		{
			slots.Add(slot.SlotIndex, slot);
		}
		if (slot.Operation == Tracker.EDataOperation.Replace)
		{
			slots[slot.SlotIndex] = slot;
		}
	}

	private void OnUserChanged(Profile oldValue, Profile newValue)
	{
		if (oldValue != null && newValue != null)
		{
			if (oldValue.Level != newValue.Level)
			{
				_eventSystem.Trigger(new LevelEvent(newValue.Level));
			}
			HasDirty = true;
		}
	}

	private void OnUpdateAcquisitions(GoodsSlot goodsSlot)
	{
		if (goodsSlot != null && goodsSlot.Goods != null)
		{
			OnUpdateAcquisition.OnNext(goodsSlot.Goods);
		}
	}

	public void Flush()
	{
		bool flag = _isStashDirty;
		if (_isInventoryDirty)
		{
			_isInventoryDirty = false;
			OnInventoryChanged.OnNext(Unit.Default);
			flag = true;
		}
		if (_isSafeInventoryDirty)
		{
			_isSafeInventoryDirty = false;
			OnSafeInventoryChanged.OnNext(Unit.Default);
		}
		if (_isStashDirty)
		{
			_isStashDirty = false;
			OnStashChanged.OnNext(Unit.Default);
		}
		if (_isStorageDirty)
		{
			_isStorageDirty = false;
			OnStorageChanged.OnNext(Unit.Default);
		}
		if (_isLostItemDirty)
		{
			_isLostItemDirty = false;
			OnLostItemChanged.OnNext(Unit.Default);
		}
		if (flag)
		{
			OnOwnedGoodsChanged.OnNext(Unit.Default);
		}
	}

	public void ClearDirty()
	{
		HasDirty = false;
	}

	public PlayerContextData ToData()
	{
		return new PlayerContextData
		{
			AccountId = AccountId,
			SlotIndex = SlotIndex,
			Version = Version,
			Profile = RxProfile.Value?.Clone(),
			Talent = RxTalent.Value?.Clone(),
			Characters = RxCharacters.Values.Select((Character x) => x.Clone()).ToArray(),
			StashSlots = RxStashSlots.Values.Select((GoodsSlot x) => x.Clone()).ToArray(),
			InventorySlots = RxInventorySlots.Values.Select((GoodsSlot x) => x.Clone()).ToArray(),
			SafeInventorySlots = RxSafeInventorySlots.Values.Select((GoodsSlot x) => x.Clone()).ToArray(),
			StorageSlots = RxStorageSlots.Values.Select((GoodsSlot x) => x.Clone()).ToArray(),
			ItemCollectionFlags = ItemCollectionFlags.Clone(),
			EquipmentCollectionFlags = EquipmentCollectionFlags.Clone(),
			ItemMarkFlags = ItemMarkFlags.Clone(),
			EquipmentMarkFlags = EquipmentMarkFlags.Clone(),
			BagMarkFlags = BagMarkFlags.Clone(),
			ClearedCampaignFlags = ClearedCampaignFlags.Clone(),
			ClearedMissionFlags = ClearedMissionFlags.Clone(),
			QuickSlotItemIds = RxQuickSlotItemIds.ToArray(),
			SelectedLostItems = RxSelectLostGoodsInfos.Select((LostItemSelectInfo x) => x.Clone()).ToArray(),
			PlayerStores = PlayerStores.Values.ToArray(),
			ItemUsedFlags = ItemUsedFlags.Clone(),
			Difficulty = Difficulty,
			LastPlayedRoomDifficulty = LastPlayedRoomDifficulty,
			PreventRepairMaxDurabilityLoss = PreventRepairMaxDurabilityLoss,
			LostGoods = RxLostGoods.Values.Select((LostGoods x) => x.Clone()).ToArray(),
			CompletedLoungeQuestFlags = ClearedLoungeQuestFlags.Clone(),
			NpcUnlockedFlags = NpcUnlockedFlags.Clone(),
			NpcFunctionUnlockedFlags = NpcFunctionUnlockedFlags.Clone(),
			FeatureUnlockedFlags = FeatureUnlockedFlags.Clone(),
			UnlockedSkinFlags = UnlockedSkinFlags.Clone(),
			LoungeQuests = LoungeQuests.Values.Select((LoungeQuest x) => x.Clone()).ToArray(),
			MonsterRecords = MonsterRecords.Values.ToArray(),
			Employees = Employees.Values.Select((Employee x) => x.Clone()).ToArray(),
			SalesSlots = SalesSlots.Values.Select((SalesSlot x) => x.Clone()).ToArray(),
			RecruitableEmployees = RecruitableEmployees.Values.Select((Employee x) => x.Clone()).ToArray(),
			FiredEmployeeRecords = FiredEmployeeCooldowns.Select((KeyValuePair<int, int> kv) => new FiredEmployeeRecord
			{
				EmployeeId = kv.Key,
				RemainingCooldown = kv.Value
			}).ToArray(),
			DialogReplayRecords = DialogReplayRecords.Values.Select((DialogReplayRecord x) => x.Clone()).ToArray(),
			CreateTimeTicks = CreateTimeTicks,
			PlayTimeTicks = PlayTimeTicks,
			AreaFlags = AreaFlags.Clone(),
			AreaDecorationUnlockedFlags = AreaDecorationUnlockedFlags.Clone(),
			ActiveAreaDecorations = ActiveAreaDecorations.Select((KeyValuePair<int, int> kv) => new ActiveAreaDecoration
			{
				GroupId = kv.Key,
				AreaDecorationSettingId = kv.Value
			}).ToArray(),
			TycoonUnlockFlags = TycoonUnlockFlags.Clone(),
			ViewedNpcTalkFlags = ViewedNpcTalkFlags.Clone(),
			LoungeBgmUnlockedFlags = LoungeBgmUnlockedFlags.Clone(),
			NpcLevels = NpcLevels.Select((KeyValuePair<int, int> kv) => new NpcLevelRecord
			{
				NpcId = kv.Key,
				Level = kv.Value
			}).ToArray()
		};
	}
}
