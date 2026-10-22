using System;

namespace GameCommon.Data
{
	[Serializable]
	public class UseSkillByIdSkillTrigger : SkillTrigger
	{
		public int[] ConditionSkillIds;
	}
}
