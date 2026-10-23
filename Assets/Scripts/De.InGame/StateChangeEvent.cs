using System;
using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;

public class StateChangeEvent : IUnitEvent
{
	public EActionState State;

	public EActionState PreviousState;

	public float GetTriggerValue => -1f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = new List<ITarget> { unitModel };
		if (trigger is StateChangeSkillTrigger stateChangeSkillTrigger)
		{
			return (Array.IndexOf(stateChangeSkillTrigger.States, State) >= 0) != stateChangeSkillTrigger.Exclude;
		}
		return false;
	}
}
