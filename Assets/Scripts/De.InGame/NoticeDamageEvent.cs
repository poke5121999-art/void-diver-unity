using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;

public class NoticeDamageEvent : IUnitEvent
{
	public UnitModel SourceModel;

	public UnitModel TargetModel;

	public int SkillId;

	public int Damage;

	public bool IsCritical;

	public bool IsBackAttack;

	public bool IsFirstHit;

	public EHpDamageReasonType ReasonType;

	public float GetTriggerValue => Damage;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = null;
		if (trigger is DamageProvideSkillTrigger damageProvideSkillTrigger)
		{
			if (SourceModel == null)
			{
				return false;
			}
			if (unitModel.NetworkObjectId == SourceModel.NetworkObjectId)
			{
				triggerTargets = new List<ITarget> { TargetModel };
				if ((damageProvideSkillTrigger.ConditionSkillIds == null || damageProvideSkillTrigger.ConditionSkillIds.Length == 0 || Contains(damageProvideSkillTrigger.ConditionSkillIds, SkillId)) && (!damageProvideSkillTrigger.ConditionIsOnlyCritical || IsCritical) && (!damageProvideSkillTrigger.ConditionIsOnlyFirstHit || IsFirstHit) && (damageProvideSkillTrigger.ConditionReasonTypes == EHpDamageReasonType.None || damageProvideSkillTrigger.ConditionReasonTypes == ReasonType))
				{
					return true;
				}
			}
		}
		return false;
	}

	private static bool Contains(int[] array, int value)
	{
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == value)
			{
				return true;
			}
		}
		return false;
	}
}
