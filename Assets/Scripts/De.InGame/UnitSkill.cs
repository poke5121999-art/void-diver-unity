using ProtoBuf;
using Tracker;

[ProtoContract]
public class UnitSkill : ITrackableDictionaryData<int, UnitSkill>
{
	public int Key => 0;

	[ProtoMember(1)]
	public EDataOperation Operation { get; set; }

	[ProtoMember(2)]
	public int Id { get; set; }

	[ProtoMember(3)]
	public int Stack { get; set; }

	[ProtoMember(4)]
	public float CoolEndTime { get; set; }

	[ProtoMember(5)]
	public float StackChargeTime { get; set; }

	[ProtoMember(6)]
	public float ChargeAmount { get; set; }

	public bool IsActiveNow { get; set; }

	public UnitSkill()
	{
	}

	public UnitSkill(int skillId, int? stackCount = null)
	{
	}

	public UnitSkill Clone()
	{
		return null;
	}
}
