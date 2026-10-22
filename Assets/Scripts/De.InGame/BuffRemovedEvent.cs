using System.Collections.Generic;
using GameCommon.Data;

public class BuffRemovedEvent : IUnitEvent
{
	public ITarget SourceTarget;

	public int BuffId;

	public bool IsExpired;

	public float GetTriggerValue => 0f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = null;
		return false;
	}
}
