using GameCommon.Data;
using GameCommon.Enum;
using ProtoBuf;
using Tracker;

[ProtoContract]
public class LostGoods : ITrackableDictionaryData<int, LostGoods>, ITrackableData<LostGoods>
{
	public int Key => SlotIndex;

	public int Id => Goods?.Id ?? 0;

	public long Count => Goods?.Count ?? 0;

	public EGoodsType Type => Goods?.Type ?? EGoodsType.None;

	public EGoodsCategory Category => Goods?.Category ?? EGoodsCategory.None;

	[ProtoMember(1)]
	public EDataOperation Operation { get; set; }

	[ProtoMember(2)]
	public int SlotIndex { get; set; }

	[ProtoMember(3)]
	public Goods Goods { get; set; }

	[ProtoMember(4)]
	public int ExpireTurn { get; set; }

	public LostGoods Clone()
	{
		return new LostGoods
		{
			Operation = Operation,
			SlotIndex = SlotIndex,
			Goods = Goods?.Clone(),
			ExpireTurn = ExpireTurn
		};
	}
}
