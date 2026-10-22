using ProtoBuf;

[ProtoContract]
[Request]
public class ReqRemovePointMarker : BaseMessage
{
	[ProtoMember(1)]
	public int MarkerIndex { get; set; }
}
