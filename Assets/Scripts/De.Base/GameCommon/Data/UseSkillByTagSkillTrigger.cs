using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class UseSkillByTagSkillTrigger : SkillTrigger
	{
		public ESkillTagType[] ConditionSkillTags = Array.Empty<ESkillTagType>();
	}
}
