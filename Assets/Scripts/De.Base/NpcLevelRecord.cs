using ProtoBuf;

[ProtoContract]
public class NpcLevelRecord
{
	[ProtoMember(1)]
	public int NpcId { get; set; }

	[ProtoMember(2)]
	public int Level { get; set; }

	public NpcLevelRecord Clone()
	{
		return new NpcLevelRecord
		{
			NpcId = NpcId,
			Level = Level
		};
	}
}
