using GameCommon.Enum;
using ProtoBuf;
using Tracker;

[ProtoContract]
public class UnitStat : ITrackableDictionaryData<EStatType, UnitStat>
{
	public EStatType Key => EStatType.None;

	[ProtoMember(1)]
	public EDataOperation Operation { get; set; }

	[ProtoMember(2)]
	public EStatType StatType { get; set; }

	[ProtoMember(3)]
	public float Value { get; set; }

	public UnitStat Clone()
	{
		return null;
	}
}
