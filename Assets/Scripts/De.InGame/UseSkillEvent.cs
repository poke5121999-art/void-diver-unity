using GameCommon.Table;
using System.Linq;
using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;

public class UseSkillEvent : IUnitEvent
{
	public int SkillId;

	public ITarget TriggerTarget;

	public Queue<ESkillInputEvent> CachedSkillInput = new Queue<ESkillInputEvent>();

	public float GetTriggerValue => -1f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = new List<ITarget> { (TriggerTarget != null) ? TriggerTarget : unitModel };
		if (trigger is UseSkillByIdSkillTrigger useSkillByIdSkillTrigger)
		{
			if (useSkillByIdSkillTrigger.ConditionSkillIds == null || useSkillByIdSkillTrigger.ConditionSkillIds.Length == 0)
			{
				return true;
			}
			return useSkillByIdSkillTrigger.ConditionSkillIds.Contains(SkillId);
		}
		if (trigger is UseSkillByTagSkillTrigger useSkillByTagSkillTrigger)
		{
			if (useSkillByTagSkillTrigger.ConditionSkillTags == null || useSkillByTagSkillTrigger.ConditionSkillTags.Length == 0)
			{
				return true;
			}
			if (TSkill.Table.TryGetValue(SkillId, out var skillData) && skillData != null)
			{
				return useSkillByTagSkillTrigger.ConditionSkillTags.Any((ESkillTagType tag) => skillData.SkillTags.Contains(tag));
			}
		}
		return false;
	}
}
