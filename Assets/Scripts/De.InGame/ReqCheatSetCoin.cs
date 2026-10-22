using ProtoBuf;

[ProtoContract]
[Request]
public class ReqCheatSetCoin : BaseMessage
{
	[ProtoMember(1)]
	public long Coin { get; set; }
}
