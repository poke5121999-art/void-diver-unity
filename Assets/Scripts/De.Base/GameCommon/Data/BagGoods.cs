using System.Linq;
using GameCommon.Enum;
using GameCommon.Table;
using ProtoBuf;

namespace GameCommon.Data
{
	[ProtoContract]
	public sealed class BagGoods : Goods
	{
		[ProtoMember(1)]
		public GoodsSlot[] GoodsSlots { get; private set; }

		public override EGoodsType Type => EGoodsType.Bag;

		public override EBagType PushableBagType => EBagType.None;

		public override EGoodsGradeType Grade => TBag.Table[base.Id].Grade;

		public override int SellPriceEach => TBag.Table[base.Id].SellPrice;

		public override int GainWorth => TBag.Table[base.Id].Worth * (int)base.GainCount;

		public int OccupiedSlotCount
		{
			get
			{
				if (GoodsSlots == null)
				{
					return 0;
				}
				return GoodsSlots.Count((GoodsSlot s) => s != null && !s.IsEmpty);
			}
		}

		public int MaxSlotCount => TBag.Table[base.Id].SlotCount;

		public BagGoods()
		{
		}

		public BagGoods(int id, long baseCount, long gainCount, GoodsSlot[] goodsSlots)
		{
			GoodsSlots = goodsSlots;
			base.Id = id;
			base.BaseCount = baseCount;
			base.GainCount = gainCount;
		}

		public override Goods Clone()
		{
			return new BagGoods(base.Id, base.BaseCount, base.GainCount, GoodsSlots?.Select((GoodsSlot s) => s?.Clone()).ToArray());
		}

		public override Goods CloneWithoutGain()
		{
			return new BagGoods(base.Id, base.BaseCount + base.GainCount, 0L, GoodsSlots?.Select((GoodsSlot s) => s?.Clone()).ToArray());
		}
	}
}
