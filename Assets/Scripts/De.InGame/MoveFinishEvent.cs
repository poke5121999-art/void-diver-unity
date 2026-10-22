using System.Collections.Generic;
using GameCommon.Data;

public class MoveFinishEvent : IUnitEvent
{
	public bool IsBlocked;

	public ITarget HitTarget;

	public float GetTriggerValue => 0f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = null;
		return false;
	}
}
