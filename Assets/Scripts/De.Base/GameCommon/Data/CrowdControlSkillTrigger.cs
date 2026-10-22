using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class CrowdControlSkillTrigger : SkillTrigger
	{
		public ECrowdControlType[] ConditionCcTypes;
	}
}
