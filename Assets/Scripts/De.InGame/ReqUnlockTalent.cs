using ProtoBuf;

[ProtoContract]
[Request]
public class ReqUnlockTalent : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public int TalentId { get; set; }
}
