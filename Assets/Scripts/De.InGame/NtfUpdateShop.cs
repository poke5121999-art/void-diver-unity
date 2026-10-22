using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfUpdateShop : BaseMessage
{
	[ProtoMember(1)]
	public int Tick { get; set; }

	[ProtoMember(2)]
	public ShopProduct[] ChangeProducts { get; set; }
}
