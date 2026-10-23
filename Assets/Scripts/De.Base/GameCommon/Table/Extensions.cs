using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GameCommon.Data;
using GameCommon.Enum;
using SomeNet.Collections;
using SomeNet.Utility;

namespace GameCommon.Table
{
	public static class Extensions
	{
		internal static readonly Regex ExpressionRegex = new Regex("\\{(\\w+):(\\w+)\\}", RegexOptions.Compiled);

		public static SummonMonsterInfo GetRandomInfo(this IList<SummonMonsterInfo> summonMonsterInfos)
		{
			if (summonMonsterInfos.Count == 0)
			{
				return null;
			}
			int total = 0;
			foreach (SummonMonsterInfo info in summonMonsterInfos)
			{
				total += info.Probability;
			}
			if (total < 1 || summonMonsterInfos.Count == 1)
			{
				return summonMonsterInfos[0];
			}
			float roll = RandomUtility.GetRandomFloat(0f, total);
			int cumulative = 0;
			foreach (SummonMonsterInfo info in summonMonsterInfos)
			{
				cumulative += info.Probability;
				if ((float)cumulative > roll)
				{
					return info;
				}
			}
			return null;
		}

		public static int GetChapterNumber(this TCampaign campaign)
		{
			return TCampaign.GroupTable[campaign.Category].Count(_ => _.Id <= campaign.Id);
		}

		public static string GetChapterString(this TCampaign campaign)
		{
			if (campaign.Category != ECampaignCategory.Story)
			{
				return string.Empty;
			}
			return string.Format("CHP {0:D2}", campaign.GetChapterNumber());
		}

		public static GoodsData[] GetRewards(this TCampaign campaign, bool isFirstClear)
		{
			if (campaign.Category != ECampaignCategory.Normal && isFirstClear)
			{
				return campaign.FirstRewards;
			}
			return campaign.RepeatRewards;
		}

		public static int GetTutorialId(this CampaignTable table)
		{
			return table.First(_ => _.Value.Category == ECampaignCategory.Tutorial).Key;
		}

		public static Dictionary<EStatType, float> CreateBaseStats(this TCharacter character)
		{
			Dictionary<EStatType, float> stats = new Dictionary<EStatType, float>();
			stats.Add(EStatType.Atk, character.Atk);
			stats.Add(EStatType.Def, character.Def);
			stats.Add(EStatType.MoveSpeed, character.MoveSpeed);
			stats.Add(EStatType.AtkSpeed, character.AtkSpeed);
			stats.Add(EStatType.HpMax, character.Hp);
			stats.Add(EStatType.HpRegen, character.HpRegen);
			stats.Add(EStatType.StaminaMax, character.Stamina);
			stats.Add(EStatType.StaminaRegen, character.StaminaRegen);
			stats.Add(EStatType.RunStaminaCost, character.RunStaminaCost);
			stats.Add(EStatType.RunIncreaseSpeed, character.RunIncreaseSpeed);
			stats.Add(EStatType.FireResistancePercent, character.FireResistancePercent);
			stats.Add(EStatType.WaterResistancePercent, character.WaterResistancePercent);
			stats.Add(EStatType.WindResistancePercent, character.WindResistancePercent);
			stats.Add(EStatType.CriticalChancePercent, TConst.DefaultCriticalChancePercent.FloatValue);
			stats.Add(EStatType.CriticalDamagePercent, TConst.DefaultCriticalDamagePercent.FloatValue);
			stats.Add(EStatType.WeaknessDamagePercent, TConst.DefaultWeaknessDamagePercent.FloatValue);
			stats.Add(EStatType.BackAttackDamagePercent, TConst.DefaultCharacterBackAttackDamagePercent.FloatValue);
			stats.Add(EStatType.CorruptionMax, TConst.CorruptionMax.IntValue);
			return stats;
		}

		public static int GetDefaultWeaponId(this TCharacter character)
		{
			TEquipment equipment = TEquipment.Table.Values.Where(_ => _.WeaponType == character.WeaponType).FirstOrDefault(_ => _.IsDefault);
			if (equipment == null)
			{
				return 0;
			}
			return equipment.Id;
		}

		public static List<Goods> PickRewards(this DropRewardTable table, int dropRewardGroupId, int campaignId, int[] missionIds, EDifficulty difficulty, HashSet<EWeaponType> partyWeaponTypes)
		{
			List<Goods> rewards = new List<Goods>();
			HashSet<int> eventItemIds = GetEventItemIds(campaignId, missionIds);
			int variantDifficulty = GetVariantDifficulty(campaignId);
			foreach (TDropReward dropReward in TDropReward.GroupTable[dropRewardGroupId])
			{
				PickRewardsRaw(rewards, dropReward.Id, eventItemIds, difficulty, variantDifficulty, partyWeaponTypes);
			}
			return MergeRewards(rewards);
		}

		public static List<Goods> PickRewardsFromSingleData(this DropRewardTable table, int dropRewardDataId, int campaignId, int[] missionIds, EDifficulty difficulty, HashSet<EWeaponType> partyWeaponTypes)
		{
			List<Goods> rewards = new List<Goods>();
			HashSet<int> eventItemIds = GetEventItemIds(campaignId, missionIds);
			int variantDifficulty = GetVariantDifficulty(campaignId);
			PickRewardsRaw(rewards, dropRewardDataId, eventItemIds, difficulty, variantDifficulty, partyWeaponTypes);
			return MergeRewards(rewards);
		}

		private static int GetVariantDifficulty(int campaignId)
		{
			if (campaignId < 1)
			{
				return 0;
			}
			return TCampaign.Table[campaignId].VariantDifficulty;
		}

		private static void PickRewardsRaw(List<Goods> rewards, int dropRewardDataId, HashSet<int> eventItemIds, EDifficulty difficulty, int variantDifficulty, HashSet<EWeaponType> partyWeaponTypes)
		{
			TDropReward dropRewardData = TDropReward.Table[dropRewardDataId];
			bool applyDifficultyRate = !dropRewardData.PickGradeBeforeProbability;
			float normalBp = dropRewardData.NormalBp;
			float artifactBp = dropRewardData.ArtifactBp;
			int artifactPickOrder = 0;
			int goodsPickOrder = 0;
			for (int i = 0; i < dropRewardData.PickCount; i++)
			{
				float roll = RandomUtility.GetRandomFloat(0f, 10000f);
				if (roll <= artifactBp + normalBp)
				{
					if (normalBp > roll)
					{
						PickGoods(dropRewardData.Id, eventItemIds, goodsPickOrder);
						goodsPickOrder++;
					}
					else
					{
						PickArtifact(dropRewardData.Id, artifactPickOrder);
						artifactPickOrder++;
					}
				}
			}

			void PickArtifact(int dataId, int pickOrder)
			{
				TDropReward data = TDropReward.Table[dataId];
				List<TDropRewardArtifactProbability> probabilityDatas = TDropRewardArtifactProbability.GroupTable[data.Id].ToList();
				int selectPickOrder = Math.Min(pickOrder, probabilityDatas.Max(_ => _.PickOrder));
				probabilityDatas.RemoveAll(_ => _.PickOrder != selectPickOrder);
				float rareRate = GetDifficultyArtifactEpicRate(data, difficulty);
				float artifactRoll = RandomUtility.GetRandomFloat(0f, probabilityDatas.Sum(_ => _.Grade > EGoodsGradeType.Elite ? _.Weight * rareRate : _.Weight));
				foreach (TDropRewardArtifactProbability probabilityData in probabilityDatas)
				{
					float weight = probabilityData.Weight;
					if (probabilityData.Grade > EGoodsGradeType.Elite)
					{
						weight *= rareRate;
					}
					artifactRoll -= weight;
					if (!(0f <= artifactRoll))
					{
						for (int j = 0; j < probabilityData.Count; j++)
						{
							int artifactId = probabilityData.Grade.GetRandomArtifactId();
							if (artifactId >= 0)
							{
								TEquipment equipData = TEquipment.Table[artifactId];
								rewards.Add(new ArtifactGoods(artifactId, 0L, 1L, equipData.PickPrefixes(), (byte)variantDifficulty));
							}
						}
						return;
					}
				}
			}

			void PickGoods(int dataId, HashSet<int> evtItemIds, int pickOrder)
			{
				List<TDropRewardProbability> probabilityDatas = TDropRewardProbability.GroupTable[dataId].Where(_ =>
				{
					if (_.GoodsData.Type.GoodsCategory() != EGoodsCategory.Item)
					{
						return true;
					}
					if (!TItem.Table[_.GoodsData.Id].IsEventItem)
					{
						return true;
					}
					return evtItemIds.Contains(_.GoodsData.Id);
				}).ToList();
				if (!probabilityDatas.Any())
				{
					return;
				}
				int selectPickOrder = Math.Min(pickOrder, probabilityDatas.Max(_ => _.PickOrder));
				probabilityDatas.RemoveAll(_ => _.PickOrder != selectPickOrder);
				if (dropRewardData.PickGradeBeforeProbability)
				{
					EGoodsGradeType grade = PickGrade(dataId);
					probabilityDatas.RemoveAll(_ => _.GoodsData.GetGrade() != grade);
				}
				float weaponWeightBalance = CalculateWeaponWeightBalance(probabilityDatas);
				float goodsRoll = RandomUtility.GetRandomFloat(0f, probabilityDatas.Sum(_ => GetAdjustedWeight(_, weaponWeightBalance)));
				foreach (TDropRewardProbability probabilityData in probabilityDatas)
				{
					goodsRoll -= GetAdjustedWeight(probabilityData, weaponWeightBalance);
					if (!(0f <= goodsRoll))
					{
						GoodsData goodsData = probabilityData.GoodsData;
						Goods goods;
						if (goodsData.Type.GoodsCategory() != EGoodsCategory.Equipment)
						{
							goods = GoodsCreator.MakeGoods(goodsData.Type, goodsData.Id, 0L, goodsData.Count);
						}
						else
						{
							TEquipment equipData = TEquipment.Table[goodsData.Id];
							float minRatio = TConst.RandomDurabilityMinimumRate.FloatValue;
							switch (equipData.GoodsType)
							{
							case EGoodsType.Weapon:
								goods = CreateRolledWeapon(goodsData.Id, goodsData.Count, equipData, minRatio);
								break;
							case EGoodsType.Accessory:
								goods = CreateRolledAccessory(goodsData.Id, goodsData.Count, equipData, minRatio);
								break;
							case EGoodsType.Artifact:
								goods = new ArtifactGoods(goodsData.Id, 0L, goodsData.Count, equipData.PickPrefixes(), (byte)variantDifficulty);
								break;
							default:
								throw new InvalidOperationException(string.Format("Invalid equipment GoodsType: {0}", equipData.GoodsType));
							}
						}
						rewards.Add(goods);
						return;
					}
				}
			}

			EGoodsGradeType PickGrade(int dataId)
			{
				TDropReward data = TDropReward.Table[dataId];
				(EGoodsGradeType, float)[] gradeWeights = new (EGoodsGradeType, float)[7];
				gradeWeights[0] = (EGoodsGradeType.None, data.NoneWeight);
				gradeWeights[1] = (EGoodsGradeType.Normal, data.NormalWeight * GetDifficultyGradeRate(data, EGoodsGradeType.Normal, difficulty));
				gradeWeights[2] = (EGoodsGradeType.Rare, data.RareWeight * GetDifficultyGradeRate(data, EGoodsGradeType.Rare, difficulty));
				gradeWeights[3] = (EGoodsGradeType.Elite, data.EliteWeight * GetDifficultyGradeRate(data, EGoodsGradeType.Elite, difficulty));
				gradeWeights[4] = (EGoodsGradeType.Epic, data.EpicWeight * GetDifficultyGradeRate(data, EGoodsGradeType.Epic, difficulty));
				gradeWeights[5] = (EGoodsGradeType.Legend, data.LegendWeight * GetDifficultyGradeRate(data, EGoodsGradeType.Legend, difficulty));
				gradeWeights[6] = (EGoodsGradeType.Unique, data.UniqueWeight * GetDifficultyGradeRate(data, EGoodsGradeType.Unique, difficulty));
				float gradeRoll = RandomUtility.GetRandomFloat(0f, gradeWeights.Sum(_ => _.Item2));
				for (int k = 0; k < gradeWeights.Length; k++)
				{
					gradeRoll -= gradeWeights[k].Item2;
					if (gradeRoll < 0f)
					{
						return gradeWeights[k].Item1;
					}
				}
				return EGoodsGradeType.None;
			}

			float GetAdjustedWeight(TDropRewardProbability probabilityData, float weaponWeightBalance)
			{
				float weight = probabilityData.Weight;
				if (applyDifficultyRate)
				{
					weight *= GetDifficultyGradeRate(dropRewardData, probabilityData.GoodsData.GetGrade(), difficulty);
				}
				if (TryGetWeaponDropBonusRate(probabilityData.GoodsData, partyWeaponTypes, out float bonusRate))
				{
					weight = weight * bonusRate * weaponWeightBalance;
				}
				return weight;
			}

			float CalculateWeaponWeightBalance(List<TDropRewardProbability> probabilityDatas)
			{
				if (partyWeaponTypes == null || partyWeaponTypes.Count == 0)
				{
					return 1f;
				}
				float totalWeight = 0f;
				float bonusWeight = 0f;
				foreach (TDropRewardProbability probabilityData in probabilityDatas)
				{
					if (TryGetWeaponDropBonusRate(probabilityData.GoodsData, partyWeaponTypes, out float bonusRate))
					{
						float weight = probabilityData.Weight;
						if (applyDifficultyRate)
						{
							weight *= GetDifficultyGradeRate(dropRewardData, probabilityData.GoodsData.GetGrade(), difficulty);
						}
						totalWeight += weight;
						bonusWeight += weight * bonusRate;
					}
				}
				if (totalWeight > 0f && bonusWeight > 0f)
				{
					return totalWeight / bonusWeight;
				}
				return 1f;
			}

			WeaponGoods CreateRolledWeapon(int id, long dropCount, TEquipment equipData, float minRatio)
			{
				int maxDurability = equipData.GetMaxDurability(minRatio);
				return new WeaponGoods(id, 0L, dropCount, maxDurability, maxDurability);
			}

			AccessoryGoods CreateRolledAccessory(int id, long dropCount, TEquipment equipData, float minRatio)
			{
				int maxDurability = equipData.GetMaxDurability(minRatio);
				return new AccessoryGoods(id, 0L, dropCount, maxDurability, maxDurability);
			}
		}

		private static List<Goods> MergeRewards(List<Goods> rewards)
		{
			List<Goods> equipments = rewards.Where(_ => _ != null && _.Type.GoodsCategory() == EGoodsCategory.Equipment).Select(_ => _.Clone()).ToList();
			List<Goods> stacked = rewards.Where(_ => _ != null && _.Type.GoodsCategory() != EGoodsCategory.Equipment).GroupBy(_ => new { _.Type, _.Id }).Select(grp => StackGoodsByGain(grp.Key.Type, grp.Key.Id, grp.Sum(_ => _.GainCount + _.BaseCount))).ToList();
			return equipments.Concat(stacked).ToList();
		}

		private static Goods StackGoodsByGain(EGoodsType type, int id, long totalGain)
		{
			switch (type)
			{
			case EGoodsType.Gold:
				return new GoldGoods(0L, totalGain);
			case EGoodsType.Coin:
				return new CoinGoods(0L, totalGain);
			case EGoodsType.Exp:
				return new ExpGoods(0L, totalGain);
			case EGoodsType.Consumable:
				return new ConsumableGoods(id, 0L, totalGain);
			case EGoodsType.Valuable:
				return new ValuableGoods(id, 0L, totalGain);
			case EGoodsType.Misc:
				return new MiscGoods(id, 0L, totalGain);
			case EGoodsType.Note:
				return new NoteGoods(id, 0L, totalGain);
			case EGoodsType.Blueprint:
				return new BlueprintGoods(id, 0L, totalGain);
			case EGoodsType.MusicDisc:
				return new MusicDiscGoods(id, 0L, totalGain);
			default:
				throw new InvalidOperationException(string.Format("Cannot stack non-stackable type: {0}", type));
			}
		}

		private static float GetDifficultyGradeRate(TDropReward data, EGoodsGradeType grade, EDifficulty difficulty)
		{
			if (data.IgnoreDifficulty)
			{
				return 1f;
			}
			TDifficulty difficultyData = TDifficulty.Table[difficulty];
			int percent;
			switch (grade)
			{
			case EGoodsGradeType.Normal:
				percent = difficultyData.NormalItemPickPercent;
				break;
			case EGoodsGradeType.Rare:
				percent = difficultyData.RareItemPickPercent;
				break;
			case EGoodsGradeType.Elite:
				percent = difficultyData.EliteItemPickPercent;
				break;
			case EGoodsGradeType.Epic:
				percent = difficultyData.EpicItemPickPercent;
				break;
			case EGoodsGradeType.Legend:
				percent = difficultyData.LegendItemPickPercent;
				break;
			case EGoodsGradeType.Unique:
				percent = difficultyData.UniqueItemPickPercent;
				break;
			default:
				percent = 100;
				break;
			}
			return (float)percent / 100f;
		}

		private static bool TryGetWeaponDropBonusRate(GoodsData goodsData, HashSet<EWeaponType> partyWeaponTypes, out float bonusRate)
		{
			bonusRate = 1f;
			if (goodsData.Type.GoodsCategory() == EGoodsCategory.Equipment && TEquipment.Table.TryGetValue(goodsData.Id, out TEquipment equipment) && equipment.GoodsType == EGoodsType.Weapon)
			{
				if (partyWeaponTypes != null && partyWeaponTypes.Contains(equipment.WeaponType))
				{
					bonusRate = (float)TConst.CharacterWeaponTypeDropWeightBonusPercent.IntValue / 100f + 1f;
				}
				return true;
			}
			return false;
		}

		private static float GetDifficultyArtifactEpicRate(TDropReward data, EDifficulty difficulty)
		{
			if (data.IgnoreDifficulty)
			{
				return 1f;
			}
			return (float)TDifficulty.Table[difficulty].EpicArtifactPickPercent / 100f;
		}

		private static HashSet<int> GetEventItemIds(int campaignId, int[] missionIds)
		{
			HashSet<int> eventItemIds = new HashSet<int>();
			if (campaignId > 0)
			{
				TCampaignTask.GroupTable[campaignId].Where(_ => _.EventCondition.Type.IsAssignItemCountCondition()).Select(_ => (int)_.EventCondition.Value1).ForEach(id => eventItemIds.Add(id));
			}
			if (missionIds != null && missionIds.Any())
			{
				missionIds.Select(id => TMission.Table[id]).Where(_ => _.EventCondition.Type.IsAssignItemCountCondition()).Select(_ => (int)_.EventCondition.Value1).ForEach(id => eventItemIds.Add(id));
			}
			return eventItemIds;
		}

		public static List<int> GetEffectBuffIds(this EquipmentTable table, IEnumerable<int> equipmentIds)
		{
			List<int> buffIds = new List<int>();
			if (equipmentIds != null && equipmentIds.Any())
			{
				Dictionary<int, int> effectLevels = equipmentIds
					.Where(_ => _ > 0)
					.SelectMany(_ => table[_].EquipmentEffectIds ?? Array.Empty<int>())
					.Where(_ => TEquipmentEffect.Table[_].BuffId > 0)
					.GroupBy(_ => TEquipmentEffect.Table[_].GroupId)
					.ToDictionary(_ => _.Key, _ => _.Sum(effectId => TEquipmentEffect.Table[effectId].Level));
				foreach (KeyValuePair<int, int> pair in effectLevels)
				{
					pair.Deconstruct(out int groupId, out int totalLevel);
					int level = Math.Min(totalLevel, TEquipmentEffect.GroupTable[groupId].Max(_ => _.Level));
					buffIds.Add(TEquipmentEffect.SubTable[groupId, level].BuffId);
				}
			}
			return buffIds;
		}

		public static List<int> GetSetBuffIds(this EquipmentTable table, IEnumerable<int> equipmentIds)
		{
			List<int> buffIds = new List<int>();
			if (equipmentIds != null && equipmentIds.Any())
			{
				Dictionary<int, int> setCounts = equipmentIds
					.Where(_ => _ > 0)
					.Where(_ => table[_].EquipmentSetGroupId > 0)
					.GroupBy(_ => table[_].EquipmentSetGroupId)
					.ToDictionary(_ => _.Key, _ => _.Count());
				foreach (KeyValuePair<int, int> pair in setCounts)
				{
					pair.Deconstruct(out int groupId, out int totalCount);
					if (TEquipmentSet.GroupTable.ContainsKey(groupId))
					{
						int maxCount = TEquipmentSet.GroupTable[groupId].Select(_ => _.Count).Where(_ => _ <= totalCount).DefaultIfEmpty(0).Max();
						if (maxCount != 0)
						{
							TEquipmentSet setData = TEquipmentSet.SubTable[groupId, maxCount];
							if (setData.BuffId > 0)
							{
								buffIds.Add(setData.BuffId);
							}
						}
					}
				}
			}
			return buffIds;
		}

		public static List<int> GetEffectSkillIds(this EquipmentTable table, IEnumerable<int> equipmentIds)
		{
			List<int> skillIds = new List<int>();
			if (equipmentIds != null && equipmentIds.Any())
			{
				Dictionary<int, int> effectLevels = equipmentIds
					.Where(_ => _ > 0)
					.SelectMany(_ => table[_].EquipmentEffectIds ?? Array.Empty<int>())
					.GroupBy(_ => TEquipmentEffect.Table[_].GroupId)
					.ToDictionary(_ => _.Key, _ => _.Sum(effectId => TEquipmentEffect.Table[effectId].Level));
				foreach (KeyValuePair<int, int> pair in effectLevels)
				{
					pair.Deconstruct(out int groupId, out int totalLevel);
					int level = Math.Min(totalLevel, TEquipmentEffect.GroupTable[groupId].Max(_ => _.Level));
					int[] effectSkillIds = TEquipmentEffect.SubTable[groupId, level].SkillIds;
					if (effectSkillIds != null && effectSkillIds.Any())
					{
						skillIds.AddRange(effectSkillIds);
					}
				}
			}
			return skillIds;
		}

		public static int[] PickPrefixes(this TEquipment data)
		{
			if (data.GoodsType != EGoodsType.Artifact)
			{
				return null;
			}
			HashSet<int> prefixIds = new HashSet<int>();
			if (data.SpecifiedArtifactPrefixIds != null)
			{
				prefixIds.UnionWith(data.SpecifiedArtifactPrefixIds);
			}
			int prefixCount = data.GetRandomPrefixCount();
			for (int i = 0; i < prefixCount; i++)
			{
				bool isNegative = (float)RandomUtility.GetRandomInt(0, 100) < TConst.ArtifactPrefixNegativePercent.FloatValue;
				List<TArtifactPrefix> candidates = TArtifactPrefix.Table.Values
					.Where(_ => isNegative ? (_.PriceMultiplier <= 0f && _.PriceMultiplier != 0f) : (0f <= _.PriceMultiplier))
					.Where(_ => !prefixIds.Contains(_.Id))
					.ToList();
				int prefixRoll = RandomUtility.GetRandomInt(0, candidates.Sum(_ => _.Weight));
				foreach (TArtifactPrefix candidate in candidates)
				{
					if (prefixIds.Contains(candidate.Id))
					{
						continue;
					}
					prefixRoll -= candidate.Weight;
					if (prefixRoll <= 0)
					{
						prefixIds.Add(candidate.Id);
						break;
					}
				}
			}
			return prefixIds.ToArray();
		}

		public static bool IsArtifact(this TEquipment data)
		{
			return data.GoodsType == EGoodsType.Artifact;
		}

		public static bool IsWeapon(this TEquipment data)
		{
			return data.GoodsType == EGoodsType.Weapon;
		}

		public static bool IsAccessory(this TEquipment data)
		{
			return data.GoodsType == EGoodsType.Accessory;
		}

		private static (int, int) GetPrefixCountRange(this TEquipment data)
		{
			if (data.GoodsType != EGoodsType.Artifact)
			{
				return (0, 0);
			}
			switch (data.Grade)
			{
			case EGoodsGradeType.Normal:
				return (TConst.ArtifactPrefixMinNormal.IntValue, TConst.ArtifactPrefixMaxNormal.IntValue);
			case EGoodsGradeType.Rare:
				return (TConst.ArtifactPrefixMinRare.IntValue, TConst.ArtifactPrefixMaxRare.IntValue);
			case EGoodsGradeType.Elite:
				return (TConst.ArtifactPrefixMinElite.IntValue, TConst.ArtifactPrefixMaxElite.IntValue);
			case EGoodsGradeType.Epic:
				return (TConst.ArtifactPrefixMinEpic.IntValue, TConst.ArtifactPrefixMaxEpic.IntValue);
			case EGoodsGradeType.Legend:
				return (TConst.ArtifactPrefixMinLegend.IntValue, TConst.ArtifactPrefixMaxLegend.IntValue);
			case EGoodsGradeType.Unique:
				return (TConst.ArtifactPrefixMinUnique.IntValue, TConst.ArtifactPrefixMaxUnique.IntValue);
			default:
				return (0, 0);
			}
		}

		public static int GetRandomPrefixCount(this TEquipment data)
		{
			(int, int) range = data.GetPrefixCountRange();
			if (range.Item2 < 1)
			{
				return 0;
			}
			int count = RandomUtility.GetRandomInt(range.Item1, range.Item2 + 1);
			int specifiedCount = data.SpecifiedArtifactPrefixIds == null ? 0 : data.SpecifiedArtifactPrefixIds.Length;
			return Math.Max(count - specifiedCount, 0);
		}

		public static int GetMaxLegitPrefixCount(this TEquipment data)
		{
			if (data.GoodsType != EGoodsType.Artifact)
			{
				return 0;
			}
			int specifiedCount = data.SpecifiedArtifactPrefixIds == null ? 0 : data.SpecifiedArtifactPrefixIds.Length;
			return data.GetPrefixCountRange().Item2 + specifiedCount;
		}

		public static int GetMaxDurability(this TEquipment data, float minRatio = 1f)
		{
			if (data.GoodsType == EGoodsType.Artifact)
			{
				return 0;
			}
			if (minRatio < 1f)
			{
				float ratio = RandomUtility.GetRandomFloat(minRatio, 1f);
				return (int)Math.Floor((float)data.MaxDurability * ratio);
			}
			return data.MaxDurability;
		}

		public static Dictionary<EStatType, float> GetBaseStats(this TExtraUnit extraUnit)
		{
			Dictionary<EStatType, float> stats = new Dictionary<EStatType, float>();
			stats.Add(EStatType.Atk, extraUnit.Atk);
			stats.Add(EStatType.Def, extraUnit.Def);
			stats.Add(EStatType.MoveSpeed, extraUnit.MoveSpeed);
			stats.Add(EStatType.AtkSpeed, extraUnit.AtkSpeed);
			stats.Add(EStatType.HpMax, extraUnit.Hp);
			stats.Add(EStatType.HpRegen, extraUnit.HpRegen);
			stats.Add(EStatType.FireResistancePercent, extraUnit.FireResistancePercent);
			stats.Add(EStatType.WaterResistancePercent, extraUnit.WaterResistancePercent);
			stats.Add(EStatType.WindResistancePercent, extraUnit.LightningResistancePercent);
			stats.Add(EStatType.CriticalChancePercent, TConst.DefaultCriticalChancePercent.FloatValue);
			stats.Add(EStatType.CriticalDamagePercent, TConst.DefaultCriticalDamagePercent.FloatValue);
			stats.Add(EStatType.WeaknessDamagePercent, TConst.DefaultWeaknessDamagePercent.FloatValue);
			return stats;
		}

		public static bool IsRecoveryItem(this TItem item)
		{
			return item.ConsumableType == EConsumableType.HpRecovery || item.ConsumableType == EConsumableType.StressRecovery || item.ConsumableType == EConsumableType.BrightnessRecovery || item.ConsumableType == EConsumableType.StaminaRecovery;
		}

		public static string Format(this TLocalizedText data, params object[] args)
		{
			return string.Format(LocalizationUtility.GetLocalizedText(data.Id), args);
		}

		public static string FormatWithExpression(this TLocalizedText data)
		{
			string text = data.LocalText;
			if (!string.IsNullOrEmpty(text))
			{
				text = ExpressionRegex.Replace(text, match =>
				{
					string kind = match.Groups[1].Value;
					string value = match.Groups[2].Value;
					if (kind.Equals("E") && int.TryParse(value, out int expressionId))
					{
						return TExpression.Table[expressionId].Text;
					}
					return LocalizationUtility.GetLocalizedText(kind + "_" + value);
				});
			}
			return text;
		}

		public static Dictionary<EStatType, float> GetBaseStats(this TMonster monster, EDifficulty difficulty, int variantDifficulty)
		{
			TDifficulty difficultyData = TDifficulty.Table[difficulty];
			float cooldownReduction = difficultyData.CooldownReductionPercent;
			float atk = (float)monster.Atk * difficultyData.MonsterAtkPercent / 100f;
			float def = (float)monster.Def * difficultyData.MonsterDefPercent / 100f;
			float hp = (float)monster.Hp * difficultyData.MonsterHpPercent / 100f;
			if (variantDifficulty > 0)
			{
				TVariantDifficulty variantData = TVariantDifficulty.Table[variantDifficulty];
				cooldownReduction += variantData.CooldownReductionPercent;
				atk *= variantData.MonsterAtkPercent / 100f;
				def *= variantData.MonsterDefPercent / 100f;
				hp *= variantData.MonsterHpPercent / 100f;
			}
			Dictionary<EStatType, float> stats = new Dictionary<EStatType, float>();
			stats.Add(EStatType.Atk, atk);
			stats.Add(EStatType.Def, def);
			stats.Add(EStatType.MoveSpeed, monster.MoveSpeed);
			stats.Add(EStatType.AtkSpeed, monster.AtkSpeed);
			stats.Add(EStatType.HpMax, hp);
			stats.Add(EStatType.HpRegen, monster.HpRegen);
			stats.Add(EStatType.FireResistancePercent, monster.FireResistancePercent);
			stats.Add(EStatType.WaterResistancePercent, monster.WaterResistancePercent);
			stats.Add(EStatType.WindResistancePercent, monster.WindResistancePercent);
			stats.Add(EStatType.CooldownReductionPercent, cooldownReduction);
			stats.Add(EStatType.CriticalChancePercent, 0f);
			stats.Add(EStatType.CriticalDamagePercent, TConst.DefaultCriticalDamagePercent.FloatValue);
			stats.Add(EStatType.WeaknessDamagePercent, TConst.DefaultWeaknessDamagePercent.FloatValue);
			return stats;
		}

		public static TMonsterSpawn Pick(this TMonsterSpawn[] monsterSpawns, bool isLightOn)
		{
			List<TMonsterSpawn> candidates = monsterSpawns.Where(_ => _.LightCondition == ELightCondition.None || (isLightOn && _.LightCondition == ELightCondition.On) || (_.LightCondition == ELightCondition.Off && !isLightOn)).ToList();
			if (candidates.Any())
			{
				if (candidates.Count == 1)
				{
					return candidates.First();
				}
				int roll = RandomUtility.GetRandomInt(0, candidates.Sum(_ => _.Probability));
				foreach (TMonsterSpawn candidate in candidates)
				{
					roll -= candidate.Probability;
					if (roll < 1)
					{
						return candidate;
					}
				}
			}
			return null;
		}

		public static int PickRandomParadoxId(this ParadoxTable table, int previousLevel, int[] excludeIds, bool isPositive)
		{
			TParadoxLevel levelData = TParadoxLevel.SubTable[previousLevel, isPositive];
			int nextLevel = levelData.GetNextRandomLevel();
			if (nextLevel < 1)
			{
				return 0;
			}
			List<TParadox> candidates = TParadox.GroupTable[nextLevel]
				.Where(_ => excludeIds == null || !excludeIds.Contains(_.Id))
				.Where(_ => _.IsPositive == isPositive)
				.ToList();
			if (!candidates.Any())
			{
				return 0;
			}
			return candidates[RandomUtility.GetRandomInt(candidates.Count)].Id;
		}

		public static string GetParadoxLevelText(this TParadox data)
		{
			string levelText = data.Level < TConst.ParadoxUniqueLevel.IntValue ? string.Format("{0}", data.Level) : TLocalizedText.ParadoxUnique.LocalText;
			return string.Format(TLocalizedText.ParadoxLevelFormat.LocalText, levelText);
		}

		public static GoodsData GetRemovalGoodsData(this TParadox data)
		{
			return TParadoxLevel.SubTable[data.Level, data.IsPositive].RemovalGoodsData;
		}

		public static GoodsData GetLockGoodsData(this TParadox data)
		{
			return TParadoxLevel.SubTable[data.Level, data.IsPositive].LockGoodsData;
		}

		public static GoodsData GetUnlockGoodsData(this TParadox data)
		{
			return TParadoxLevel.SubTable[data.Level, data.IsPositive].UnLockGoodsData;
		}

		public static string GetParadoxNameWithLevel(this TParadox data)
		{
			return data.Name + " " + data.GetParadoxLevelText();
		}

		public static int GetMaxLevel(this ParadoxLevelTable table)
		{
			return TParadox.GroupTable.Keys.Max();
		}

		public static int GetNextRandomLevel(this TParadoxLevel data)
		{
			if (data.NextLevelProbability != null && data.NextLevelProbability.Any())
			{
				int roll = RandomUtility.GetRandomInt(0, data.NextLevelProbability.Sum());
				for (int i = 0; i < data.NextLevelProbability.Length; i++)
				{
					roll -= data.NextLevelProbability[i];
					if (roll < 1)
					{
						return i + 1;
					}
				}
			}
			return 0;
		}

		public static float GetBaseAtkStat(this TRewardBox rewardBox, EDifficulty difficulty, int variantDifficulty)
		{
			float atk = rewardBox.HitBoxAtk;
			atk *= TDifficulty.Table[difficulty].TrapAtkPercent / 100f;
			if (variantDifficulty < 1)
			{
				return atk;
			}
			return atk * (TVariantDifficulty.Table[variantDifficulty].TrapAtkPercent / 100f);
		}

		public static TRewardBox Pick(this TRewardBox[] rewardBox)
		{
			List<TRewardBox> candidates = rewardBox.ToList();
			if (candidates.Any())
			{
				if (candidates.Count == 1)
				{
					return candidates.First();
				}
				int roll = RandomUtility.GetRandomInt(0, candidates.Sum(_ => _.Weight));
				foreach (TRewardBox candidate in candidates)
				{
					roll -= candidate.Weight;
					if (roll < 1)
					{
						return candidate;
					}
				}
			}
			return null;
		}

		public static EventCondition[] GetUnlockConditions(this TTalent data)
		{
			List<EventCondition> conditions = data.UnlockConditions != null ? data.UnlockConditions.ToList() : new List<EventCondition>();
			if (data.RootTalentIds != null)
			{
				data.RootTalentIds.ForEach(id => conditions.Add(new EventCondition(EEventConditionType.Talent, id)));
			}
			return conditions.ToArray();
		}

		public static float GetBaseAtkStat(this TTrap trap, EDifficulty difficulty, int variantDifficulty)
		{
			float atk = trap.HitBoxAtk;
			atk *= TDifficulty.Table[difficulty].TrapAtkPercent / 100f;
			if (variantDifficulty < 1)
			{
				return atk;
			}
			return atk * (TVariantDifficulty.Table[variantDifficulty].TrapAtkPercent / 100f);
		}
	}
}
