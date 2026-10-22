using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;
using UnityEngine;

public class StressDamageEvent : IUnitEvent
{
	public UnitModel SourceModel;

	public float Amount;

	public bool IsFixed;

	public bool ShowDamageEffect;

	public Vector3 HitPosition;

	public EStressReasonType ReasonType;

	public int SkillId;

	public float GetTriggerValue => 0f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = null;
		return false;
	}
}
