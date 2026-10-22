using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqSetCharacterPosition : BaseMessage
{
	[ProtoMember(1)]
	public ProtoVector3 Position { get; set; }
}
