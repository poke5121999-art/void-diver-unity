using GameCommon.Enum;
using ProtoBuf;

namespace GameCommon.Data
{
	[ProtoContract]
	public sealed class CoinGoods : Goods
	{
		public override EGoodsType Type => EGoodsType.Coin;

		public override EGoodsGradeType Grade => EGoodsGradeType.None;

		public override int SellPriceEach => 0;

		public override int GainWorth => 0;

		public override EBagType PushableBagType => EBagType.None;

		public CoinGoods()
		{
		}

		public CoinGoods(long baseCount = 0L, long gainCount = 0L)
			: base(baseCount, gainCount)
		{
		}

		public override Goods Clone()
		{
			return new CoinGoods(BaseCount, GainCount);
		}

		public override Goods CloneWithoutGain()
		{
			return new CoinGoods(BaseCount + GainCount, 0L);
		}
	}
}
