using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfChangeSkill : BaseMessage
{
	[ProtoMember(1)]
	public ulong NetworkObjectId { get; set; }

	[ProtoMember(2)]
	public int OriginalSkillId { get; set; }

	[ProtoMember(3)]
	public int ReplacementSkillId { get; set; }

	[ProtoMember(4)]
	public bool IsRemove { get; set; }
}
