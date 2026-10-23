using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class ActiveSkillTagBuffTrigger : BuffTriggerBase
	{
		public ESkillTagType[] SkillTags = Array.Empty<ESkillTagType>();

		public bool MatchAny = true;
	}
}
