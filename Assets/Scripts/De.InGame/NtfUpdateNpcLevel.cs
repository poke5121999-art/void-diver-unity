using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfUpdateNpcLevel : BaseMessage
{
	[ProtoMember(1)]
	public NpcLevelRecord[] NpcLevels { get; set; }
}
