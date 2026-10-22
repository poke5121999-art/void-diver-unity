using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfPlayerKilledToast : BaseMessage
{
	[ProtoMember(1)]
	public string PlayerName { get; set; }

	[ProtoMember(2)]
	public string KillerName { get; set; }

	[ProtoMember(3)]
	public int KillerMonsterTableId { get; set; }
}
