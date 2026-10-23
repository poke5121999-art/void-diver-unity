using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;
using UnityEngine;

public class NoticeDieEvent : IUnitEvent
{
	public UnitModel SourceModel;

	public UnitIdentity SourceIdentity;

	public UnitModel TargetModel;

	public float GetTriggerValue => -1f;

	public bool CheckSkillTriggered(UnitModel unitModel, SkillTrigger trigger, out List<ITarget> triggerTargets)
	{
		triggerTargets = new List<ITarget> { unitModel };
		if (trigger is NoticeDieSkillTrigger noticeDieSkillTrigger)
		{
			if (noticeDieSkillTrigger.SourceUnitType != EUnitType.None && SourceIdentity.UnitType != noticeDieSkillTrigger.SourceUnitType)
			{
				return false;
			}
			if (noticeDieSkillTrigger.SourceUnitIds != null && noticeDieSkillTrigger.SourceUnitIds.Count >= 1 && !noticeDieSkillTrigger.SourceUnitIds.Contains(SourceIdentity.TableId))
			{
				return false;
			}
			if (noticeDieSkillTrigger.TargetUnitType != EUnitType.None)
			{
				if (TargetModel == null)
				{
					return false;
				}
				if (TargetModel.UnitType != noticeDieSkillTrigger.TargetUnitType)
				{
					return false;
				}
			}
			if (noticeDieSkillTrigger.TargetUnitIds != null && noticeDieSkillTrigger.TargetUnitIds.Count > 0)
			{
				if (TargetModel == null)
				{
					return false;
				}
				if (!noticeDieSkillTrigger.TargetUnitIds.Contains(TargetModel.TableId))
				{
					return false;
				}
			}
			if (TargetModel == null)
			{
				return false;
			}
			if (noticeDieSkillTrigger.SourceRelation != EHitTarget.None && noticeDieSkillTrigger.SourceRelation != EHitTarget.All)
			{
				if (SourceModel == null)
				{
					return false;
				}
				if (!noticeDieSkillTrigger.SourceRelation.IsHitable(unitModel.GetHitLayerInfo(), SourceModel.GetHitLayerInfo(), SourceModel.HitLayer))
				{
					return false;
				}
			}
			if (noticeDieSkillTrigger.TargetRelation != EHitTarget.None && noticeDieSkillTrigger.TargetRelation != EHitTarget.All)
			{
				if (!noticeDieSkillTrigger.TargetRelation.IsHitable(unitModel.GetHitLayerInfo(), TargetModel.GetHitLayerInfo(), TargetModel.HitLayer))
				{
					return false;
				}
			}
			if (noticeDieSkillTrigger.TriggerRange > 0f && !ValueComparer.Compare(Vector3.Distance(unitModel.Position, TargetModel.Position), noticeDieSkillTrigger.TriggerRangeCompareType, noticeDieSkillTrigger.TriggerRange))
			{
				return false;
			}
			triggerTargets = new List<ITarget> { TargetModel };
			return true;
		}
		return false;
	}
}
