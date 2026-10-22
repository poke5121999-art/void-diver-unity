using GameCommon.Data;
using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
public class ZoneData
{
	[ProtoMember(1)]
	public int ZoneSpawnId { get; set; }

	[ProtoMember(2)]
	public EZoneFlags ZoneFlags { get; set; }

	[ProtoMember(3)]
	public ProtoVector3 Position { get; set; }

	[ProtoMember(4)]
	public ESpawnType SpawnType { get; set; }

	[ProtoMember(5)]
	public float ExcludedSpawnRange { get; set; }
}
