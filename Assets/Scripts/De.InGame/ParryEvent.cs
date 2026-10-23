using System.Collections.Generic;
using GameCommon.Data;
using UnityEngine;

public class ParryEvent : IUnitEvent
{
	public UnitModel SourceAttacker;

	public Vector3 HitPosition;

	public int SkillId;

	public float GetTriggerValue => -1f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = new List<ITarget> { unitModel };
		if (trigger is ParrySkillTrigger)
		{
			triggerTargets = new List<ITarget> { SourceAttacker };
			return true;
		}
		return false;
	}
}
