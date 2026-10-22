using ProtoBuf;

[ProtoContract]
public class BuffStackEntry
{
	[ProtoMember(1)]
	public UnitStat[] CasterStats { get; set; }

	public BuffStackEntry Clone()
	{
		return null;
	}
}
