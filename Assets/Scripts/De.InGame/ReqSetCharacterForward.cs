using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqSetCharacterForward : BaseMessage
{
	[ProtoMember(1)]
	public ProtoVector3 Forward { get; set; }
}
