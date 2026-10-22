using ProtoBuf;

[ProtoContract]
[Request]
public class ReqCheatApplyMonsterHp : BaseMessage
{
	[ProtoMember(1)]
	public int Hp { get; set; }
}
