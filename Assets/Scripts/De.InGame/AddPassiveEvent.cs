using System.Collections.Generic;
using GameCommon.Data;

public class AddPassiveEvent : IUnitEvent
{
	public int SkillId;

	public float GetTriggerValue => -1f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = new List<ITarget> { unitModel };
		if (trigger is AddPassiveSkillTrigger addPassiveSkillTrigger)
		{
			return addPassiveSkillTrigger.SkillId == SkillId;
		}
		return false;
	}
}
