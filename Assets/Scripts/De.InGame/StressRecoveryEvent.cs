using System.Collections.Generic;
using GameCommon.Data;

public class StressRecoveryEvent : IUnitEvent
{
	public float Amount;

	public int SkillId;

	public bool IsFixed;

	public float GetTriggerValue => 0f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = null;
		return false;
	}
}
