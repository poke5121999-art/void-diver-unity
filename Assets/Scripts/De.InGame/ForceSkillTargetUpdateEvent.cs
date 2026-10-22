using System.Collections.Generic;
using GameCommon.Data;

public class ForceSkillTargetUpdateEvent : IUnitEvent
{
	public List<ITarget> NextTargets;

	public float GetTriggerValue => 0f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = null;
		return false;
	}
}
