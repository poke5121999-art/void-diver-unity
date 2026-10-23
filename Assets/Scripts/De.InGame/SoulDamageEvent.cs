using GameCommon.Enum;
using System.Collections.Generic;
using GameCommon.Data;

public class SoulDamageEvent : IUnitEvent
{
	public int Amount;

	public float GetTriggerValue => Amount;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = new List<ITarget> { unitModel };
		if (trigger is SoulSkillTrigger soulSkillTrigger)
		{
			return ValueComparer.Compare(unitModel.Soul, soulSkillTrigger.CompareType, soulSkillTrigger.CompareValue);
		}
		return false;
	}
}
