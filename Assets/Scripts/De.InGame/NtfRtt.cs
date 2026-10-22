using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfRtt : BaseMessage
{
	[ProtoMember(1)]
	public ClientRtt[] ClientRtts { get; set; }
}
