using System.Collections.Generic;
using System.Linq;
using GameCommon.Data;
using Sirenix.Utilities;

public class NoticeRecoveryEvent : IUnitEvent
{
	public UnitModel SourceModel;

	public UnitModel TargetModel;

	public int SkillId;

	public int Amount;

	public EHpRecoveryReasonType ReasonType;

	public float GetTriggerValue => Amount;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = null;
		if (trigger is RecoveryProvideSkillTrigger recoveryProvideSkillTrigger)
		{
			if (SourceModel == null)
			{
				return false;
			}
			if (unitModel.NetworkObjectId != SourceModel.NetworkObjectId)
			{
				return false;
			}
			if (!recoveryProvideSkillTrigger.ConditionSkillIds.IsNullOrEmpty() && !recoveryProvideSkillTrigger.ConditionSkillIds.Contains(SkillId))
			{
				return false;
			}
			if (recoveryProvideSkillTrigger.ConditionIgnoreRegen && ReasonType == EHpRecoveryReasonType.Regen)
			{
				return false;
			}
			triggerTargets = new List<ITarget> { TargetModel };
			return true;
		}
		return false;
	}
}
