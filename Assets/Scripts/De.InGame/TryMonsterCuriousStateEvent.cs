using System.Collections.Generic;
using GameCommon.Data;
using UnityEngine;

public class TryMonsterCuriousStateEvent : IUnitEvent
{
	public Vector3 TargetPosition;

	public float GetTriggerValue => -1f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = new List<ITarget> { unitModel };
		return false;
	}
}
