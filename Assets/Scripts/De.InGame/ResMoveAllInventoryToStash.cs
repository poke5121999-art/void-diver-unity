using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Response]
public class ResMoveAllInventoryToStash : BaseMessage
{
	[ProtoMember(1)]
	public Goods[] PulledGoods { get; set; }
}
