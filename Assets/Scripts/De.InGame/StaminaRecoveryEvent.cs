using GameCommon.Enum;
using System.Linq;
using System.Collections.Generic;
using GameCommon.Data;

public class StaminaRecoveryEvent : IUnitEvent
{
	public float Amount;

	public float GetTriggerValue => (int)Amount;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = new List<ITarget> { unitModel };
		if (trigger is RecoverySkillTrigger recoverySkillTrigger)
		{
			return recoverySkillTrigger.ConditionHitEffectType.Contains(EHitEffectType.StaminaRecovery);
		}
		return false;
	}
}
