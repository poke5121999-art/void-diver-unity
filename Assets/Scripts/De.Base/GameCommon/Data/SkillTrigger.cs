using System;

namespace GameCommon.Data
{
	[Serializable]
	public abstract class SkillTrigger
	{
		public string Text = "설명 없음";

		public virtual bool Validate(out string errorMessage)
		{
			errorMessage = string.Empty;
			return true;
		}
	}
}
