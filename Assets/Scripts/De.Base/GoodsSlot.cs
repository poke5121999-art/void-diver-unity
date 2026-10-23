using GameCommon.Data;
using GameCommon.Enum;
using ProtoBuf;
using Tracker;

[ProtoContract]
public class GoodsSlot : ITrackableDictionaryData<int, GoodsSlot>, ITrackableData<GoodsSlot>
{
	public int Key => SlotIndex;

	public int Id => Goods?.Id ?? 0;

	public long Count => Goods?.Count ?? 0;

	public EGoodsType Type => Goods?.Type ?? EGoodsType.None;

	public EGoodsCategory Category => Goods?.Category ?? EGoodsCategory.None;

	public EGoodsGradeType Grade => Goods?.Grade ?? EGoodsGradeType.None;

	public int GainWorth => Goods?.GainWorth ?? 0;

	public bool IsEventItem => Goods != null && Goods.IsEventItem();

	public bool IsEmpty => Goods == null;

	[ProtoMember(1)]
	public EDataOperation Operation { get; set; }

	[ProtoMember(2)]
	public int SlotIndex { get; set; }

	[ProtoMember(3)]
	public Goods Goods { get; set; }

	[ProtoMember(4)]
	public bool IsUnrevealed { get; set; }

	public bool IsRevealed => !IsUnrevealed;

	public GoodsSlot Clone()
	{
		return new GoodsSlot
		{
			Operation = Operation,
			SlotIndex = SlotIndex,
			Goods = Goods?.Clone(),
			IsUnrevealed = IsUnrevealed
		};
	}
}
