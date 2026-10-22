using ProtoBuf;

[ProtoContract]
[Request]
public class ReqChangeCharacterSkills : BaseMessage
{
	[ProtoMember(1)]
	public int[] ActiveSkillIds { get; set; }
}
