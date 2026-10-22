using System.Collections.Generic;
using GameCommon.Enum;

namespace GameCommon.Data
{
	public static class GoodsExtensions
	{
		private const int DurabilityFractionScale = 100;

		public static bool IsCurrency(this Goods goods)
		{
			return false;
		}

		public static bool IsItem(this Goods goods)
		{
			return false;
		}

		public static bool IsEquipment(this Goods goods)
		{
			return false;
		}

		public static bool IsBag(this Goods goods)
		{
			return false;
		}

		public static bool IsGold(this Goods goods)
		{
			return false;
		}

		public static bool IsCoin(this Goods goods)
		{
			return false;
		}

		public static bool IsExp(this Goods goods)
		{
			return false;
		}

		public static bool IsConsumable(this Goods goods)
		{
			return false;
		}

		public static bool IsValuable(this Goods goods)
		{
			return false;
		}

		public static bool IsMisc(this Goods goods)
		{
			return false;
		}

		public static bool IsNote(this Goods goods)
		{
			return false;
		}

		public static bool IsBlueprint(this Goods goods)
		{
			return false;
		}

		public static bool IsMusicDisc(this Goods goods)
		{
			return false;
		}

		public static bool IsWeapon(this Goods goods)
		{
			return false;
		}

		public static bool IsAccessory(this Goods goods)
		{
			return false;
		}

		public static bool IsArtifact(this Goods goods)
		{
			return false;
		}

		public static bool HasDurability(this Goods goods)
		{
			return false;
		}

		public static bool IsEventItem(this Goods goods)
		{
			return false;
		}

		public static bool IsQuestItem(this Goods goods)
		{
			return false;
		}

		public static string GetName(this Goods goods)
		{
			return null;
		}

		public static EGoodsGradeType GetGrade(this Goods goods)
		{
			return EGoodsGradeType.None;
		}

		public static int GetArtifactPrice(this Goods goods)
		{
			return 0;
		}

		public static int GetWorth(this Goods goods)
		{
			return 0;
		}

		public static int GetCampaignLevel(this Goods goods)
		{
			return 0;
		}

		public static bool IsBrokenEquipment(this Goods goods)
		{
			return false;
		}

		public static float GetDurabilityRatio(this Goods goods)
		{
			return 0f;
		}

		public static int GetRepairCost(this Goods goods)
		{
			return 0;
		}

		public static bool CanRepair(this Goods goods)
		{
			return false;
		}

		public static Goods Repair(this Goods goods, bool preventMaxDurabilityLoss = false)
		{
			return null;
		}

		public static int[] GetPrefixes(this Goods goods)
		{
			return null;
		}

		public static EArtifactCategory GetArtifactCategory(this Goods goods)
		{
			return EArtifactCategory.None;
		}

		public static Dictionary<EStatType, float> GetArtifactStats(this Goods goods)
		{
			return null;
		}

		public static Dictionary<EStatType, float> GetEquipmentStat(this Goods goods, bool checkBrokenStat)
		{
			return null;
		}

		public static int GetMaxDurability(this Goods goods)
		{
			return 0;
		}

		public static float GetCurrentDurability(this Goods goods)
		{
			return 0f;
		}

		public static float GetEffectiveMaxDurability(this Goods goods)
		{
			return 0f;
		}

		public static string ToSimpleString(this Goods goods)
		{
			return null;
		}

		public static GoodsData ToData(this Goods goods)
		{
			return default;
		}

		public static Goods CloneWithDurability(this Goods goods, float newCurrentDurability)
		{
			return null;
		}

		public static WeaponGoods CloneWithDurability(this WeaponGoods weapon, float newCurrentDurability)
		{
			return null;
		}

		public static AccessoryGoods CloneWithDurability(this AccessoryGoods accessory, float newCurrentDurability)
		{
			return null;
		}

		public static bool CheckFilter(this Goods goods, EGoodsType[] allowedTypes, EArtifactCategory[] artifactFilterTypes, EKeyItemType[] keyItemFilterTypes)
		{
			return false;
		}
	}
}
