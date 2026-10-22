using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfCloseCampaign : BaseMessage
{
	[ProtoMember(1)]
	public ECampaignCloseReason Reason { get; set; }
}
