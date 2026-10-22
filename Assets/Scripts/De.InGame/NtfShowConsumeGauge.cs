using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfShowConsumeGauge : BaseMessage
{
	[ProtoMember(1)]
	public ulong NetworkObjectId { get; set; }

	[ProtoMember(2)]
	public int ItemId { get; set; }

	[ProtoMember(3)]
	public float Duration { get; set; }

	[ProtoMember(4)]
	public bool OtherPlayerCanSee { get; set; }
}
