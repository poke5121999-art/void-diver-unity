using System;

namespace GameCommon.Data
{
	[Serializable]
	public abstract class SkillTrigger
	{
		public string Text;

		public virtual bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
