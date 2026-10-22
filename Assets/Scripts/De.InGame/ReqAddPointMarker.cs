using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqAddPointMarker : BaseMessage
{
	[ProtoMember(1)]
	public ProtoVector3 Position { get; set; }
}
