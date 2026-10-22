using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;

public class UseSkillEvent : IUnitEvent
{
	public int SkillId;

	public ITarget TriggerTarget;

	public Queue<ESkillInputEvent> CachedSkillInput;

	public float GetTriggerValue => 0f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = null;
		return false;
	}
}
