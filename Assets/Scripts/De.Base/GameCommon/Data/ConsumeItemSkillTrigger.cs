using System;

namespace GameCommon.Data
{
	[Serializable]
	public class ConsumeItemSkillTrigger : SkillTrigger
	{
		public int ItemId;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
