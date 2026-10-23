using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class CrowdControlProvideSkillTrigger : SkillTrigger
	{
		public ECrowdControlType[] ConditionCcTypes = Array.Empty<ECrowdControlType>();

		public CrowdControlProvideSkillTrigger()
		{
			Text = "설명 없음";
		}
	}
}
