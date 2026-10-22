using System.Collections.Generic;
using GameCommon.Data;

public class NoticeReviveEvent : IUnitEvent
{
	public UnitModel SourceModel;

	public UnitModel TargetModel;

	public float GetTriggerValue => 0f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = null;
		return false;
	}
}
