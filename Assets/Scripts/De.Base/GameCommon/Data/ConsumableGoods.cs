using GameCommon.Table;
using GameCommon.Enum;
using ProtoBuf;

namespace GameCommon.Data
{
	[ProtoContract]
	public sealed class ConsumableGoods : Goods
	{
		public override EGoodsType Type => EGoodsType.Consumable;

		public override EGoodsGradeType Grade => TItem.Table[Id].Grade;

		public override int SellPriceEach => TItem.Table[Id].SellPrice;

		public override int GainWorth => TItem.Table[Id].Worth * (int)GainCount;

		public override EBagType PushableBagType => TItem.Table[Id].BagType;

		public ConsumableGoods()
		{
		}

		public ConsumableGoods(int id, long baseCount = 0L, long gainCount = 0L)
			: base(id, baseCount, gainCount)
		{
		}

		public override Goods Clone()
		{
			return new ConsumableGoods(Id, BaseCount, GainCount);
		}

		public override Goods CloneWithoutGain()
		{
			return new ConsumableGoods(Id, GainCount + BaseCount, 0L);
		}
	}
}
