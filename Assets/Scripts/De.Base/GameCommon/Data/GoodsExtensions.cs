using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Enum;
using GameCommon.Table;

namespace GameCommon.Data
{
	public static class GoodsExtensions
	{
		private const int DurabilityFractionScale = 100;

		public static bool IsCurrency(this Goods goods)
		{
			if (goods == null)
			{
				return false;
			}
			return goods.Type.GoodsCategory() == EGoodsCategory.Currency;
		}

		public static bool IsItem(this Goods goods)
		{
			if (goods == null)
			{
				return false;
			}
			return goods.Type.GoodsCategory() == EGoodsCategory.Item;
		}

		public static bool IsEquipment(this Goods goods)
		{
			if (goods == null)
			{
				return false;
			}
			return goods.Type.GoodsCategory() == EGoodsCategory.Equipment;
		}

		public static bool IsBag(this Goods goods)
		{
			if (goods == null)
			{
				return false;
			}
			return goods.Type.GoodsCategory() == EGoodsCategory.Bag;
		}

		public static bool IsGold(this Goods goods)
		{
			return goods is GoldGoods;
		}

		public static bool IsCoin(this Goods goods)
		{
			return goods is CoinGoods;
		}

		public static bool IsExp(this Goods goods)
		{
			return goods is ExpGoods;
		}

		public static bool IsConsumable(this Goods goods)
		{
			return goods is ConsumableGoods;
		}

		public static bool IsValuable(this Goods goods)
		{
			return goods is ValuableGoods;
		}

		public static bool IsMisc(this Goods goods)
		{
			return goods is MiscGoods;
		}

		public static bool IsNote(this Goods goods)
		{
			return goods is NoteGoods;
		}

		public static bool IsBlueprint(this Goods goods)
		{
			return goods is BlueprintGoods;
		}

		public static bool IsMusicDisc(this Goods goods)
		{
			return goods is MusicDiscGoods;
		}

		public static bool IsWeapon(this Goods goods)
		{
			return goods is WeaponGoods;
		}

		public static bool IsAccessory(this Goods goods)
		{
			return goods is AccessoryGoods;
		}

		public static bool IsArtifact(this Goods goods)
		{
			return goods is ArtifactGoods;
		}

		public static bool HasDurability(this Goods goods)
		{
			return goods is WeaponGoods || goods is AccessoryGoods;
		}

		public static bool IsEventItem(this Goods goods)
		{
			if (goods != null && goods.Type.GoodsCategory() == EGoodsCategory.Item)
			{
				return TItem.Table[goods.Id].IsEventItem;
			}
			return false;
		}

		public static bool IsQuestItem(this Goods goods)
		{
			if (goods != null && goods.Type.GoodsCategory() == EGoodsCategory.Item)
			{
				return TItem.Table[goods.Id].Category == EItemCategory.Quest;
			}
			return false;
		}

		public static string GetName(this Goods goods)
		{
			if (goods == null)
			{
				return string.Empty;
			}
			switch (goods.Type.GoodsCategory())
			{
			case EGoodsCategory.Item:
				return TItem.Table[goods.Id].Name;
			case EGoodsCategory.Equipment:
				return TEquipment.Table[goods.Id].Name;
			case EGoodsCategory.Bag:
				return TBag.Table[goods.Id].Name;
			default:
				return string.Empty;
			}
		}

		public static EGoodsGradeType GetGrade(this Goods goods)
		{
			if (goods == null)
			{
				return EGoodsGradeType.None;
			}
			return goods.Grade;
		}

		public static int GetArtifactPrice(this Goods goods)
		{
			if (goods is ArtifactGoods)
			{
				return goods.SellPriceEach * (int)(goods.GainCount + goods.BaseCount);
			}
			return 0;
		}

		public static int GetWorth(this Goods goods)
		{
			if (goods == null)
			{
				return 0;
			}
			return goods.GainWorth;
		}

		public static int GetCampaignLevel(this Goods goods)
		{
			if (goods is ArtifactGoods artifactGoods)
			{
				return artifactGoods.CampaignLevel;
			}
			return 1;
		}

		public static bool IsBrokenEquipment(this Goods goods)
		{
			if (goods == null)
			{
				return false;
			}
			float currentDurability;
			if (goods is WeaponGoods weaponGoods)
			{
				currentDurability = weaponGoods.CurrentDurability;
			}
			else
			{
				if (!(goods is AccessoryGoods accessoryGoods))
				{
					return false;
				}
				currentDurability = accessoryGoods.CurrentDurability;
			}
			return currentDurability <= (float)TConst.EquipmentBreakThreshold.IntValue;
		}

		public static float GetDurabilityRatio(this Goods goods)
		{
			if (goods == null)
			{
				return 1f;
			}
			int maxDurability = goods.GetMaxDurability();
			if (!goods.HasDurability() || maxDurability <= 0)
			{
				return 1f;
			}
			return goods.GetCurrentDurability() / (float)maxDurability;
		}

		public static bool CanRepair(this Goods goods)
		{
			if (goods == null)
			{
				return false;
			}
			int maxDurability = goods.GetMaxDurability();
			if (goods.HasDurability() && maxDurability > 0)
			{
				// TODO(decomp): double->double Math fn at 0x184a51a20 (Floor vs Ceiling) @0x180599840
				return (int)Math.Floor(goods.GetCurrentDurability()) < maxDurability;
			}
			return false;
		}

		public static Goods Repair(this Goods goods, bool preventMaxDurabilityLoss = false)
		{
			if (goods == null)
			{
				return null;
			}
			if (!goods.HasDurability())
			{
				return goods;
			}
			TEquipment tEquipment = TEquipment.Table[goods.Id];
			float effectiveMaxDurability = goods.GetEffectiveMaxDurability();
			float currentDurability = goods.GetCurrentDurability();
			float num = Math.Max(0f, effectiveMaxDurability - currentDurability);
			float num2 = (preventMaxDurabilityLoss ? 0f : (num * tEquipment.RepairMaxDurabilityLoss));
			float num3 = Math.Max(1f, effectiveMaxDurability - num2);
			// TODO(decomp): double->double Math fns at 0x184a51804 (Floor) and 0x1843a50a0 (Round) @0x18059bab0
			int num4 = (int)Math.Floor(num3);
			int num5 = (int)Math.Round((num3 - (float)num4) * 100f);
			int num6;
			int num7;
			if (num5 < 100)
			{
				num6 = num4;
				num7 = num5;
			}
			else
			{
				num6 = num4 + 1;
				num7 = 0;
			}
			if (goods is WeaponGoods weaponGoods)
			{
				return new WeaponGoods(weaponGoods.Id, weaponGoods.BaseCount, weaponGoods.GainCount, num6, num6, num7);
			}
			AccessoryGoods accessoryGoods = (AccessoryGoods)goods;
			return new AccessoryGoods(accessoryGoods.Id, accessoryGoods.BaseCount, accessoryGoods.GainCount, num6, num6, num7);
		}

		public static int GetRepairCost(this Goods goods)
		{
			if (goods == null)
			{
				return 0;
			}
			if (!goods.HasDurability())
			{
				return 0;
			}
			TEquipment tEquipment = TEquipment.Table[goods.Id];
			return (goods.GetMaxDurability() - (int)goods.GetCurrentDurability()) * tEquipment.RepairCostPerDurability;
		}

		public static int[] GetPrefixes(this Goods goods)
		{
			if (goods is ArtifactGoods artifactGoods)
			{
				return artifactGoods.Prefixes;
			}
			return Array.Empty<int>();
		}

		public static EArtifactCategory GetArtifactCategory(this Goods goods)
		{
			if (goods is ArtifactGoods)
			{
				return TEquipment.Table[goods.Id].ArtifactCategory;
			}
			return EArtifactCategory.None;
		}

		public static Dictionary<EStatType, float> GetArtifactStats(this Goods goods)
		{
			if (goods is ArtifactGoods artifactGoods)
			{
				int[] source = artifactGoods.Prefixes ?? Array.Empty<int>();
				return source.SelectMany((int prefixId) => TArtifactPrefix.Table[prefixId].Stats)
					.GroupBy((Stat s) => s.StatType)
					.Select((IGrouping<EStatType, Stat> g) => (g.Key, g.Sum((Stat s) => s.Value)))
					.Where(((EStatType, float) t) => t.Item2 != 0f)
					.ToDictionary(((EStatType, float) t) => t.Item1, ((EStatType, float) t) => t.Item2);
			}
			return new Dictionary<EStatType, float>();
		}

		public static Dictionary<EStatType, float> GetEquipmentStat(this Goods goods, bool checkBrokenStat)
		{
			if (goods is ArtifactGoods)
			{
				return goods.GetArtifactStats();
			}
			if (goods.Type.GoodsCategory() != EGoodsCategory.Equipment)
			{
				return new Dictionary<EStatType, float>();
			}
			TEquipment tEquipment = TEquipment.Table[goods.Id];
			Stat[] array = ((checkBrokenStat && goods.IsBrokenEquipment()) ? tEquipment.BrokenStats : tEquipment.Stats);
			Dictionary<EStatType, float> dictionary = new Dictionary<EStatType, float>();
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					Stat stat = array[i];
					if (stat.Value != 0f)
					{
						dictionary[stat.StatType] = dictionary.GetValueOrDefault(stat.StatType) + stat.Value;
					}
				}
			}
			return dictionary;
		}

		public static int GetMaxDurability(this Goods goods)
		{
			if (goods is WeaponGoods weaponGoods)
			{
				return weaponGoods.MaxDurability;
			}
			if (goods is AccessoryGoods accessoryGoods)
			{
				return accessoryGoods.MaxDurability;
			}
			return 0;
		}

		public static float GetCurrentDurability(this Goods goods)
		{
			if (goods is WeaponGoods weaponGoods)
			{
				return weaponGoods.CurrentDurability;
			}
			if (goods is AccessoryGoods accessoryGoods)
			{
				return accessoryGoods.CurrentDurability;
			}
			return 0f;
		}

		public static float GetEffectiveMaxDurability(this Goods goods)
		{
			if (goods is WeaponGoods weaponGoods)
			{
				return (float)weaponGoods.MaxDurabilityFraction / 100f + (float)weaponGoods.MaxDurability;
			}
			if (goods is AccessoryGoods accessoryGoods)
			{
				return (float)accessoryGoods.MaxDurabilityFraction / 100f + (float)accessoryGoods.MaxDurability;
			}
			return 0f;
		}

		public static string ToSimpleString(this Goods goods)
		{
			if (goods == null)
			{
				return "null";
			}
			return string.Format("{0}:{1}:{2}", goods.Type, goods.Id, goods.GainCount + goods.BaseCount);
		}

		public static GoodsData ToData(this Goods goods)
		{
			GoodsData result = default(GoodsData);
			result.Type = goods.Type;
			result.Id = goods.Id;
			result.Count = goods.GainCount + goods.BaseCount;
			return result;
		}

		public static Goods CloneWithDurability(this Goods goods, float newCurrentDurability)
		{
			if (goods is WeaponGoods weapon)
			{
				return weapon.CloneWithDurability(newCurrentDurability);
			}
			if (goods is AccessoryGoods accessory)
			{
				return accessory.CloneWithDurability(newCurrentDurability);
			}
			return goods;
		}

		public static WeaponGoods CloneWithDurability(this WeaponGoods weapon, float newCurrentDurability)
		{
			return new WeaponGoods(weapon.Id, weapon.BaseCount, weapon.GainCount, weapon.MaxDurability, newCurrentDurability, weapon.MaxDurabilityFraction);
		}

		public static AccessoryGoods CloneWithDurability(this AccessoryGoods accessory, float newCurrentDurability)
		{
			return new AccessoryGoods(accessory.Id, accessory.BaseCount, accessory.GainCount, accessory.MaxDurability, newCurrentDurability, accessory.MaxDurabilityFraction);
		}

		public static bool CheckFilter(this Goods goods, EGoodsType[] allowedTypes, EArtifactCategory[] artifactFilterTypes, EKeyItemType[] keyItemFilterTypes)
		{
			if (goods == null)
			{
				return false;
			}
			EArtifactCategory artifactCategory = EArtifactCategory.None;
			if (goods is ArtifactGoods)
			{
				artifactCategory = TEquipment.Table[goods.Id].ArtifactCategory;
			}
			if (artifactFilterTypes != null && !artifactFilterTypes.Contains(EArtifactCategory.All) && !artifactFilterTypes.Any((EArtifactCategory t) => t == artifactCategory))
			{
				return false;
			}
			if (allowedTypes != null)
			{
				return allowedTypes.Any((EGoodsType t) => t == goods.Type);
			}
			if (keyItemFilterTypes == null)
			{
				return true;
			}
			if (goods.Type.GoodsCategory() != EGoodsCategory.Item)
			{
				return false;
			}
			EKeyItemType[] keyTypes = TItem.Table[goods.Id].KeyTypes;
			if (keyTypes != null && keyTypes.Any())
			{
				return keyTypes.Any((EKeyItemType k) => keyItemFilterTypes.Contains(k));
			}
			return false;
		}
	}
}
