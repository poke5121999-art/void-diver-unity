using System.Collections.Generic;
using GameCommon.Data;

public interface IUnitEvent
{
	float GetTriggerValue { get; }

	bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets);
}
