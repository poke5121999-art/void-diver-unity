using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqInputUseItem : BaseMessage
{
	[ProtoMember(1)]
	public int ItemId { get; set; }

	[ProtoMember(2)]
	public ESkillInputEvent InputEvent { get; set; }
}
