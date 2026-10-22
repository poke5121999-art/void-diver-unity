using System.Collections.Generic;
using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using Sirenix.Utilities;
using UnityEngine;

public class CrowdControlEvent : IUnitEvent
{
	public UnitModel SourceModel;

	public CcInfo CcInfo;

	public Vector3 KnockbackHitPosition;

	public Vector3 ForcedMoveDirection;

	public float? ForcedMoveDistance;

	public float GetTriggerValue => 0f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = new List<ITarget> { unitModel };
		if (trigger is CrowdControlSkillTrigger crowdControlSkillTrigger)
		{
			if (crowdControlSkillTrigger.ConditionCcTypes.IsNullOrEmpty())
			{
				return true;
			}
			return !crowdControlSkillTrigger.ConditionCcTypes.All((ECrowdControlType t) => (CcInfo.ccType & t) != t);
		}
		return false;
	}
}
