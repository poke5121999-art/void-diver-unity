using GameCommon.Table;
using System.Linq;
using System.Text;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class ConditionalStat
	{
		public float Probability = 100f;

		public ConditionalStatConditionGroup SourceCondition = new ConditionalStatConditionGroup();

		public ConditionalStatConditionGroup TargetCondition = new ConditionalStatConditionGroup();

		public EStatType StatType;

		public float StatValue;

		public bool TryResolveExpression(string expression, out string result)
		{
			if (expression == "Probability")
			{
				result = Probability.ToFloatString();
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
				if (expression.StartsWith("SourceCondition."))
				{
					return SourceCondition.TryResolveExpression(expression.Substring("SourceCondition.".Length), out result);
				}
				if (expression.StartsWith("TargetCondition."))
				{
					return TargetCondition.TryResolveExpression(expression.Substring("TargetCondition.".Length), out result);
				}
				result = null;
			}
			return result != null;
		}

		public string[] ExtractExpressions()
		{
			return new string[3] { "Probability", "StatType", "StatValue" }.Concat(SourceCondition.ExtractExpressions().Select((string e) => "SourceCondition." + e)).Concat(TargetCondition.ExtractExpressions().Select((string e) => "TargetCondition." + e)).ToArray();
		}
	}
}
