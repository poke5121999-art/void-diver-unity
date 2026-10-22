using GameCommon.Data;
using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfPing : BaseMessage
{
	[ProtoMember(1)]
	public ulong ClientId { get; set; }

	[ProtoMember(2)]
	public EPingType PingType { get; set; }

	[ProtoMember(3)]
	public ProtoVector3 Position { get; set; }

	[ProtoMember(4)]
	public EPingTargetType TargetType { get; set; }

	[ProtoMember(5)]
	public int Value1 { get; set; }

	[ProtoMember(6)]
	public int Value2 { get; set; }
}
