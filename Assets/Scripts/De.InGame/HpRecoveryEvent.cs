using System.Collections.Generic;
using System.Linq;
using GameCommon.Enum;
using GameCommon.Data;

public class HpRecoveryEvent : IUnitEvent
{
	public int Amount;

	public int SkillId;

	public EHpRecoveryReasonType ReasonType;

	public UnitModel SourceModel;

	public float GetTriggerValue => Amount;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = new List<ITarget> { unitModel };
		if (trigger is RecoverySkillTrigger recoverySkillTrigger)
		{
			return recoverySkillTrigger.ConditionHitEffectType.Contains(EHitEffectType.HpRecovery);
		}
		return false;
	}
}
