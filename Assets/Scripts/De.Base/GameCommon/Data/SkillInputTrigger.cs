using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class SkillInputTrigger : SkillTrigger
	{
		public ESkillInputEvent SkillInputType;
	}
}
