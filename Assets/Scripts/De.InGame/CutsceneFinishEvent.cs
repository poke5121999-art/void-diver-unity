using System.Collections.Generic;
using GameCommon.Data;

public class CutsceneFinishEvent : IUnitEvent
{
	public ITarget SourceTarget;

	public float GetTriggerValue => 0f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = null;
		return false;
	}
}
