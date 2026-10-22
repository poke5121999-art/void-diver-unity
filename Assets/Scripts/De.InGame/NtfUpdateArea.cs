using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfUpdateArea : BaseMessage
{
	[ProtoMember(1)]
	public BitFlags AreaFlags { get; set; }
}
