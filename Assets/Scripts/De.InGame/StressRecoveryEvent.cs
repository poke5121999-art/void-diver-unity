using GameCommon.Enum;
using System.Linq;
using System.Collections.Generic;
using GameCommon.Data;

public class StressRecoveryEvent : IUnitEvent
{
	public float Amount;

	public int SkillId;

	public bool IsFixed;

	public float GetTriggerValue => Amount;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = new List<ITarget> { unitModel };
		if (trigger is RecoverySkillTrigger recoverySkillTrigger)
		{
			return recoverySkillTrigger.ConditionHitEffectType.Contains(EHitEffectType.BrightnessRecovery);
		}
		return false;
	}
}
