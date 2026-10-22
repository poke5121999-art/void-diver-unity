using GameCommon.Enum;
using ProtoBuf;
using Tracker;

[ProtoContract]
public class UnitItemCoolTime : ITrackableDictionaryData<EConsumableType, UnitItemCoolTime>
{
	public EConsumableType Key => EConsumableType.None;

	[ProtoMember(1)]
	public EDataOperation Operation { get; set; }

	[ProtoMember(2)]
	public EConsumableType ConsumableType { get; set; }

	[ProtoMember(3)]
	public float EndTime { get; set; }

	public UnitItemCoolTime Clone()
	{
		return null;
	}
}
