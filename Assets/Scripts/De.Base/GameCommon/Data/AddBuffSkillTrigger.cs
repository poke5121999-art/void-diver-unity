using System;

namespace GameCommon.Data
{
	[Serializable]
	public class AddBuffSkillTrigger : SkillTrigger
	{
		public int BuffId;

		public int BuffStack;
	}
}
