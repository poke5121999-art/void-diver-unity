using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfUpdateAreaDecoration : BaseMessage
{
	[ProtoMember(1)]
	public BitFlags AreaDecorationUnlockedFlags { get; set; }

	[ProtoMember(2)]
	public ActiveAreaDecoration[] ActiveAreaDecorations { get; set; }
}
