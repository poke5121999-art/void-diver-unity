using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class SkillBuffStackCondition
	{
		public int BuffId;

		public int StackCount;

		public ECompareType CompareType;
	}
}
