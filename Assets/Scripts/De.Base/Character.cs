using System;
using System.Linq;
using ProtoBuf;
using Tracker;

[ProtoContract]
public class Character : ITrackableDictionaryData<int, Character>, ITrackableData<Character>
{
	public int Key => Id;

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

	public GoodsSlot ActiveWeaponSlot => WeaponSlots[ActiveWeaponIndex];

	public GoodsSlot SubWeaponSlot => WeaponSlots[ActiveWeaponIndex == 0 ? 1 : 0];

	public int WeaponId => WeaponSlots[ActiveWeaponIndex].Goods?.Id ?? 0;

	public int[] AccessoryIds => AccessorySlots.Select((GoodsSlot s) => s.Goods?.Id ?? 0).ToArray();

	public Character Clone()
	{
		return new Character
		{
			Operation = Operation,
			Id = Id,
			SelectedActiveSkillIds = SelectedActiveSkillIds?.ToArray(),
			WeaponSlots = WeaponSlots?.Select((GoodsSlot s) => s.Clone()).ToArray(),
			ActiveWeaponIndex = ActiveWeaponIndex,
			AccessorySlots = AccessorySlots?.Select((GoodsSlot s) => s.Clone()).ToArray(),
			ArtifactSlots = ArtifactSlots?.Select((GoodsSlot s) => s.Clone()).ToArray(),
			SkinPresetId = SkinPresetId,
			PositiveParadoxIds = (PositiveParadoxIds?.ToArray() ?? Array.Empty<int>()),
			NegativeParadoxIds = (NegativeParadoxIds?.ToArray() ?? Array.Empty<int>()),
			LockedParadoxId = LockedParadoxId,
			AttackSkillId = AttackSkillId,
			DashSkillId = DashSkillId,
			LockedActiveSkillSlots = LockedActiveSkillSlots?.ToArray(),
			EquipmentActiveSkillId = EquipmentActiveSkillId
		};
	}
}
