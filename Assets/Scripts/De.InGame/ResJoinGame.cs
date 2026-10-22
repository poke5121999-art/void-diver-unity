using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Response]
public class ResJoinGame : BaseMessage
{
	[ProtoMember(1)]
	public GamePlayerData[] PlayerDatas { get; set; }

	[ProtoMember(2)]
	public ERoomDifficulty RoomDifficulty { get; set; }

	[ProtoMember(3)]
	public int CampaignId { get; set; }

	[ProtoMember(4)]
	public int[] MapSize { get; set; }

	[ProtoMember(5)]
	public int[] SectorIds { get; set; }
}
