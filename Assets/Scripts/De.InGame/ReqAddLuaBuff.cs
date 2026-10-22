using ProtoBuf;

[ProtoContract]
[Request]
public class ReqAddLuaBuff : BaseMessage
{
	[ProtoMember(1)]
	public int BuffId { get; set; }
}
