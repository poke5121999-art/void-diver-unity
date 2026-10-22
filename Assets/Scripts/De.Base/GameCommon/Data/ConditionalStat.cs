using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class ConditionalStat
	{
		public float Probability;

		public ConditionalStatConditionGroup SourceCondition;

		public ConditionalStatConditionGroup TargetCondition;

		public EStatType StatType;

		public float StatValue;

		public bool TryResolveExpression(string expression, out string result)
		{
			result = null;
			return false;
		}

		public string[] ExtractExpressions()
		{
			return null;
		}
	}
}
