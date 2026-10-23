using System;
using GameCommon.Table;
using ProtoBuf;
using Tracker;

[ProtoContract]
public class UnitSkill : ITrackableDictionaryData<int, UnitSkill>
{
	public int Key => Id;

	[ProtoMember(1)]
	public EDataOperation Operation { get; set; }

	[ProtoMember(2)]
	public int Id { get; set; }

	[ProtoMember(3)]
	public int Stack { get; set; }

	[ProtoMember(4)]
	public float CoolEndTime { get; set; }

	[ProtoMember(5)]
	public float StackChargeTime { get; set; } = -1f;

	[ProtoMember(6)]
	public float ChargeAmount { get; set; }

	public bool IsActiveNow { get; set; } = true;

	public UnitSkill()
	{
	}

	public UnitSkill(int skillId, int? stackCount = null)
	{
		Id = skillId;
		Stack = stackCount ?? TSkill.Table[skillId].StackCount;
		IsActiveNow = true;
	}

	public UnitSkill Clone()
	{
		return new UnitSkill
		{
			Operation = Operation,
			Id = Id,
			Stack = Stack,
			CoolEndTime = CoolEndTime,
			StackChargeTime = StackChargeTime,
			ChargeAmount = ChargeAmount,
			IsActiveNow = IsActiveNow
		};
	}
}
