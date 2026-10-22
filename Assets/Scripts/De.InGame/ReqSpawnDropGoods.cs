using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqSpawnDropGoods : BaseMessage
{
	[ProtoMember(1)]
	public Goods[] Goods { get; set; }

	[ProtoMember(2)]
	public ProtoVector3 Position { get; set; }
}
