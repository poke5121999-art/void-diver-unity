using System.Collections.Generic;
using GameCommon.Data;
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
		triggerTargets = null;
		return false;
	}
}
