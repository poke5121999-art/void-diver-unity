using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqGainGoods : BaseMessage
{
	[ProtoMember(1)]
	public Goods[] Goods { get; set; }
}
