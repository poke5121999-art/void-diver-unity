using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using SomeNet.Utility;
using UnityEngine;

public static class InGameUtility
{
	public struct DamageResult
	{
		public int Damage;

		public bool IsCritical;
	}

	public static DamageResult CalculateDamage(DamageInfo damageInfo, IActionStats actionStats, UnitModel targetUnit, bool isBackAttack, int level, UnitModel buffSource = null, bool allowCritical = true, UnitIdentity sourceIdentity = default(UnitIdentity))
	{
		UnitModel sourceUnit = buffSource ?? (actionStats as UnitModel);
		if (!sourceIdentity.IsValid)
		{
			sourceIdentity = UnitIdentity.From(sourceUnit);
		}
		float sourceStat = 0f;
		float sourcePenetration = 0f;
		float sourcePenetrationPercent = 0f;
		float sourceWeaknessDamagePercent = 0f;
		float sourceCriticalDamagePercent = 0f;
		float sourceAttackDamagePercent = 0f;
		float targetDef = 0f;
		float targetReductionDamagePercent = 0f;
		float additionalDamageRate = 0f;
		bool isCritical = false;
		int power = 0;
		sourceStat = CalculateSourceStat(damageInfo.CoefficientStat);
		float conditionalPercent = GetConditionalOnlyOnAttack(GetPercentCounterpart(damageInfo.CoefficientStat), damageInfo, actionStats, targetUnit, sourceUnit, sourceIdentity);
		if (conditionalPercent != 0f)
		{
			sourceStat *= conditionalPercent / 100f + 1f;
		}
		sourcePenetration = CalculateSourceStat(EStatType.Penetration);
		sourcePenetrationPercent = CalculateSourceStat(EStatType.PenetrationPercent);
		sourceWeaknessDamagePercent = CalculateSourceStat(EStatType.WeaknessDamagePercent);
		sourceCriticalDamagePercent = CalculateSourceStat(EStatType.CriticalDamagePercent);
		sourceAttackDamagePercent = CalculateSourceStat(EStatType.AdditionalAttackDamagePercent);
		(targetDef, targetReductionDamagePercent) = GatherTargetStats();
		(isCritical, additionalDamageRate) = ResolveCriticalAndBackAttack();
		power = CalculatePower();
		float damage = AssembleDamage();
		if (damage <= 1f)
		{
			damage = 1f;
		}
		DamageResult result = default(DamageResult);
		result.Damage = (int)damage;
		result.IsCritical = isCritical;
		return result;

		(float, float) GatherTargetStats()
		{
			float num = CalculateStatOnAttacked(EStatType.Def, damageInfo, sourceUnit, targetUnit, sourceIdentity);
			float conditionalOnlyOnAttacked = GetConditionalOnlyOnAttacked(EStatType.DefPercent, damageInfo, sourceUnit, targetUnit, sourceIdentity);
			if (conditionalOnlyOnAttacked != 0f)
			{
				num *= conditionalOnlyOnAttacked / 100f + 1f;
			}
			return (num, CalculateStatOnAttacked(EStatType.ReductionDamagePercent, damageInfo, sourceUnit, targetUnit, sourceIdentity));
		}
		(bool, float) ResolveCriticalAndBackAttack()
		{
			float num2 = CalculateSourceStat(EStatType.CriticalChancePercent) + CalculateTargetStat(EStatType.CriticalTakenChancePercent);
			float num3 = 0f;
			if (isBackAttack)
			{
				(float, float) tuple = CalculateBackAttack();
				num2 += tuple.Item1;
				num3 += tuple.Item2;
			}
			bool item = allowCritical && RandomUtility.GetRandomBoolFromPercentage((num2 <= 0f) ? 0f : num2);
			return (item, num3);
		}
		(float, float) CalculateBackAttack()
		{
			if (sourceUnit == null)
			{
				return (0f, TConst.EtcBackAttackDamagePercent.FloatValue);
			}
			float num4 = ((sourceUnit is CharacterModel) ? TConst.CharacterBackAttackIncreaseCriticalChancePercent.FloatValue : ((sourceUnit is MonsterModel) ? TConst.MonsterBackAttackIncreaseCriticalChancePercent.FloatValue : ((!(sourceUnit is ExtraUnitModel)) ? 0f : TConst.ExtraUnitBackAttackIncreaseCriticalChancePercent.FloatValue)));
			float num5 = CalculateSourceStat(EStatType.BackAttackDamagePercent);
			IList<BuffEffectInfo<BackAttackCriticalChanceAddEffect>> buffEffects = sourceUnit.GetBuffEffects<BackAttackCriticalChanceAddEffect>();
			if (buffEffects != null && buffEffects.Count > 0)
			{
				num4 += buffEffects.Sum((BuffEffectInfo<BackAttackCriticalChanceAddEffect> e) => (float)e.Stack * e.BuffEffect.Percent);
			}
			IList<BuffEffectInfo<BackAttackDamageAddEffect>> buffEffects2 = sourceUnit.GetBuffEffects<BackAttackDamageAddEffect>();
			if (buffEffects2 != null && buffEffects2.Count > 0)
			{
				num5 += buffEffects2.Sum((BuffEffectInfo<BackAttackDamageAddEffect> e) => (float)e.Stack * e.BuffEffect.Percent);
			}
			return (num4, num5);
		}
		int CalculatePower()
		{
			return (int)(actionStats.WeaponAtkMultiplier * (((float)level * damageInfo.StatFactorGrowth + damageInfo.StatFactor) / 100f) * sourceStat + (float)(damageInfo.PowerGrowth * level + damageInfo.Power));
		}
		float AssembleDamage()
		{
			float num6 = targetDef - sourcePenetration;
			if (num6 <= 0f)
			{
				num6 = 0f;
			}
			num6 = num6 * (100f - sourcePenetrationPercent) / 100f;
			float num7 = (float)power - (float)power * num6 / (num6 + TConst.DefenseFactor.FloatValue);
			EElementalType attackElement = ResolveAttackElement(damageInfo, actionStats, targetUnit);
			if (attackElement != EElementalType.None)
			{
				float additionalDamageDealtRate = GetAdditionalDamageDealtRateByElemental(actionStats, attackElement, damageInfo, targetUnit, sourceUnit, sourceIdentity);
				float additionalDamageTakenRate = GetAdditionalDamageTakenRateByElemental(targetUnit, attackElement, damageInfo, sourceUnit, sourceIdentity);
				float num8 = ((additionalDamageTakenRate > 0f) ? (sourceWeaknessDamagePercent / 100f) : 0f);
				num7 = num7 * (additionalDamageDealtRate + 1f) * (additionalDamageTakenRate + 1f + num8);
			}
			float num9 = sourceAttackDamagePercent - targetReductionDamagePercent;
			if (num9 != 0f)
			{
				if (num9 <= -100f)
				{
					num9 = -100f;
				}
				num7 = (num9 + 100f) * num7 / 100f;
			}
			if (isCritical)
			{
				num7 *= sourceCriticalDamagePercent / 100f + 1f;
			}
			return RandomUtility.GetRandomFloat(0.95f, 1.05f) * ((additionalDamageRate + 100f) / 100f) * num7;
		}
		float CalculateSourceStat(EStatType stat)
		{
			return CalculateStatOnAttack(stat, damageInfo, actionStats, targetUnit, sourceUnit, sourceIdentity);
		}
		float CalculateTargetStat(EStatType stat)
		{
			return CalculateStatOnAttacked(stat, damageInfo, sourceUnit, targetUnit, sourceIdentity);
		}
	}

	public static float CalculateStatOnAttack(EStatType statType, DamageInfo damageInfo, IActionStats actionStats, UnitModel targetUnit, UnitModel buffSource = null, UnitIdentity sourceIdentity = default(UnitIdentity))
	{
		float num = actionStats.GetStat(statType);
		if (buffSource == null)
		{
			buffSource = actionStats as UnitModel;
		}
		if (!sourceIdentity.IsValid)
		{
			sourceIdentity = UnitIdentity.From(buffSource);
		}
		IList<BuffEffectInfo<AttackConditionalStatsEffect>> buffEffects = buffSource.GetBuffEffects<AttackConditionalStatsEffect>();
		if (buffEffects != null && buffEffects.Count > 0)
		{
			num += buffEffects.Where((BuffEffectInfo<AttackConditionalStatsEffect> b) => b.BuffEffect != null).SelectMany((BuffEffectInfo<AttackConditionalStatsEffect> effect) => effect.BuffEffect.ConditionalStats.Where((ConditionalStat cs) => CheckConditionalStats(cs, buffSource, sourceIdentity, targetUnit, statType, damageInfo)).Select((ConditionalStat cs) => (float)effect.Stack * cs.StatValue)).Sum();
		}
		if (damageInfo != null && damageInfo.ConditionalStats != null && damageInfo.ConditionalStats.Length != 0)
		{
			num += damageInfo.ConditionalStats.Where((ConditionalStat cs) => CheckConditionalStats(cs, buffSource, sourceIdentity, targetUnit, statType, damageInfo)).Sum((ConditionalStat cs) => cs.StatValue);
		}
		return num;
	}

	private static float CalculateStatOnAttacked(EStatType statType, DamageInfo damageInfo, UnitModel sourceUnit, UnitModel targetUnit, UnitIdentity sourceIdentity = default(UnitIdentity))
	{
		float stat = targetUnit.GetStat(statType);
		if (!sourceIdentity.IsValid)
		{
			sourceIdentity = UnitIdentity.From(sourceUnit);
		}
		IList<BuffEffectInfo<AttackedConditionalStatsEffect>> buffEffects = targetUnit.GetBuffEffects<AttackedConditionalStatsEffect>();
		if (buffEffects == null || buffEffects.Count == 0)
		{
			return stat;
		}
		return stat + buffEffects.SelectMany((BuffEffectInfo<AttackedConditionalStatsEffect> effect) => effect.BuffEffect.ConditionalStats.Where((ConditionalStat cs) => CheckConditionalStats(cs, sourceUnit, sourceIdentity, targetUnit, statType, damageInfo)).Select((ConditionalStat cs) => (float)effect.Stack * cs.StatValue)).Sum();
	}

	public static EElementalType GetResolvedElementalType(DamageInfo damageInfo, UnitModel sourceUnit, UnitModel targetUnit)
	{
		if (damageInfo == null)
		{
			return EElementalType.None;
		}
		EElementalType[] array = new EElementalType[2]
		{
			damageInfo.ElementalType,
			(sourceUnit != null) ? sourceUnit.ElementalType : EElementalType.None
		}.Where((EElementalType e) => e != EElementalType.None).ToArray();
		if (array.Length == 0)
		{
			return EElementalType.None;
		}
		return array.MaxBy(delegate(EElementalType e)
		{
			EStatType statType;
			switch (e)
			{
			case EElementalType.Fire:
				statType = EStatType.FireResistancePercent;
				break;
			case EElementalType.Water:
				statType = EStatType.WaterResistancePercent;
				break;
			case EElementalType.Wind:
				statType = EStatType.WindResistancePercent;
				break;
			default:
				return 0f;
			}
			return (0f - targetUnit.GetStat(statType)) / 100f;
		});
	}

	private static bool CheckConditionalStats(ConditionalStat conditionalStat, UnitModel sourceUnit, UnitIdentity sourceIdentity, UnitModel targetUnit, EStatType statType, DamageInfo damageInfo)
	{
		if (conditionalStat.StatType == statType && RandomUtility.GetRandomBoolFromPercentage(conditionalStat.Probability))
		{
			EElementalType resolvedElemental = GetResolvedElementalType(damageInfo, sourceUnit, targetUnit);
			if (CheckConditionGroup(conditionalStat.SourceCondition, sourceUnit, sourceIdentity, resolvedElemental) && CheckConditionGroup(conditionalStat.TargetCondition, targetUnit, UnitIdentity.From(targetUnit), resolvedElemental))
			{
				return true;
			}
		}
		return false;
	}

	private static bool CheckConditionGroup(ConditionalStatConditionGroup group, UnitModel unitModel, UnitIdentity identity, EElementalType resolvedElemental)
	{
		if (group == null || group.Conditions == null || group.Conditions.Length == 0)
		{
			return true;
		}
		if (unitModel == null && !identity.IsValid)
		{
			return false;
		}
		if (!group.IsAny)
		{
			return group.Conditions.All((ConditionalStatCondition c) => CheckSingleCondition(c, unitModel, identity, resolvedElemental));
		}
		return group.Conditions.Any((ConditionalStatCondition c) => CheckSingleCondition(c, unitModel, identity, resolvedElemental));
	}

	private static bool CheckSingleCondition(ConditionalStatCondition condition, UnitModel unitModel, UnitIdentity identity, EElementalType resolvedElemental)
	{
		TMonster value = null;
		if (identity.UnitType == EUnitType.Monster)
		{
			TMonster.Table.TryGetValue(identity.TableId, out value);
		}
		if (condition.ElementalCondition != EElementalType.None && resolvedElemental != condition.ElementalCondition)
		{
			return false;
		}
		if (condition.FactionCondition != EMonsterCategory.None)
		{
			EMonsterCategory[] source = ((identity.UnitType != EUnitType.Monster) ? (unitModel?.MonsterCategories ?? Array.Empty<EMonsterCategory>()) : (value?.MonsterCategories ?? Array.Empty<EMonsterCategory>()));
			if (!source.Contains(condition.FactionCondition))
			{
				return false;
			}
		}
		if (condition.CcCondition != ECrowdControlType.None && (unitModel == null || !unitModel.IsCrowdControlled(condition.CcCondition)))
		{
			return false;
		}
		if (condition.EffectTag != EStatusEffectTag.None)
		{
			if (unitModel == null)
			{
				return false;
			}
			if (!unitModel.HasStatusEffectTag(condition.EffectTag))
			{
				return false;
			}
		}
		if (condition.MonsterCondition == EMonsterType.None || (value != null && value.MonsterType == condition.MonsterCondition))
		{
			return true;
		}
		return false;
	}

	private static float GetConditionalOnlyOnAttack(EStatType statType, DamageInfo damageInfo, IActionStats actionStats, UnitModel targetUnit, UnitModel sourceUnit, UnitIdentity sourceIdentity = default(UnitIdentity))
	{
		float num = 0f;
		if (!sourceIdentity.IsValid)
		{
			sourceIdentity = UnitIdentity.From(sourceUnit);
		}
		IList<BuffEffectInfo<AttackConditionalStatsEffect>> buffEffects = sourceUnit.GetBuffEffects<AttackConditionalStatsEffect>();
		if (buffEffects != null && buffEffects.Count > 0)
		{
			num += buffEffects.Where((BuffEffectInfo<AttackConditionalStatsEffect> b) => b.BuffEffect != null).SelectMany((BuffEffectInfo<AttackConditionalStatsEffect> effect) => effect.BuffEffect.ConditionalStats.Where((ConditionalStat cs) => CheckConditionalStats(cs, sourceUnit, sourceIdentity, targetUnit, statType, damageInfo)).Select((ConditionalStat cs) => (float)effect.Stack * cs.StatValue)).Sum();
		}
		if (damageInfo != null && damageInfo.ConditionalStats != null && damageInfo.ConditionalStats.Length != 0)
		{
			num += damageInfo.ConditionalStats.Where((ConditionalStat cs) => CheckConditionalStats(cs, sourceUnit, sourceIdentity, targetUnit, statType, damageInfo)).Sum((ConditionalStat cs) => cs.StatValue);
		}
		return num;
	}

	private static float GetConditionalOnlyOnAttacked(EStatType statType, DamageInfo damageInfo, UnitModel sourceUnit, UnitModel targetUnit, UnitIdentity sourceIdentity = default(UnitIdentity))
	{
		IList<BuffEffectInfo<AttackedConditionalStatsEffect>> buffEffects = targetUnit.GetBuffEffects<AttackedConditionalStatsEffect>();
		if (buffEffects == null || buffEffects.Count == 0)
		{
			return 0f;
		}
		if (!sourceIdentity.IsValid)
		{
			sourceIdentity = UnitIdentity.From(sourceUnit);
		}
		return buffEffects.SelectMany((BuffEffectInfo<AttackedConditionalStatsEffect> effect) => effect.BuffEffect.ConditionalStats.Where((ConditionalStat cs) => CheckConditionalStats(cs, sourceUnit, sourceIdentity, targetUnit, statType, damageInfo)).Select((ConditionalStat cs) => (float)effect.Stack * cs.StatValue)).Sum();
	}

	private static EStatType GetPercentCounterpart(EStatType statType)
	{
		return statType switch
		{
			EStatType.Atk => EStatType.AtkPercent,
			EStatType.Def => EStatType.DefPercent,
			EStatType.MoveSpeed => EStatType.MoveSpeedPercent,
			EStatType.AtkSpeed => EStatType.AtkSpeedPercent,
			EStatType.HpMax => EStatType.HpMaxPercent,
			EStatType.HpRegen => EStatType.HpRegenPercent,
			_ => EStatType.None,
		};
	}

	public static EElementalType ResolveAttackElement(DamageInfo damageInfo, IActionStats actionStats, UnitModel targetUnit)
	{
		EElementalType elementalType = damageInfo.ElementalType;
		EElementalType elementalType2 = actionStats.ElementalType;
		if (elementalType == EElementalType.None)
		{
			return elementalType2;
		}
		if (elementalType2 != EElementalType.None)
		{
			float num = Vulnerability(elementalType);
			float num2 = Vulnerability(elementalType2);
			if (num > num2)
			{
				return elementalType;
			}
			if (num2 > num || elementalType > elementalType2)
			{
				return elementalType2;
			}
		}
		return elementalType;

		float Vulnerability(EElementalType element)
		{
			EStatType statType;
			switch (element)
			{
			case EElementalType.Fire:
				statType = EStatType.FireResistancePercent;
				break;
			case EElementalType.Water:
				statType = EStatType.WaterResistancePercent;
				break;
			case EElementalType.Wind:
				statType = EStatType.WindResistancePercent;
				break;
			default:
				return 0f;
			}
			return (0f - targetUnit.GetStat(statType)) / 100f;
		}
	}

	public static EElementalDamagedType GetElementalDamagedType(DamageInfo damageInfo, IActionStats actionStats, UnitModel targetUnit)
	{
		EElementalType elementalType = ResolveAttackElement(damageInfo, actionStats, targetUnit);
		EStatType statType;
		switch (elementalType)
		{
		case EElementalType.Fire:
			statType = EStatType.FireResistancePercent;
			break;
		case EElementalType.Water:
			statType = EStatType.WaterResistancePercent;
			break;
		case EElementalType.Wind:
			statType = EStatType.WindResistancePercent;
			break;
		default:
			return EElementalDamagedType.None;
		}
		float num = (0f - targetUnit.GetStat(statType)) / 100f;
		if (num > 0f)
		{
			switch (elementalType)
			{
			case EElementalType.Fire:
				return EElementalDamagedType.FirePenalty;
			case EElementalType.Water:
				return EElementalDamagedType.WaterPenalty;
			case EElementalType.Wind:
				return EElementalDamagedType.WindPenalty;
			}
		}
		else if (num < 0f)
		{
			switch (elementalType)
			{
			case EElementalType.Fire:
				return EElementalDamagedType.FireResistance;
			case EElementalType.Water:
				return EElementalDamagedType.WaterResistance;
			case EElementalType.Wind:
				return EElementalDamagedType.WindResistance;
			}
		}
		return EElementalDamagedType.None;
	}

	public static float GetAdditionalDamageTakenRateByElemental(UnitModel targetUnit, EElementalType elementalType, DamageInfo damageInfo = null, UnitModel sourceUnit = null, UnitIdentity sourceIdentity = default(UnitIdentity))
	{
		EStatType statType;
		switch (elementalType)
		{
		case EElementalType.Fire:
			statType = EStatType.FireResistancePercent;
			break;
		case EElementalType.Water:
			statType = EStatType.WaterResistancePercent;
			break;
		case EElementalType.Wind:
			statType = EStatType.WindResistancePercent;
			break;
		default:
			return 0f;
		}
		if (damageInfo != null)
		{
			return (0f - CalculateStatOnAttacked(statType, damageInfo, sourceUnit, targetUnit, sourceIdentity)) / 100f;
		}
		return (0f - targetUnit.GetStat(statType)) / 100f;
	}

	public static float GetAdditionalDamageDealtRateByElemental(IActionStats actionStats, EElementalType elementalType, DamageInfo damageInfo = null, UnitModel targetUnit = null, UnitModel sourceUnit = null, UnitIdentity sourceIdentity = default(UnitIdentity))
	{
		EStatType statType;
		switch (elementalType)
		{
		case EElementalType.Fire:
			statType = EStatType.FireDamagePercent;
			break;
		case EElementalType.Water:
			statType = EStatType.WaterDamagePercent;
			break;
		case EElementalType.Wind:
			statType = EStatType.WindDamagePercent;
			break;
		default:
			return 0f;
		}
		float num = ((damageInfo == null) ? actionStats.GetStat(statType) : CalculateStatOnAttack(statType, damageInfo, actionStats, targetUnit, sourceUnit, sourceIdentity));
		return num / 100f;
	}

	public static int GetCharacterTalentTierUnlockedLevel(int tier)
	{
		if (tier < 1)
		{
			return 1;
		}
		switch (tier)
		{
		case 1:
			return TConst.CharacterTalentTier1UserLevel.IntValue;
		case 2:
			return TConst.CharacterTalentTier2UserLevel.IntValue;
		case 3:
			return TConst.CharacterTalentTier3UserLevel.IntValue;
		case 4:
			return TConst.CharacterTalentTier4UserLevel.IntValue;
		case 5:
			return TConst.CharacterTalentTier5UserLevel.IntValue;
		case 6:
			return TConst.CharacterTalentTier6UserLevel.IntValue;
		default:
			return 1;
		}
	}

	public static void LookAtForward(EMeshType meshType, Vector3 forward, GameObject frontGo, GameObject backGo)
	{
		switch (meshType)
		{
		case EMeshType.D25:
			LookAsForwardAsD25();
			break;
		case EMeshType.D3:
			frontGo.transform.forward = forward;
			break;
		}

		void LookAsForwardAsD25()
		{
			Vector3 vector = MathUtility.SnapToCardinalDirection(forward);
			if (vector == Vector3.forward)
			{
				frontGo.SetActive(value: true);
				backGo.SetActive(value: false);
			}
			if (vector == Vector3.back)
			{
				frontGo.SetActive(value: false);
				backGo.SetActive(value: true);
			}
			if (vector == Vector3.left)
			{
				frontGo.SetActive(value: true);
				backGo.SetActive(value: false);
				frontGo.transform.Flip25D();
			}
			if (vector == Vector3.right)
			{
				frontGo.SetActive(value: false);
				backGo.SetActive(value: true);
				backGo.transform.Flip25D();
			}
		}
	}

	public static Vector3 GetRandomDirection()
	{
		return new Vector3(RandomUtility.GetRandomFloat(-1f, 1f), 0f, RandomUtility.GetRandomFloat(-1f, 1f)).normalized;
	}

	public static ESkillTagType[] GetCombinedSkillTags(int skillId, int hitBoxId)
	{
		ESkillTagType[] array = null;
		ESkillTagType[] array2 = null;
		if (skillId > 0 && TSkill.Table.TryGetValue(skillId, out var value))
		{
			array = value.SkillTags;
		}
		if (hitBoxId > 0 && THitBox.Table.TryGetValue(hitBoxId, out var value2) && value2.HitBoxInfo != null)
		{
			array2 = value2.HitBoxInfo.SkillTags;
		}
		if (array == null || array.Length == 0)
		{
			return array2 ?? Array.Empty<ESkillTagType>();
		}
		if (array2 == null || array2.Length == 0)
		{
			return array;
		}
		return array.Concat(array2).Distinct().ToArray();
	}
}
