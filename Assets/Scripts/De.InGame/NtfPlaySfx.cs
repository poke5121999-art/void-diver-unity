using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfPlaySfx : BaseMessage
{
	[ProtoMember(1)]
	public string Sfx { get; set; }

	[ProtoMember(2)]
	public ProtoVector3 Position { get; set; }

	[ProtoMember(3)]
	public ulong TargetNetworkObjectId { get; set; }

	[ProtoMember(4)]
	public ProtoVector3 TargetPositionOffset { get; set; }
}
