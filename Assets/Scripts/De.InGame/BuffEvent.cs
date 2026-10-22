using System.Collections.Generic;
using System.Linq;
using GameCommon.Data;

public class BuffEvent : IUnitEvent
{
	public ITarget SourceTarget;

	public int BuffId;

	public int BuffStack;

	public float GetTriggerValue => BuffStack;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = new List<ITarget> { unitModel };
		if (trigger is BuffSkillTrigger buffSkillTrigger)
		{
			triggerTargets = new List<ITarget> { SourceTarget };
			return buffSkillTrigger.ConditionBuffIds.Contains(BuffStack);
		}
		if (trigger is AddBuffSkillTrigger addBuffSkillTrigger && addBuffSkillTrigger.BuffId == BuffId)
		{
			return addBuffSkillTrigger.BuffStack == BuffStack;
		}
		return false;
	}
}
