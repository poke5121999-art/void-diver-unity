using GameCommon.Enum;
using ProtoBuf;

namespace GameCommon.Data
{
	[ProtoContract]
	public sealed class BagGoods : Goods
	{
		[ProtoMember(1)]
		public GoodsSlot[] GoodsSlots { get; private set; }

		public override EGoodsType Type => EGoodsType.None;

		public override EBagType PushableBagType => EBagType.None;

		public override EGoodsGradeType Grade => EGoodsGradeType.None;

		public override int SellPriceEach => 0;

		public override int GainWorth => 0;

		public int OccupiedSlotCount => 0;

		public int MaxSlotCount => 0;

		public BagGoods()
		{
		}

		public BagGoods(int id, long baseCount, long gainCount, GoodsSlot[] goodsSlots)
		{
		}

		public override Goods Clone()
		{
			return null;
		}

		public override Goods CloneWithoutGain()
		{
			return null;
		}
	}
}
