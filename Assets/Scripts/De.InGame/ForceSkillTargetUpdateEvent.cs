using System.Collections.Generic;
using GameCommon.Data;

public class ForceSkillTargetUpdateEvent : IUnitEvent
{
	public List<ITarget> NextTargets;

	public float GetTriggerValue => -1f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = new List<ITarget> { unitModel };
		if (trigger is ForceSkillTargetUpdateSkillTrigger && NextTargets != null && NextTargets.Count != 0)
		{
			triggerTargets = NextTargets;
			return true;
		}
		return false;
	}
}
