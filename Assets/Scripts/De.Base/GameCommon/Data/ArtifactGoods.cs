using System;
using GameCommon.Enum;
using GameCommon.Table;
using UnityEngine;
using ProtoBuf;

namespace GameCommon.Data
{
	[ProtoContract]
	public sealed class ArtifactGoods : Goods
	{
		[ProtoMember(1)]
		public int[] Prefixes { get; private set; } = Array.Empty<int>();

		[ProtoMember(2)]
		public byte CampaignLevel { get; private set; } = 1;

		public override EGoodsType Type => EGoodsType.Artifact;

		public override EGoodsGradeType Grade => TEquipment.Table[base.Id].Grade;

		public override int SellPriceEach => GetSellPrice();

		public override int GainWorth => TEquipment.Table[base.Id].Worth * (int)base.GainCount;

		public override EBagType PushableBagType => TEquipment.Table[base.Id].BagType;

		public ArtifactGoods()
		{
		}

		public ArtifactGoods(int id, long baseCount, long gainCount, int[] prefixes = null, byte campaignLevel = 1)
		{
			base.Id = id;
			base.BaseCount = baseCount;
			base.GainCount = gainCount;
			Prefixes = prefixes ?? Array.Empty<int>();
			CampaignLevel = campaignLevel;
		}

		public void NormalizeHackPrefixes()
		{
			Prefixes = new int[1] { 1 };
		}

		private int GetSellPrice()
		{
			TEquipment tEquipment = TEquipment.Table[base.Id];
			int sellPrice = tEquipment.SellPrice;
			float num = 1f;
			if (Prefixes != null)
			{
				for (int i = 0; i < Prefixes.Length; i++)
				{
					num += TArtifactPrefix.Table[Prefixes[i]].PriceMultiplier;
				}
				float num2 = Mathf.Pow(TConst.ArtifactPriceCampaignLevelGrowthRate.FloatValue + 1f, (int)CampaignLevel - 1);
				// 0x1843a50a0 is Math.Round(double), half-to-even (machine code checked)
				int num3 = (int)Math.Round((float)sellPrice * num * num2);
				int num4 = (int)Math.Round((float)(sellPrice * TConst.ArtifactMinimumPricePercent.IntValue) / 100f);
				return (num3 > num4) ? num3 : num4;
			}
			return sellPrice;
		}

		public override Goods Clone()
		{
			return new ArtifactGoods(base.Id, base.BaseCount, base.GainCount, (int[])Prefixes?.Clone(), CampaignLevel);
		}

		public override Goods CloneWithoutGain()
		{
			return new ArtifactGoods(base.Id, base.BaseCount + base.GainCount, 0L, (int[])Prefixes?.Clone(), CampaignLevel);
		}
	}
}
