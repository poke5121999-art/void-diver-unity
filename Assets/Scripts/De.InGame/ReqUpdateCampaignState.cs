using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqUpdateCampaignState : BaseMessage
{
	[ProtoMember(1)]
	public ECampaignState State { get; set; }
}
