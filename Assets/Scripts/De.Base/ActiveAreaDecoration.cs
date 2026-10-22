using ProtoBuf;

[ProtoContract]
public class ActiveAreaDecoration
{
	[ProtoMember(1)]
	public int GroupId { get; set; }

	[ProtoMember(2)]
	public int AreaDecorationSettingId { get; set; }

	public ActiveAreaDecoration Clone()
	{
		return null;
	}
}
