using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class KillSkillTrigger : SkillTrigger
	{
		public EHitTarget TargetRelation;

		public float TriggerRange;

		public ECompareType TriggerRangeCompareType;
	}
}
