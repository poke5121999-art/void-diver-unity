using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class StressDamageSkillTrigger : SkillTrigger
	{
		public EStressReasonType ConditionStressReasonType;

		public bool ConditionIsOnlyParryable;
	}
}
