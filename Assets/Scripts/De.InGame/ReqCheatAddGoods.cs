using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqCheatAddGoods : BaseMessage
{
	[ProtoMember(1)]
	public Goods[] Goods { get; set; }
}
