using GameCommon.Enum;
using ProtoBuf;

namespace GameCommon.Data
{
	[ProtoContract]
	[ProtoInclude(11, typeof(GoldGoods))]
	[ProtoInclude(12, typeof(CoinGoods))]
	[ProtoInclude(13, typeof(ExpGoods))]
	[ProtoInclude(14, typeof(ConsumableGoods))]
	[ProtoInclude(15, typeof(ValuableGoods))]
	[ProtoInclude(16, typeof(MiscGoods))]
	[ProtoInclude(17, typeof(NoteGoods))]
	[ProtoInclude(18, typeof(BlueprintGoods))]
	[ProtoInclude(19, typeof(WeaponGoods))]
	[ProtoInclude(20, typeof(AccessoryGoods))]
	[ProtoInclude(21, typeof(ArtifactGoods))]
	[ProtoInclude(22, typeof(BagGoods))]
	[ProtoInclude(23, typeof(MusicDiscGoods))]
	public abstract class Goods
	{
		[ProtoMember(1)]
		public int Id { get; protected set; }

		[ProtoMember(2)]
		public long BaseCount { get; protected set; }

		[ProtoMember(3)]
		public long GainCount { get; protected set; }

		public abstract EGoodsType Type { get; }

		public abstract EGoodsGradeType Grade { get; }

		public abstract int SellPriceEach { get; }

		public abstract int GainWorth { get; }

		public abstract EBagType PushableBagType { get; }

		public EGoodsCategory Category => Type.GoodsCategory();

		public long Count => GainCount + BaseCount;

		public int SellPriceTotal => SellPriceEach * (int)(GainCount + BaseCount);

		protected Goods()
		{
		}

		protected Goods(long baseCount, long gainCount)
		{
			BaseCount = baseCount;
			GainCount = gainCount;
		}

		protected Goods(int id, long baseCount, long gainCount)
		{
			Id = id;
			BaseCount = baseCount;
			GainCount = gainCount;
		}

		public abstract Goods Clone();

		public abstract Goods CloneWithoutGain();

		public override string ToString()
		{
			return string.Format("{0}:{1}:{2}", Type, Id, GainCount + BaseCount);
		}
	}
}
