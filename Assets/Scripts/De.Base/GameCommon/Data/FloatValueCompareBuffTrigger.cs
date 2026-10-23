using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class FloatValueCompareBuffTrigger : BuffTriggerBase
	{
		public EBuffTrigger TriggerType;

		public ECompareType CompareType;

		public float CompareValue;

		public override bool TryResolveExpression(string expression, out string result)
		{
			result = null;
			if (expression.ToLower() == "comparevalue")
			{
				result = CompareValue.ToFloatString();
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return new string[1] { "comparevalue" };
		}
	}
}
