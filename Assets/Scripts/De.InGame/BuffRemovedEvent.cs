using System.Collections.Generic;
using GameCommon.Data;

public class BuffRemovedEvent : IUnitEvent
{
	public ITarget SourceTarget;

	public int BuffId;

	public bool IsExpired;

	public float GetTriggerValue => -1f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = new List<ITarget> { unitModel };
		if (trigger is BuffRemovedSkillTrigger buffRemoved && (!buffRemoved.OnlyExpired || IsExpired))
		{
			triggerTargets = new List<ITarget> { unitModel };
			return buffRemoved.BuffId == BuffId;
		}
		return false;
	}
}
