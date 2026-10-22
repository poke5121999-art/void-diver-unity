using GameCommon.Enum;
using ProtoBuf;

namespace GameCommon.Data
{
	[ProtoContract]
	public sealed class ArtifactGoods : Goods
	{
		[ProtoMember(1)]
		public int[] Prefixes { get; private set; }

		[ProtoMember(2)]
		public byte CampaignLevel { get; private set; }

		public override EGoodsType Type => EGoodsType.None;

		public override EGoodsGradeType Grade => EGoodsGradeType.None;

		public override int SellPriceEach => 0;

		public override int GainWorth => 0;

		public override EBagType PushableBagType => EBagType.None;

		public ArtifactGoods()
		{
		}

		public ArtifactGoods(int id, long baseCount, long gainCount, int[] prefixes = null, byte campaignLevel = 1)
		{
		}

		public void NormalizeHackPrefixes()
		{
		}

		private int GetSellPrice()
		{
			return 0;
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
