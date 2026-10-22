using ProtoBuf;

[ProtoContract]
[Request]
public class ReqJoinGame : BaseMessage
{
	[ProtoMember(1)]
	public GamePlayerData PlayerData { get; set; }
}
