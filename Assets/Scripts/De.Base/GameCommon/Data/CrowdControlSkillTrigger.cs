using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class CrowdControlSkillTrigger : SkillTrigger
	{
		public ECrowdControlType[] ConditionCcTypes = Array.Empty<ECrowdControlType>();

		public CrowdControlSkillTrigger()
		{
			Text = "설명 없음";
		}
	}
}
