using System.Collections.Generic;
using GameCommon.Data;

public class ConsumeItemEvent : IUnitEvent
{
	public UnitModel SourceModel;

	public int ItemId;

	public float GetTriggerValue => 0f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = null;
		if (trigger is ConsumeItemSkillTrigger consumeItemSkillTrigger)
		{
			if (SourceModel == null)
			{
				return false;
			}
			if (unitModel.NetworkObjectId != SourceModel.NetworkObjectId)
			{
				return false;
			}
			if (consumeItemSkillTrigger.ItemId > 0 && consumeItemSkillTrigger.ItemId != ItemId)
			{
				return false;
			}
			triggerTargets = new List<ITarget> { SourceModel };
			return true;
		}
		return false;
	}
}
