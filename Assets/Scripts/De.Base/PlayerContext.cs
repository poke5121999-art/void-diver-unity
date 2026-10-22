using System;
using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;
using UniRx;
using VContainer;

public class PlayerContext : NativeBase, IDisposable
{
	[Inject]
	private GameEventSystem _eventSystem;

	private CompositeDisposable _disposable;

	public readonly ReactiveProperty<Profile> RxProfile;

	public readonly ReactiveDictionary<int, Character> RxCharacters;

	public readonly ReactiveProperty<Talent> RxTalent;

	public readonly ReactiveDictionary<int, GoodsSlot> RxInventorySlots;

	public readonly ReactiveDictionary<int, GoodsSlot> RxSafeInventorySlots;

	public readonly ReactiveDictionary<int, GoodsSlot> RxStorageSlots;

	public readonly ReactiveDictionary<int, LostGoods> RxLostGoods;

	public readonly ReactiveDictionary<int, GoodsSlot> RxStashSlots;

	public readonly ReactiveCollection<LostItemSelectInfo> RxSelectLostGoodsInfos;

	public readonly ReactiveCollection<int> RxQuickSlotItemIds;

	public readonly Dictionary<int, LoungeQuest> LoungeQuests;

	public readonly Dictionary<int, PlayerStore> PlayerStores;

	public readonly Dictionary<int, MonsterRecord> MonsterRecords;

	public readonly Dictionary<int, Employee> Employees;

	public readonly Dictionary<int, SalesSlot> SalesSlots;

	public readonly Dictionary<int, Employee> RecruitableEmployees;

	public readonly Dictionary<int, int> FiredEmployeeCooldowns;

	public readonly Dictionary<long, DialogReplayRecord> DialogReplayRecords;

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

	public readonly Dictionary<int, int> ActiveAreaDecorations;

	public readonly Dictionary<int, int> NpcLevels;

	public bool Initialized { get; private set; }

	public string AccountId { get; set; }

	public AppVersion Version { get; private set; }

	public long SaveTimeTicks { get; set; }

	public long CreateTimeTicks { get; private set; }

	public int SlotIndex { get; private set; }

	public long PlayTimeTicks { get; set; }

	public TimeSpan PlayTime => default;

	public ERoomDifficulty Difficulty { get; set; }

	public ERoomDifficulty LastPlayedRoomDifficulty { get; set; }

	public bool PreventRepairMaxDurabilityLoss { get; set; }

	public Subject<Unit> OnInventoryChanged { get; }

	public Subject<Unit> OnStashChanged { get; }

	public Subject<Unit> OnSafeInventoryChanged { get; }

	public Subject<Goods> OnUpdateAcquisition { get; }

	public Subject<Unit> OnOwnedGoodsChanged { get; }

	public Subject<Unit> OnStorageChanged { get; }

	public Subject<Unit> OnLostItemChanged { get; }

	public bool HasDirty { get; private set; }

	public PlayerContext(PlayerContextData data)
	{
	}

	public void Dispose()
	{
	}

	public void ApplyInventorySlots(GoodsSlot[] inventorySlots)
	{
	}

	public void ApplySafeInventorySlots(GoodsSlot[] safeInventorySlots)
	{
	}

	private void OnUserChanged(Profile oldValue, Profile newValue)
	{
	}

	private void OnUpdateAcquisitions(GoodsSlot goodsSlot)
	{
	}

	public void Flush()
	{
	}

	public void ClearDirty()
	{
	}

	public PlayerContextData ToData()
	{
		return null;
	}
}
