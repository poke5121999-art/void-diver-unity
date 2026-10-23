using System.Collections.Generic;
using GameCommon.Data;

public class MoveFinishEvent : IUnitEvent
{
	public bool IsBlocked;

	public ITarget HitTarget;

	public float GetTriggerValue => -1f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = new List<ITarget> { (ITarget)HitTarget ?? unitModel };
		if (trigger is MoveFinishSkillTrigger moveFinishSkillTrigger)
		{
			if (!moveFinishSkillTrigger.TriggerOnlyWhenBlocked)
			{
				return true;
			}
			return IsBlocked;
		}
		return false;
	}
}
