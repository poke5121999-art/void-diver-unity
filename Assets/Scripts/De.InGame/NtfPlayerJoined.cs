using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfPlayerJoined : BaseMessage
{
	[ProtoMember(1)]
	public GamePlayerData PlayerData { get; set; }
}
