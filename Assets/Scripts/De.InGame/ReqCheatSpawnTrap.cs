using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqCheatSpawnTrap : BaseMessage
{
	[ProtoMember(1)]
	public int TrapId { get; set; }

	[ProtoMember(2)]
	public ProtoVector3 Forward { get; set; }
}
