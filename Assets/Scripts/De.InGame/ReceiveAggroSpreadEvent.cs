using System.Collections.Generic;
using GameCommon.Data;

public class ReceiveAggroSpreadEvent : IUnitEvent
{
	public ITarget Target;

	public int ChainDepth;

	public float GetTriggerValue => -1f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = new List<ITarget> { unitModel };
		return false;
	}
}
