using GameCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Enum;
using GameCommon.Table;
using ProtoBuf;

[ProtoContract]
public class PlayerContextData : ISaveData
{
	[ProtoMember(1)]
	public string AccountId { get; set; }

	[ProtoMember(2)]
	public AppVersion Version { get; set; }

	[ProtoMember(3)]
	public long SaveTimeTicks { get; set; }

	[ProtoMember(4)]
	public long CreateTimeTicks { get; set; }

	[ProtoMember(5)]
	public int SlotIndex { get; set; }

	[ProtoMember(6)]
	public Profile Profile { get; set; }

	[ProtoMember(7)]
	public Talent Talent { get; set; }

	[ProtoMember(8)]
	public Character[] Characters { get; set; }

	[ProtoMember(9)]
	public GoodsSlot[] StashSlots { get; set; }

	[ProtoMember(10)]
	public GoodsSlot[] InventorySlots { get; set; }

	[ProtoMember(11)]
	public GoodsSlot[] StorageSlots { get; set; }

	[ProtoMember(12)]
	public BitFlags ItemCollectionFlags { get; set; }

	[ProtoMember(13)]
	public BitFlags EquipmentCollectionFlags { get; set; }

	[ProtoMember(14)]
	public BitFlags ItemMarkFlags { get; set; }

	[ProtoMember(15)]
	public BitFlags EquipmentMarkFlags { get; set; }

	[ProtoMember(16)]
	public BitFlags ClearedCampaignFlags { get; set; }

	[ProtoMember(17)]
	public BitFlags ClearedMissionFlags { get; set; }

	[ProtoMember(18)]
	public int[] QuickSlotItemIds { get; set; }

	[ProtoMember(19)]
	public PlayerStore[] PlayerStores { get; set; }

	[ProtoMember(20)]
	public BitFlags ItemUsedFlags { get; set; }

	[ProtoMember(21)]
	public ERoomDifficulty Difficulty { get; set; }

	[ProtoMember(22)]
	public LostGoods[] LostGoods { get; set; }

	[ProtoMember(23)]
	public LostItemSelectInfo[] SelectedLostItems { get; set; }

	[ProtoMember(24)]
	public GoodsSlot[] SafeInventorySlots { get; set; }

	[ProtoMember(25)]
	public BitFlags CompletedLoungeQuestFlags { get; set; }

	[ProtoMember(26)]
	public LoungeQuest[] LoungeQuests { get; set; }

	[ProtoMember(27)]
	public ERoomDifficulty LastPlayedRoomDifficulty { get; set; }

	[ProtoMember(28)]
	public BitFlags NpcUnlockedFlags { get; set; }

	[ProtoMember(29)]
	public MonsterRecord[] MonsterRecords { get; set; }

	[ProtoMember(30)]
	public Employee[] Employees { get; set; }

	[ProtoMember(31)]
	public SalesSlot[] SalesSlots { get; set; }

	[ProtoMember(32)]
	public Employee[] RecruitableEmployees { get; set; }

	[ProtoMember(33)]
	public DialogReplayRecord[] DialogReplayRecords { get; set; }

	[ProtoMember(34)]
	public BitFlags UnlockedSkinFlags { get; set; }

	[ProtoMember(35)]
	public long PlayTimeTicks { get; set; }

	[ProtoMember(36)]
	public BitFlags AreaFlags { get; set; }

	[ProtoMember(37)]
	public BitFlags AreaDecorationUnlockedFlags { get; set; }

	[ProtoMember(38)]
	public ActiveAreaDecoration[] ActiveAreaDecorations { get; set; }

	[ProtoMember(41)]
	public BitFlags TycoonUnlockFlags { get; set; }

	[ProtoMember(42)]
	public FiredEmployeeRecord[] FiredEmployeeRecords { get; set; }

	[ProtoMember(43)]
	public BitFlags ViewedNpcTalkFlags { get; set; }

	[ProtoMember(44)]
	public BitFlags LoungeBgmUnlockedFlags { get; set; }

	[ProtoMember(45)]
	public BitFlags NpcFunctionUnlockedFlags { get; set; }

	[ProtoMember(46)]
	public NpcLevelRecord[] NpcLevels { get; set; }

	[ProtoMember(47)]
	public BitFlags FeatureUnlockedFlags { get; set; }

	[ProtoMember(48)]
	public BitFlags BagMarkFlags { get; set; }

	[ProtoMember(49)]
	public bool PreventRepairMaxDurabilityLoss { get; set; }

	public ISaveData Clone()
	{
		return new PlayerContextData
		{
			AccountId = AccountId,
			SaveTimeTicks = SaveTimeTicks,
			Version = Version?.Clone(),
			SlotIndex = SlotIndex,
			Profile = Profile?.Clone(),
			Talent = Talent?.Clone(),
			Characters = Characters?.Select((Character x) => x.Clone()).ToArray(),
			StashSlots = StashSlots?.Select((GoodsSlot x) => x.Clone()).ToArray(),
			InventorySlots = InventorySlots?.Select((GoodsSlot x) => x.Clone()).ToArray(),
			StorageSlots = StorageSlots?.Select((GoodsSlot x) => x.Clone()).ToArray(),
			PlayerStores = PlayerStores?.Select((PlayerStore x) => x.Clone()).ToArray(),
			LostGoods = LostGoods?.Select((LostGoods x) => x.Clone()).ToArray(),
			SelectedLostItems = SelectedLostItems?.Select((LostItemSelectInfo x) => x.Clone()).ToArray(),
			SafeInventorySlots = SafeInventorySlots?.Select((GoodsSlot x) => x.Clone()).ToArray(),
			LoungeQuests = LoungeQuests?.Select((LoungeQuest x) => x.Clone()).ToArray(),
			MonsterRecords = MonsterRecords?.Select((MonsterRecord x) => x.Clone()).ToArray(),
			Employees = Employees?.Select((Employee x) => x.Clone()).ToArray(),
			SalesSlots = SalesSlots?.Select((SalesSlot x) => x.Clone()).ToArray(),
			RecruitableEmployees = RecruitableEmployees?.Select((Employee x) => x.Clone()).ToArray(),
			DialogReplayRecords = DialogReplayRecords?.Select((DialogReplayRecord x) => x.Clone()).ToArray(),
			ActiveAreaDecorations = ActiveAreaDecorations?.Select((ActiveAreaDecoration x) => x.Clone()).ToArray(),
			FiredEmployeeRecords = FiredEmployeeRecords?.Select((FiredEmployeeRecord x) => x.Clone()).ToArray(),
			NpcLevels = NpcLevels?.Select((NpcLevelRecord x) => x.Clone()).ToArray(),
			ItemCollectionFlags = ItemCollectionFlags?.Clone(),
			EquipmentCollectionFlags = EquipmentCollectionFlags?.Clone(),
			ItemMarkFlags = ItemMarkFlags?.Clone(),
			EquipmentMarkFlags = EquipmentMarkFlags?.Clone(),
			ClearedCampaignFlags = ClearedCampaignFlags?.Clone(),
			ClearedMissionFlags = ClearedMissionFlags?.Clone(),
			ItemUsedFlags = ItemUsedFlags?.Clone(),
			CompletedLoungeQuestFlags = CompletedLoungeQuestFlags?.Clone(),
			NpcUnlockedFlags = NpcUnlockedFlags?.Clone(),
			UnlockedSkinFlags = UnlockedSkinFlags?.Clone(),
			AreaFlags = AreaFlags?.Clone(),
			AreaDecorationUnlockedFlags = AreaDecorationUnlockedFlags?.Clone(),
			TycoonUnlockFlags = TycoonUnlockFlags?.Clone(),
			ViewedNpcTalkFlags = ViewedNpcTalkFlags?.Clone(),
			LoungeBgmUnlockedFlags = LoungeBgmUnlockedFlags?.Clone(),
			NpcFunctionUnlockedFlags = NpcFunctionUnlockedFlags?.Clone(),
			FeatureUnlockedFlags = FeatureUnlockedFlags?.Clone(),
			BagMarkFlags = BagMarkFlags?.Clone(),
			QuickSlotItemIds = (int[])QuickSlotItemIds?.Clone(),
			Difficulty = Difficulty,
			LastPlayedRoomDifficulty = LastPlayedRoomDifficulty,
			PlayTimeTicks = PlayTimeTicks,
			CreateTimeTicks = CreateTimeTicks,
			PreventRepairMaxDurabilityLoss = PreventRepairMaxDurabilityLoss
		};
	}

	public void Normalize()
	{
		NormalizeCharacters();
		NormalizeAreaFlags();
		NormalizeAreaDecorations();
		Characters = Characters ?? Array.Empty<Character>();
		StashSlots = StashSlots ?? Array.Empty<GoodsSlot>();
		InventorySlots = InventorySlots ?? Array.Empty<GoodsSlot>();
		StorageSlots = StorageSlots ?? Array.Empty<GoodsSlot>();
		SafeInventorySlots = SafeInventorySlots ?? Array.Empty<GoodsSlot>();
		QuickSlotItemIds = QuickSlotItemIds ?? Array.Empty<int>();
		PlayerStores = PlayerStores ?? Array.Empty<PlayerStore>();
		LostGoods = LostGoods ?? Array.Empty<LostGoods>();
		SelectedLostItems = SelectedLostItems ?? Array.Empty<LostItemSelectInfo>();
		LoungeQuests = LoungeQuests ?? Array.Empty<LoungeQuest>();
		MonsterRecords = MonsterRecords ?? Array.Empty<MonsterRecord>();
		Employees = Employees ?? Array.Empty<Employee>();
		SalesSlots = SalesSlots ?? Array.Empty<SalesSlot>();
		RecruitableEmployees = RecruitableEmployees ?? Array.Empty<Employee>();
		DialogReplayRecords = DialogReplayRecords ?? Array.Empty<DialogReplayRecord>();
		FiredEmployeeRecords = FiredEmployeeRecords ?? Array.Empty<FiredEmployeeRecord>();
		NpcLevels = NpcLevels ?? Array.Empty<NpcLevelRecord>();
		ItemCollectionFlags = ItemCollectionFlags ?? new BitFlags();
		EquipmentCollectionFlags = EquipmentCollectionFlags ?? new BitFlags();
		ItemMarkFlags = ItemMarkFlags ?? new BitFlags();
		EquipmentMarkFlags = EquipmentMarkFlags ?? new BitFlags();
		BagMarkFlags = BagMarkFlags ?? new BitFlags();
		ClearedCampaignFlags = ClearedCampaignFlags ?? new BitFlags();
		ClearedMissionFlags = ClearedMissionFlags ?? new BitFlags();
		ItemUsedFlags = ItemUsedFlags ?? new BitFlags();
		CompletedLoungeQuestFlags = CompletedLoungeQuestFlags ?? new BitFlags();
		NpcUnlockedFlags = NpcUnlockedFlags ?? new BitFlags();
		NpcFunctionUnlockedFlags = NpcFunctionUnlockedFlags ?? new BitFlags();
		FeatureUnlockedFlags = FeatureUnlockedFlags ?? new BitFlags();
		UnlockedSkinFlags = UnlockedSkinFlags ?? new BitFlags();
		ViewedNpcTalkFlags = ViewedNpcTalkFlags ?? new BitFlags();
		TycoonUnlockFlags = TycoonUnlockFlags ?? new BitFlags();
		LoungeBgmUnlockedFlags = LoungeBgmUnlockedFlags ?? new BitFlags();
	}

	private void NormalizeCharacters()
	{
		if (Characters == null)
		{
			return;
		}
		for (int i = 0; i < Characters.Length; i++)
		{
			Character character = Characters[i];
			if (character == null)
			{
				continue;
			}
			TCharacter tCharacter = TCharacter.Table[character.Id];
			if (character.WeaponSlots == null)
			{
				character.WeaponSlots = new GoodsSlot[2]
				{
					new GoodsSlot
					{
						Goods = GoodsCreator.MakeEquipmentGoods(tCharacter.DefaultWeaponId, 1L, 0L, null, null, null, 1)
					},
					new GoodsSlot
					{
						SlotIndex = 1,
						Goods = null
					}
				};
			}
			character.WeaponSlots = NormalizeSlots(character.WeaponSlots, 2);
			character.AccessorySlots = NormalizeSlots(character.AccessorySlots, TConst.CharacterAccessorySlotCount.IntValue);
			character.ArtifactSlots = NormalizeSlots(character.ArtifactSlots, TConst.CharacterArtifactSlotCount.IntValue);
		}

		static GoodsSlot[] NormalizeSlots(GoodsSlot[] slots, int count)
		{
			GoodsSlot[] array = new GoodsSlot[count];
			for (int j = 0; j < count; j++)
			{
				array[j] = ((slots != null && j < slots.Length) ? slots[j] : new GoodsSlot
				{
					SlotIndex = j,
					Goods = null
				});
			}
			return array;
		}
	}

	private void NormalizeAreaFlags()
	{
		if (AreaFlags == null)
		{
			AreaFlags = new BitFlags(TArea.Table.Values.Max((TArea a) => a.BitFlagIndex));
		}
		foreach (TArea item in TArea.Table.Values.Where((TArea a) => a.DefaultUnlocked))
		{
			AreaFlags.Set(item.BitFlagIndex);
		}
	}

	private void NormalizeAreaDecorations()
	{
		if (AreaDecorationUnlockedFlags == null)
		{
			AreaDecorationUnlockedFlags = new BitFlags(TAreaDecoration.Table.Values.Max((TAreaDecoration d) => d.BitFlagIndex));
		}
		if (ActiveAreaDecorations == null)
		{
			ActiveAreaDecorations = Array.Empty<ActiveAreaDecoration>();
		}
		HashSet<int> hashSet = ActiveAreaDecorations.Select((ActiveAreaDecoration a) => a.GroupId).ToHashSet();
		List<ActiveAreaDecoration> list = new List<ActiveAreaDecoration>();
		foreach (TAreaDecoration item in TAreaDecoration.Table.Values.Where((TAreaDecoration d) => d.Index == 0))
		{
			AreaDecorationUnlockedFlags.Set(item.BitFlagIndex);
			if (!hashSet.Contains(item.GroupId))
			{
				list.Add(new ActiveAreaDecoration
				{
					GroupId = item.GroupId,
					AreaDecorationSettingId = item.Id
				});
			}
		}
		if (list.Count > 0)
		{
			ActiveAreaDecorations = ActiveAreaDecorations.Concat(list).ToArray();
		}
	}
}
