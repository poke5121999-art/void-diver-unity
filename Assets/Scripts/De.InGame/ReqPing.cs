using GameCommon.Data;
using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqPing : BaseMessage
{
	[ProtoMember(1)]
	public EPingType PingType { get; set; }

	[ProtoMember(2)]
	public ProtoVector3 Position { get; set; }

	[ProtoMember(3)]
	public EPingTargetType TargetType { get; set; }

	[ProtoMember(4)]
	public int Value1 { get; set; }

	[ProtoMember(5)]
	public int Value2 { get; set; }
}
