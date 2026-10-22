using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfChangeActiveSkill : BaseMessage
{
	[ProtoMember(1)]
	public ulong NetworkObjectId { get; set; }

	[ProtoMember(2)]
	public int[] SkillIds { get; set; }
}
