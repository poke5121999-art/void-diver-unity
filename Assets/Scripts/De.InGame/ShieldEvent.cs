using System.Collections.Generic;
using GameCommon.Data;

public class ShieldEvent : IUnitEvent
{
	public UnitModel SourceModel;

	public int ShieldId;

	public int Amount;

	public ulong CasterId;

	public float GetTriggerValue => Amount;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = new List<ITarget> { unitModel };
		return false;
	}
}
