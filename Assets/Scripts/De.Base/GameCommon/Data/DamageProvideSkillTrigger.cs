using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class DamageProvideSkillTrigger : SkillTrigger
	{
		public int[] ConditionSkillIds;

		public bool ConditionIsOnlyCritical;

		public bool ConditionIsOnlyFirstHit;

		public EHpDamageReasonType ConditionReasonTypes;
	}
}
