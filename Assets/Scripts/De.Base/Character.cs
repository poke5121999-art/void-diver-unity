using ProtoBuf;
using Tracker;

[ProtoContract]
public class Character : ITrackableDictionaryData<int, Character>, ITrackableData<Character>
{
	public int Key => 0;

	[ProtoMember(1)]
	public EDataOperation Operation { get; set; }

	[ProtoMember(2)]
	public int Id { get; set; }

	[ProtoMember(3)]
	public int[] SelectedActiveSkillIds { get; set; }

	[ProtoMember(4)]
	public GoodsSlot[] WeaponSlots { get; set; }

	[ProtoMember(5)]
	public GoodsSlot[] AccessorySlots { get; set; }

	[ProtoMember(6)]
	public GoodsSlot[] ArtifactSlots { get; set; }

	[ProtoMember(7)]
	public int SkinPresetId { get; set; }

	[ProtoMember(8)]
	public int[] PositiveParadoxIds { get; set; }

	[ProtoMember(9)]
	public int[] NegativeParadoxIds { get; set; }

	[ProtoMember(10)]
	public int LockedParadoxId { get; set; }

	[ProtoMember(11)]
	public int AttackSkillId { get; set; }

	[ProtoMember(12)]
	public int DashSkillId { get; set; }

	[ProtoMember(13)]
	public bool[] LockedActiveSkillSlots { get; set; }

	[ProtoMember(14)]
	public int ActiveWeaponIndex { get; set; }

	[ProtoMember(15)]
	public int EquipmentActiveSkillId { get; set; }

	public GoodsSlot ActiveWeaponSlot => null;

	public GoodsSlot SubWeaponSlot => null;

	public int WeaponId => 0;

	public int[] AccessoryIds => null;

	public Character Clone()
	{
		return null;
	}
}
