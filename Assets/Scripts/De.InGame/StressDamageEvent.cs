using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;
using UnityEngine;

public class StressDamageEvent : IUnitEvent
{
	public UnitModel SourceModel;

	public float Amount;

	public bool IsFixed;

	public bool ShowDamageEffect = true;

	public Vector3 HitPosition;

	public EStressReasonType ReasonType;

	public int SkillId;

	public float GetTriggerValue => Amount;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = new List<ITarget> { unitModel };
		if (trigger is StressDamageSkillTrigger stressDamageSkillTrigger)
		{
			triggerTargets = new List<ITarget> { SourceModel };
			if (stressDamageSkillTrigger.ConditionStressReasonType == EStressReasonType.None)
			{
				return true;
			}
			return stressDamageSkillTrigger.ConditionStressReasonType == ReasonType;
		}
		return false;
	}
}
