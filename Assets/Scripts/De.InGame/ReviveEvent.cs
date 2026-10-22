using System.Collections.Generic;
using GameCommon.Data;

public class ReviveEvent : IUnitEvent
{
	public UnitModel SourceModel;

	public bool IsSelfRevive;

	public bool IsEquipmentRevive;

	public float EquipmentReviveHpPercent;

	public float GetTriggerValue => 0f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = null;
		return false;
	}
}
