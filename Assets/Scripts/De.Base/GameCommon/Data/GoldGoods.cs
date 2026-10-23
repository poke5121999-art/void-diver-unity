using GameCommon.Enum;
using ProtoBuf;

namespace GameCommon.Data
{
	[ProtoContract]
	public sealed class GoldGoods : Goods
	{
		public override EGoodsType Type => EGoodsType.Gold;

		public override EGoodsGradeType Grade => EGoodsGradeType.None;

		public override int SellPriceEach => 0;

		public override int GainWorth => 0;

		public override EBagType PushableBagType => EBagType.None;

		public GoldGoods()
		{
		}

		public GoldGoods(long baseCount = 0L, long gainCount = 0L)
			: base(baseCount, gainCount)
		{
		}

		public override Goods Clone()
		{
			return new GoldGoods(BaseCount, GainCount);
		}

		public override Goods CloneWithoutGain()
		{
			return new GoldGoods(GainCount + BaseCount, 0L);
		}
	}
}
