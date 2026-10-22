using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;

public class StateChangeEvent : IUnitEvent
{
	public EActionState State;

	public EActionState PreviousState;

	public float GetTriggerValue => 0f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = null;
		return false;
	}
}
