using System;

namespace GameCommon.Data
{
	[Serializable]
	public class RecoveryProvideSkillTrigger : SkillTrigger
	{
		public int[] ConditionSkillIds;

		public bool ConditionIgnoreRegen;
	}
}
