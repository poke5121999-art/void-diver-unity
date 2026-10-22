using System.Collections.Generic;
using System.Text.RegularExpressions;
using GameCommon.Data;
using GameCommon.Enum;

namespace GameCommon.Table
{
	public static class Extensions
	{
		internal static readonly Regex ExpressionRegex;

		public static SummonMonsterInfo GetRandomInfo(this IList<SummonMonsterInfo> summonMonsterInfos)
		{
			return null;
		}

		public static int GetChapterNumber(this TCampaign campaign)
		{
			return 0;
		}

		public static string GetChapterString(this TCampaign campaign)
		{
			return null;
		}

		public static GoodsData[] GetRewards(this TCampaign campaign, bool isFirstClear)
		{
			return null;
		}

		public static int GetTutorialId(this CampaignTable table)
		{
			return 0;
		}

		public static Dictionary<EStatType, float> CreateBaseStats(this TCharacter character)
		{
			return null;
		}

		public static int GetDefaultWeaponId(this TCharacter character)
		{
			return 0;
		}

		public static List<Goods> PickRewards(this DropRewardTable table, int dropRewardGroupId, int campaignId, int[] missionIds, EDifficulty difficulty, HashSet<EWeaponType> partyWeaponTypes)
		{
			return null;
		}

		public static List<Goods> PickRewardsFromSingleData(this DropRewardTable table, int dropRewardDataId, int campaignId, int[] missionIds, EDifficulty difficulty, HashSet<EWeaponType> partyWeaponTypes)
		{
			return null;
		}

		private static int GetVariantDifficulty(int campaignId)
		{
			return 0;
		}

		private static void PickRewardsRaw(List<Goods> rewards, int dropRewardDataId, HashSet<int> eventItemIds, EDifficulty difficulty, int variantDifficulty, HashSet<EWeaponType> partyWeaponTypes)
		{
		}

		private static List<Goods> MergeRewards(List<Goods> rewards)
		{
			return null;
		}

		private static Goods StackGoodsByGain(EGoodsType type, int id, long totalGain)
		{
			return null;
		}

		private static float GetDifficultyGradeRate(TDropReward data, EGoodsGradeType grade, EDifficulty difficulty)
		{
			return 0f;
		}

		private static bool TryGetWeaponDropBonusRate(GoodsData goodsData, HashSet<EWeaponType> partyWeaponTypes, out float bonusRate)
		{
			bonusRate = default;
			return false;
		}

		private static float GetDifficultyArtifactEpicRate(TDropReward data, EDifficulty difficulty)
		{
			return 0f;
		}

		private static HashSet<int> GetEventItemIds(int campaignId, int[] missionIds)
		{
			return null;
		}

		public static List<int> GetEffectBuffIds(this EquipmentTable table, IEnumerable<int> equipmentIds)
		{
			return null;
		}

		public static List<int> GetSetBuffIds(this EquipmentTable table, IEnumerable<int> equipmentIds)
		{
			return null;
		}

		public static List<int> GetEffectSkillIds(this EquipmentTable table, IEnumerable<int> equipmentIds)
		{
			return null;
		}

		public static int[] PickPrefixes(this TEquipment data)
		{
			return null;
		}

		public static bool IsArtifact(this TEquipment data)
		{
			return false;
		}

		public static bool IsWeapon(this TEquipment data)
		{
			return false;
		}

		public static bool IsAccessory(this TEquipment data)
		{
			return false;
		}

		private static (int, int) GetPrefixCountRange(this TEquipment data)
		{
			return default;
		}

		public static int GetRandomPrefixCount(this TEquipment data)
		{
			return 0;
		}

		public static int GetMaxLegitPrefixCount(this TEquipment data)
		{
			return 0;
		}

		public static int GetMaxDurability(this TEquipment data, float minRatio = 1f)
		{
			return 0;
		}

		public static Dictionary<EStatType, float> GetBaseStats(this TExtraUnit extraUnit)
		{
			return null;
		}

		public static bool IsRecoveryItem(this TItem item)
		{
			return false;
		}

		public static string Format(this TLocalizedText data, params object[] args)
		{
			return null;
		}

		public static string FormatWithExpression(this TLocalizedText data)
		{
			return null;
		}

		public static Dictionary<EStatType, float> GetBaseStats(this TMonster monster, EDifficulty difficulty, int variantDifficulty)
		{
			return null;
		}

		public static TMonsterSpawn Pick(this TMonsterSpawn[] monsterSpawns, bool isLightOn)
		{
			return null;
		}

		public static int PickRandomParadoxId(this ParadoxTable table, int previousLevel, int[] excludeIds, bool isPositive)
		{
			return 0;
		}

		public static string GetParadoxLevelText(this TParadox data)
		{
			return null;
		}

		public static GoodsData GetRemovalGoodsData(this TParadox data)
		{
			return default;
		}

		public static GoodsData GetLockGoodsData(this TParadox data)
		{
			return default;
		}

		public static GoodsData GetUnlockGoodsData(this TParadox data)
		{
			return default;
		}

		public static string GetParadoxNameWithLevel(this TParadox data)
		{
			return null;
		}

		public static int GetMaxLevel(this ParadoxLevelTable table)
		{
			return 0;
		}

		public static int GetNextRandomLevel(this TParadoxLevel data)
		{
			return 0;
		}

		public static float GetBaseAtkStat(this TRewardBox rewardBox, EDifficulty difficulty, int variantDifficulty)
		{
			return 0f;
		}

		public static TRewardBox Pick(this TRewardBox[] rewardBox)
		{
			return null;
		}

		public static EventCondition[] GetUnlockConditions(this TTalent data)
		{
			return null;
		}

		public static float GetBaseAtkStat(this TTrap trap, EDifficulty difficulty, int variantDifficulty)
		{
			return 0f;
		}
	}
}
