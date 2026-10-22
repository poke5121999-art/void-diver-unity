using GameCommon.Data;
using GameCommon.Enum;
using ProtoBuf;
using Tracker;

[ProtoContract]
public class LostGoods : ITrackableDictionaryData<int, LostGoods>, ITrackableData<LostGoods>
{
	public int Key => 0;

	public int Id => 0;

	public long Count => 0L;

	public EGoodsType Type => EGoodsType.None;

	public EGoodsCategory Category => EGoodsCategory.None;

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
		return null;
	}
}
