using ProtoBuf;

[ProtoContract]
[Response]
public class ResSyncShop : BaseMessage
{
	[ProtoMember(1)]
	public int Tick { get; set; }

	[ProtoMember(2)]
	public ShopProduct[] Products { get; set; }
}
