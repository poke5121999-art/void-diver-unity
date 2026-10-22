using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqCheatMovePosition : BaseMessage
{
	[ProtoMember(1)]
	public ProtoVector3 Position { get; set; }
}
