using GameCommon.Data;
using GameCommon.Enum;
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
		return default;
	}

	public static float CalculateStatOnAttack(EStatType statType, DamageInfo damageInfo, IActionStats actionStats, UnitModel targetUnit, UnitModel buffSource = null, UnitIdentity sourceIdentity = default(UnitIdentity))
	{
		return 0f;
	}

	private static float CalculateStatOnAttacked(EStatType statType, DamageInfo damageInfo, UnitModel sourceUnit, UnitModel targetUnit, UnitIdentity sourceIdentity = default(UnitIdentity))
	{
		return 0f;
	}

	public static EElementalType GetResolvedElementalType(DamageInfo damageInfo, UnitModel sourceUnit, UnitModel targetUnit)
	{
		return EElementalType.None;
	}

	private static bool CheckConditionalStats(ConditionalStat conditionalStat, UnitModel sourceUnit, UnitIdentity sourceIdentity, UnitModel targetUnit, EStatType statType, DamageInfo damageInfo)
	{
		return false;
	}

	private static bool CheckConditionGroup(ConditionalStatConditionGroup group, UnitModel unitModel, UnitIdentity identity, EElementalType resolvedElemental)
	{
		return false;
	}

	private static bool CheckSingleCondition(ConditionalStatCondition condition, UnitModel unitModel, UnitIdentity identity, EElementalType resolvedElemental)
	{
		return false;
	}

	private static float GetConditionalOnlyOnAttack(EStatType statType, DamageInfo damageInfo, IActionStats actionStats, UnitModel targetUnit, UnitModel sourceUnit, UnitIdentity sourceIdentity = default(UnitIdentity))
	{
		return 0f;
	}

	private static float GetConditionalOnlyOnAttacked(EStatType statType, DamageInfo damageInfo, UnitModel sourceUnit, UnitModel targetUnit, UnitIdentity sourceIdentity = default(UnitIdentity))
	{
		return 0f;
	}

	private static EStatType GetPercentCounterpart(EStatType statType)
	{
		return EStatType.None;
	}

	public static EElementalType ResolveAttackElement(DamageInfo damageInfo, IActionStats actionStats, UnitModel targetUnit)
	{
		return EElementalType.None;
	}

	public static EElementalDamagedType GetElementalDamagedType(DamageInfo damageInfo, IActionStats actionStats, UnitModel targetUnit)
	{
		return EElementalDamagedType.None;
	}

	public static float GetAdditionalDamageTakenRateByElemental(UnitModel targetUnit, EElementalType elementalType, DamageInfo damageInfo = null, UnitModel sourceUnit = null, UnitIdentity sourceIdentity = default(UnitIdentity))
	{
		return 0f;
	}

	public static float GetAdditionalDamageDealtRateByElemental(IActionStats actionStats, EElementalType elementalType, DamageInfo damageInfo = null, UnitModel targetUnit = null, UnitModel sourceUnit = null, UnitIdentity sourceIdentity = default(UnitIdentity))
	{
		return 0f;
	}

	public static int GetCharacterTalentTierUnlockedLevel(int tier)
	{
		return 0;
	}

	public static void LookAtForward(EMeshType meshType, Vector3 forward, GameObject frontGo, GameObject backGo)
	{
	}

	public static Vector3 GetRandomDirection()
	{
		return default;
	}

	public static ESkillTagType[] GetCombinedSkillTags(int skillId, int hitBoxId)
	{
		return null;
	}
}
