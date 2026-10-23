using System.Collections.Generic;
using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using Sirenix.Utilities;

public class NoticeCrowdControlEvent : IUnitEvent
{
	public UnitModel SourceModel;

	public UnitModel TargetModel;

	public CcInfo CcInfo;

	public float GetTriggerValue => -1f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = null;
		if (trigger is CrowdControlProvideSkillTrigger crowdControlProvideSkillTrigger)
		{
			if (SourceModel == null)
			{
				return false;
			}
			if (unitModel.NetworkObjectId != SourceModel.NetworkObjectId)
			{
				return false;
			}
			triggerTargets = new List<ITarget> { TargetModel };
			if (!crowdControlProvideSkillTrigger.ConditionCcTypes.IsNullOrEmpty() && crowdControlProvideSkillTrigger.ConditionCcTypes.All((ECrowdControlType _) => (CcInfo.ccType & _) != _))
			{
				return false;
			}
			return true;
		}
		return false;
	}
}
