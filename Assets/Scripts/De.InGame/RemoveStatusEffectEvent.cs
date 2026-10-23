using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;

public class RemoveStatusEffectEvent : IUnitEvent
{
	public ITarget SourceTarget;

	public EStatusEffectTag EffectTags;

	public float GetTriggerValue => -1f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = new List<ITarget> { unitModel };
		return false;
	}
}
