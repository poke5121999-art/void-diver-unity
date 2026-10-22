using GameCommon.Enum;
using GameCommon.Table;
using ProtoBuf;

namespace GameCommon.Data
{
	[ProtoContract]
	public sealed class BlueprintGoods : Goods
	{
		public override EGoodsType Type => EGoodsType.Blueprint;

		public override EGoodsGradeType Grade => TItem.Table[base.Id].Grade;

		public override int SellPriceEach => TItem.Table[base.Id].SellPrice;

		public override int GainWorth => TItem.Table[base.Id].Worth * (int)base.GainCount;

		public override EBagType PushableBagType => TItem.Table[base.Id].BagType;

		public BlueprintGoods()
		{
		}

		public BlueprintGoods(int id, long baseCount = 0L, long gainCount = 0L)
		{
			base.Id = id;
			base.BaseCount = baseCount;
			base.GainCount = gainCount;
		}

		public override Goods Clone()
		{
			return new BlueprintGoods(base.Id, base.BaseCount, base.GainCount);
		}

		public override Goods CloneWithoutGain()
		{
			return new BlueprintGoods(base.Id, base.BaseCount + base.GainCount, 0L);
		}
	}
}
