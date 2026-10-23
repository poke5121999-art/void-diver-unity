using GameCommon.Enum;
using GameCommon.Table;
using ProtoBuf;

namespace GameCommon.Data
{
	[ProtoContract]
	public sealed class ValuableGoods : Goods
	{
		public override EGoodsType Type => EGoodsType.Valuable;

		public override EGoodsGradeType Grade => TItem.Table[Id].Grade;

		public override int SellPriceEach => TItem.Table[Id].SellPrice;

		public override int GainWorth => TItem.Table[Id].Worth * (int)GainCount;

		public override EBagType PushableBagType => TItem.Table[Id].BagType;

		public ValuableGoods()
		{
		}

		public ValuableGoods(int id, long baseCount = 0L, long gainCount = 0L)
			: base(id, baseCount, gainCount)
		{
		}

		public override Goods Clone()
		{
			return new ValuableGoods(Id, BaseCount, GainCount);
		}

		public override Goods CloneWithoutGain()
		{
			return new ValuableGoods(Id, BaseCount + GainCount, 0L);
		}
	}
}
