using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;

public class LightFuelRecoveryEvent : IUnitEvent
{
	public int Amount;

	public int SkillId;

	public float GetTriggerValue => Amount;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = new List<ITarget> { unitModel };
		if (trigger is RecoverySkillTrigger recoverySkillTrigger)
		{
			return System.Linq.Enumerable.Contains(recoverySkillTrigger.ConditionHitEffectType, EHitEffectType.BrightnessRecovery);
		}
		return false;
	}
}
