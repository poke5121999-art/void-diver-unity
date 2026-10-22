using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;
using UnityEngine;

public class HpDamageEvent : IUnitEvent
{
	public UnitModel SourceModel;

	public UnitIdentity SourceIdentity;

	public int Amount;

	public int SkillId;

	public int HitBoxId;

	public bool IsCritical;

	public bool IsBackAttack;

	public EElementalDamagedType ElementalDamagedType;

	public bool IsFirstHit;

	public bool IsFixedDamage;

	public bool IgnoreShield;

	public Vector3 HitPosition;

	public EElementalType ElementalType;

	public EHpDamageReasonType ReasonType;

	public int Damage;

	public float GetTriggerValue => Damage;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = new List<ITarget> { unitModel };
		if (trigger is DamageSkillTrigger damageSkillTrigger)
		{
			triggerTargets = new List<ITarget> { SourceModel };
			if ((!damageSkillTrigger.ConditionIsOnlyCritical || IsCritical) && (!damageSkillTrigger.ConditionIsIgnoreBackAttack || !IsBackAttack))
			{
				return true;
			}
		}
		return false;
	}
}
