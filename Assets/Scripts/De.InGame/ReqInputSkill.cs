using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqInputSkill : BaseMessage
{
	[ProtoMember(1)]
	public ESkillInputEvent InputEvent { get; set; }

	[ProtoMember(2)]
	public bool ContinuousAttack { get; set; }
}
