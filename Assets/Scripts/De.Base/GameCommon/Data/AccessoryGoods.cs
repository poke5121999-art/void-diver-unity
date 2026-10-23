using System;
using GameCommon.Enum;
using GameCommon.Table;
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

		public override EGoodsType Type => EGoodsType.Accessory;

		public override EGoodsGradeType Grade => TEquipment.Table[base.Id].Grade;

		public override int SellPriceEach => TEquipment.Table[base.Id].SellPrice;

		public override int GainWorth => TEquipment.Table[base.Id].Worth * (int)base.GainCount;

		public override EBagType PushableBagType => TEquipment.Table[base.Id].BagType;

		public bool IsBroken => CurrentDurability <= (float)TConst.EquipmentBreakThreshold.IntValue;

		public bool CanRepair
		{
			get
			{
				if (MaxDurability <= 0)
				{
					return false;
				}
				// 0x184a51a20 is ceil(double) (machine code checked)
				return (int)Math.Ceiling(CurrentDurability) < MaxDurability;
			}
		}

		public AccessoryGoods()
		{
		}

		public AccessoryGoods(int id, long baseCount, long gainCount, int maxDurability, float currentDurability, int maxDurabilityFraction = 0)
		{
			base.Id = id;
			base.BaseCount = baseCount;
			base.GainCount = gainCount;
			MaxDurability = maxDurability;
			CurrentDurability = currentDurability;
			MaxDurabilityFraction = maxDurabilityFraction;
		}

		public override Goods Clone()
		{
			return new AccessoryGoods(base.Id, base.BaseCount, base.GainCount, MaxDurability, CurrentDurability, MaxDurabilityFraction);
		}

		public override Goods CloneWithoutGain()
		{
			return new AccessoryGoods(base.Id, base.BaseCount + base.GainCount, 0L, MaxDurability, CurrentDurability, MaxDurabilityFraction);
		}
	}
}
