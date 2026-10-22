using GameCommon.Data;
using GameCommon.Enum;
using ProtoBuf;
using Tracker;

[ProtoContract]
public class GoodsSlot : ITrackableDictionaryData<int, GoodsSlot>, ITrackableData<GoodsSlot>
{
	public int Key => 0;

	public int Id => 0;

	public long Count => 0L;

	public EGoodsType Type => EGoodsType.None;

	public EGoodsCategory Category => EGoodsCategory.None;

	public EGoodsGradeType Grade => EGoodsGradeType.None;

	public int GainWorth => 0;

	public bool IsEventItem => false;

	public bool IsEmpty => false;

	[ProtoMember(1)]
	public EDataOperation Operation { get; set; }

	[ProtoMember(2)]
	public int SlotIndex { get; set; }

	[ProtoMember(3)]
	public Goods Goods { get; set; }

	[ProtoMember(4)]
	public bool IsUnrevealed { get; set; }

	public bool IsRevealed => false;

	public GoodsSlot Clone()
	{
		return null;
	}
}
