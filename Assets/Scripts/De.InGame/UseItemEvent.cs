using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;

public class UseItemEvent : IUnitEvent
{
	public int ItemId;

	public ESkillInputEvent SkillInputType;

	public float GetTriggerValue => -1f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = new List<ITarget> { unitModel };
		if (trigger is SkillInputTrigger skillInputTrigger)
		{
			return SkillInputType == skillInputTrigger.SkillInputType;
		}
		return false;
	}
}
