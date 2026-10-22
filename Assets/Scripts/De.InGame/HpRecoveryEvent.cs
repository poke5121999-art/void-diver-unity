using System.Collections.Generic;
using GameCommon.Data;

public class HpRecoveryEvent : IUnitEvent
{
	public int Amount;

	public int SkillId;

	public EHpRecoveryReasonType ReasonType;

	public UnitModel SourceModel;

	public float GetTriggerValue => 0f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = null;
		return false;
	}
}
