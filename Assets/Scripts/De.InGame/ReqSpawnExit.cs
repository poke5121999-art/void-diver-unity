using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqSpawnExit : BaseMessage
{
	[ProtoMember(1)]
	public int ExitId { get; set; }

	[ProtoMember(2)]
	public ProtoVector3 Position { get; set; }
}
