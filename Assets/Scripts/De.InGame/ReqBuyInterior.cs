using ProtoBuf;

[ProtoContract]
[Request]
public class ReqBuyInterior : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public int InteriorShopId { get; set; }
}
