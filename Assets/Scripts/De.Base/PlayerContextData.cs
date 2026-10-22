using GameCommon.Data;
using GameCommon.Enum;
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
		return null;
	}

	public void Normalize()
	{
	}

	private void NormalizeCharacters()
	{
	}

	private void NormalizeAreaFlags()
	{
	}

	private void NormalizeAreaDecorations()
	{
	}
}
