using ProtoBuf;

[ProtoContract]
[Request]
public class ReqSetPet : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public int PetId { get; set; }
}
