using System;
using System.Collections.Generic;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class NoticeDieSkillTrigger : SkillTrigger
	{
		public EUnitType SourceUnitType;

		public List<int> SourceUnitIds;

		public EUnitType TargetUnitType;

		public List<int> TargetUnitIds;

		public EHitTarget SourceRelation;

		public EHitTarget TargetRelation;

		public float TriggerRange;

		public ECompareType TriggerRangeCompareType;
	}
}
