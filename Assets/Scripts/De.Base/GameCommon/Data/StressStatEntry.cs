using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class StressStatEntry
	{
		public EStressState StressState;

		public EStatType StatType;

		public float StatValue;

		public bool TryResolveExpression(string expression, out string result)
		{
			if (expression == "StressState")
			{
				result = StressState.ToExpressionText();
			}
			else if (expression == "StatType")
			{
				result = StatType.ToExpressionText();
			}
			else if (expression == "StatValue")
			{
				result = StatValue.ToFloatString();
			}
			else
			{
				result = null;
			}
			return result != null;
		}

		public string[] ExtractExpressions()
		{
			return new string[3] { "StressState", "StatType", "StatValue" };
		}
	}
}
