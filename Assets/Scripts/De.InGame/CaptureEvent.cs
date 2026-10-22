using System.Collections.Generic;
using GameCommon.Data;
using UnityEngine;

public class CaptureEvent : IUnitEvent
{
	public Vector3 HitPosition;

	public float GetTriggerValue => 0f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = null;
		return false;
	}
}
