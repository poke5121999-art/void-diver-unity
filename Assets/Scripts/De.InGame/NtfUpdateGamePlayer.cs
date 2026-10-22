using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfUpdateGamePlayer : BaseMessage
{
	[ProtoMember(1)]
	public GamePlayerData ChangeData { get; set; }
}
