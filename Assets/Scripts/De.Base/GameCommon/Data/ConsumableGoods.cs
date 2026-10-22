using GameCommon.Enum;
using ProtoBuf;

namespace GameCommon.Data
{
	[ProtoContract]
	public sealed class ConsumableGoods : Goods
	{
		public override EGoodsType Type => EGoodsType.None;

		public override EGoodsGradeType Grade => EGoodsGradeType.None;

		public override int SellPriceEach => 0;

		public override int GainWorth => 0;

		public override EBagType PushableBagType => EBagType.None;

		public ConsumableGoods()
		{
		}

		public ConsumableGoods(int id, long baseCount = 0L, long gainCount = 0L)
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
