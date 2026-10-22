using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfUpdateCampaignState : BaseMessage
{
	[ProtoMember(1)]
	public ulong ClientId { get; set; }

	[ProtoMember(2)]
	public ECampaignState State { get; set; }
}
