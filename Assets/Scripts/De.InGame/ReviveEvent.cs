using System.Collections.Generic;
using GameCommon.Data;

public class ReviveEvent : IUnitEvent
{
	public UnitModel SourceModel;

	public bool IsSelfRevive;

	public bool IsEquipmentRevive;

	public float EquipmentReviveHpPercent;

	public float GetTriggerValue => -1f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = new List<ITarget> { unitModel };
		if (trigger is ReviveSkillTrigger reviveSkillTrigger)
		{
			triggerTargets = new List<ITarget> { SourceModel ?? unitModel };
			if (!reviveSkillTrigger.OnlySelfRevive)
			{
				return true;
			}
			return IsSelfRevive;
		}
		return false;
	}
}
