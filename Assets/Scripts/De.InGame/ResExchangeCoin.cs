using GameCommon.Data;
using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Response]
public class ResExchangeCoin : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public bool IsSuccess { get; set; }

	[ProtoMember(3)]
	public ECoinExchangeDirection Direction { get; set; }

	[ProtoMember(4)]
	public Goods RemainGoods { get; set; }
}
