using System;
using System.Collections.Generic;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class NoticeDieSkillTrigger : SkillTrigger
	{
		public EUnitType SourceUnitType;

		public List<int> SourceUnitIds = new List<int>();

		public EUnitType TargetUnitType;

		public List<int> TargetUnitIds = new List<int>();

		public EHitTarget SourceRelation = EHitTarget.All;

		public EHitTarget TargetRelation = EHitTarget.All;

		public float TriggerRange;

		public ECompareType TriggerRangeCompareType = ECompareType.LessEqual;
	}
}
