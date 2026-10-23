using GameCommon.Enum;
using ProtoBuf;
using Tracker;

[ProtoContract]
public class UnitItemCoolTime : ITrackableDictionaryData<EConsumableType, UnitItemCoolTime>
{
	public EConsumableType Key => ConsumableType;

	[ProtoMember(1)]
	public EDataOperation Operation { get; set; }

	[ProtoMember(2)]
	public EConsumableType ConsumableType { get; set; }

	[ProtoMember(3)]
	public float EndTime { get; set; }

	public UnitItemCoolTime Clone()
	{
		return new UnitItemCoolTime
		{
			Operation = Operation,
			ConsumableType = ConsumableType,
			EndTime = EndTime
		};
	}
}
