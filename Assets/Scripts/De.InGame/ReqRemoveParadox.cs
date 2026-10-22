using ProtoBuf;

[ProtoContract]
[Request]
public class ReqRemoveParadox : BaseMessage
{
	[ProtoMember(1)]
	public int ParadoxId { get; set; }

	[ProtoMember(2)]
	public bool IncludePositive { get; set; }

	[ProtoMember(3)]
	public bool IncludeNegative { get; set; }
}
