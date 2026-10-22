using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;

public class NoticeDamageEvent : IUnitEvent
{
	public UnitModel SourceModel;

	public UnitModel TargetModel;

	public int SkillId;

	public int Damage;

	public bool IsCritical;

	public bool IsBackAttack;

	public bool IsFirstHit;

	public EHpDamageReasonType ReasonType;

	public float GetTriggerValue => 0f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = null;
		return false;
	}
}
