using System.Linq;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class DamageInfo
	{
		public EElementalType ElementalType;

		public int Power;

		public int PowerGrowth;

		public EStatType CoefficientStat;

		public float StatFactor;

		public float StatFactorGrowth;

		public ConditionalStat[] ConditionalStats;

		public bool IgnoreShield;

		public bool TryResolveExpression(string expression, out string result)
		{
			if (expression.StartsWith("ConditionalStats["))
			{
				int length = "ConditionalStats[".Length;
				int num = expression.IndexOf(']', length);
				if (num > length)
				{
					if (int.TryParse(expression.Substring(length, num - length), out var result2) && ConditionalStats != null && result2 >= 0 && result2 < ConditionalStats.Length && num + 2 < expression.Length && expression[num + 1] == '.')
					{
						string expression2 = expression.Substring(num + 2, expression.Length - (num + 2));
						return ConditionalStats[result2].TryResolveExpression(expression2, out result);
					}
					result = null;
					return false;
				}
			}
			switch (expression)
			{
			case "Power":
				result = string.Format("{0}", Power);
				break;
			case "PowerGrowth":
				result = string.Format("{0}", PowerGrowth);
				break;
			case "ElementalType":
				result = ElementalType.ToExpressionText();
				break;
			case "CoefficientStat":
				result = CoefficientStat.ToExpressionText();
				break;
			case "StatFactor":
				result = StatFactor.ToFloatString();
				break;
			case "StatFactorGrowth":
				result = StatFactorGrowth.ToFloatString();
				break;
			case "IgnoreShield":
				result = string.Format("{0}", IgnoreShield);
				break;
			default:
				result = null;
				break;
			}
			return result != null;
		}

		public string[] ExtractExpressions()
		{
			string[] array = new string[7] { "StatFactor", "StatFactorGrowth", "Power", "PowerGrowth", "ElementalType", "CoefficientStat", "IgnoreShield" };
			if (ConditionalStats == null || ConditionalStats.Length == 0)
			{
				return array;
			}
			return array.Concat(ConditionalStats.SelectMany((ConditionalStat c, int i) => c.ExtractExpressions().Select((string e) => string.Format("ConditionalStats[{0}].{1}", i, e)))).ToArray();
		}
	}
}
