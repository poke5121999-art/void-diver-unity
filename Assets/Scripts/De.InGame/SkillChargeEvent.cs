using System.Collections.Generic;
using GameCommon.Data;

public class SkillChargeEvent : IUnitEvent
{
	public UnitModel SourceModel;

	public float Amount;

	public float GetTriggerValue => Amount;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = new List<ITarget> { unitModel };
		return false;
	}
}
