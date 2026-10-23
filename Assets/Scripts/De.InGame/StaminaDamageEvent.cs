using System.Collections.Generic;
using GameCommon.Data;

public class StaminaDamageEvent : IUnitEvent
{
	public float Amount;

	public float GetTriggerValue => (int)Amount;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = new List<ITarget> { unitModel };
		return false;
	}
}
