using System.Collections.Generic;
using GameCommon.Data;
using UnityEngine;

public class LightFuelDamageEvent : IUnitEvent
{
	public int Amount;

	public Vector3 HitPosition;

	public int SkillId;

	public float GetTriggerValue => Amount;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = new List<ITarget> { unitModel };
		return false;
	}
}
