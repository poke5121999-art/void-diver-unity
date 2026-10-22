using GameCommon.Enum;
using ProtoBuf;
using Tracker;

[ProtoContract]
public class UnitCc : ITrackableDictionaryData<ECrowdControlType, UnitCc>
{
	public ECrowdControlType Key => ECrowdControlType.None;

	[ProtoMember(1)]
	public EDataOperation Operation { get; set; }

	[ProtoMember(2)]
	public ECrowdControlType CcType { get; set; }

	[ProtoMember(3)]
	public float EndTime { get; set; }

	public UnitCc Clone()
	{
		return null;
	}
}
