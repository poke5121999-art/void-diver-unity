using System;

namespace GameCommon.Data
{
	[Serializable]
	public class RecoveryProvideSkillTrigger : SkillTrigger
	{
		public int[] ConditionSkillIds = Array.Empty<int>();

		public bool ConditionIgnoreRegen;
	}
}
