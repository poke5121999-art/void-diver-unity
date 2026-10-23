using System.Linq;
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
			if (expression.StartsWith("Conditions["))
			{
				int length = "Conditions[".Length;
				int num = expression.IndexOf(']', length);
				if (num > length && int.TryParse(expression.Substring(length, num - length), out var result2) && Conditions != null && result2 < Conditions.Length && num + 2 < expression.Length && expression[num + 1] == '.')
				{
					string expression2 = expression.Substring(num + 2);
					return Conditions[result2].TryResolveExpression(expression2, out result);
				}
			}
			result = null;
			return false;
		}

		public string[] ExtractExpressions()
		{
			if (Conditions == null || Conditions.Length == 0)
			{
				return Array.Empty<string>();
			}
			return Conditions.SelectMany((ConditionalStatCondition c, int i) => c.ExtractExpressions().Select((string e) => string.Format("Conditions[{0}].{1}", i, e))).ToArray();
		}
	}
}
