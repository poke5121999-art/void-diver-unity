using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class CrowdControlProvideSkillTrigger : SkillTrigger
	{
		public ECrowdControlType[] ConditionCcTypes;
	}
}
