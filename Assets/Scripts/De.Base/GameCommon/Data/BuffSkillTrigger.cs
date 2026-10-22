using System;

namespace GameCommon.Data
{
	[Serializable]
	public class BuffSkillTrigger : SkillTrigger
	{
		public int[] ConditionBuffIds;
	}
}
