using System.Collections.Generic;
using GameCommon.Data;

public class BuffEvent : IUnitEvent
{
	public ITarget SourceTarget;

	public int BuffId;

	public int BuffStack;

	public float GetTriggerValue => 0f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = null;
		return false;
	}
}
