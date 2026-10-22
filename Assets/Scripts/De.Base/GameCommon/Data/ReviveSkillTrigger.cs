using System;

namespace GameCommon.Data
{
	[Serializable]
	public class ReviveSkillTrigger : SkillTrigger
	{
		public bool OnlySelfRevive;
	}
}
