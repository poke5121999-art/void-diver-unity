using GameCommon.Enum;
using ProtoBuf;

namespace GameCommon.Data
{
	[ProtoContract]
	public sealed class AccessoryGoods : Goods
	{
		[ProtoMember(1)]
		public int MaxDurability { get; private set; }

		[ProtoMember(2)]
		public float CurrentDurability { get; private set; }

		[ProtoMember(3)]
		public int MaxDurabilityFraction { get; private set; }

		public override EGoodsType Type => EGoodsType.None;

		public override EGoodsGradeType Grade => EGoodsGradeType.None;

		public override int SellPriceEach => 0;

		public override int GainWorth => 0;

		public override EBagType PushableBagType => EBagType.None;

		public bool IsBroken => false;

		public bool CanRepair => false;

		public AccessoryGoods()
		{
		}

		public AccessoryGoods(int id, long baseCount, long gainCount, int maxDurability, float currentDurability, int maxDurabilityFraction = 0)
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
