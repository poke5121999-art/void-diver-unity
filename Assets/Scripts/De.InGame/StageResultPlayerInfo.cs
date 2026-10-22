using GameCommon.Data;
using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
public class StageResultPlayerInfo
{
	[ProtoMember(1)]
	public EActionState State;

	[ProtoMember(2)]
	public ulong ClientId;

	[ProtoMember(3)]
	public bool IsLostItem;

	[ProtoMember(4)]
	public Goods[] HighGradeGoods;

	[ProtoMember(5)]
	public Goods[] LostGoods;

	[ProtoMember(6)]
	public int[] GainedParadoxIds;

	[ProtoMember(7)]
	public int[] RemovedParadoxIds;

	[ProtoMember(8)]
	public int CharacterId;

	[ProtoMember(9)]
	public string UserName;

	public int[] InitialParadoxIds;
}
