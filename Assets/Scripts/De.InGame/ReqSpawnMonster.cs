using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqSpawnMonster : BaseMessage
{
	[ProtoMember(1)]
	public int MonsterId { get; set; }

	[ProtoMember(2)]
	public ProtoVector3 Position { get; set; }
}
