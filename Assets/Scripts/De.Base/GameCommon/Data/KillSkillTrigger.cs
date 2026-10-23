using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class KillSkillTrigger : SkillTrigger
	{
		public EHitTarget TargetRelation = EHitTarget.All;

		public float TriggerRange;

		public ECompareType TriggerRangeCompareType = ECompareType.LessEqual;
	}
}
