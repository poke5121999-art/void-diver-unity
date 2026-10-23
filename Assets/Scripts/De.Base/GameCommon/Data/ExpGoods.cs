using GameCommon.Enum;
using ProtoBuf;

namespace GameCommon.Data
{
	[ProtoContract]
	public sealed class ExpGoods : Goods
	{
		public override EGoodsType Type => EGoodsType.Exp;

		public override EGoodsGradeType Grade => EGoodsGradeType.None;

		public override int SellPriceEach => 0;

		public override int GainWorth => 0;

		public override EBagType PushableBagType => EBagType.None;

		public ExpGoods()
		{
		}

		public ExpGoods(long baseCount = 0L, long gainCount = 0L)
			: base(baseCount, gainCount)
		{
		}

		public override Goods Clone()
		{
			return new ExpGoods(BaseCount, GainCount);
		}

		public override Goods CloneWithoutGain()
		{
			return new ExpGoods(GainCount + BaseCount, 0L);
		}
	}
}
