using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqRewardBox : BaseMessage
{
	[ProtoMember(1)]
	public int RewardBoxId { get; set; }

	[ProtoMember(2)]
	public ProtoVector3 Position { get; set; }

	[ProtoMember(3)]
	public ProtoVector3 Forward { get; set; }
}
