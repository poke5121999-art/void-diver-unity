using ProtoBuf;

[ProtoContract]
[Request]
public class ReqTycoonHire : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public int EmployeeId { get; set; }

	[ProtoMember(3)]
	public int Level { get; set; }
}
