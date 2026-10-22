using System;

namespace GameCommon.Data
{
	[Serializable]
	public class ConditionalStatConditionGroup
	{
		public bool IsAny;

		public ConditionalStatCondition[] Conditions;

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
