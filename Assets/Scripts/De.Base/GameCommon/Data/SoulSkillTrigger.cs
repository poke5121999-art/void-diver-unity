using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class SoulSkillTrigger : SkillTrigger
	{
		public int CompareValue;

		public ECompareType CompareType;
	}
}
