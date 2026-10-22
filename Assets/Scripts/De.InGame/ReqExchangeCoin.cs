using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqExchangeCoin : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public ECoinExchangeDirection Direction { get; set; }

	[ProtoMember(3)]
	public long Quantity { get; set; }
}
