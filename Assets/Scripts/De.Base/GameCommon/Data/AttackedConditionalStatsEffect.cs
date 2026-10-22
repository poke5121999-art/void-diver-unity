using System.Text;
using System.Linq;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class AttackedConditionalStatsEffect : BuffEffect
	{
		public ConditionalStat[] ConditionalStats;

		public override EBuffEffectType EffectType => EBuffEffectType.AttackedConditionalStats;

		public override bool TryResolveExpression(string expression, out string result)
		{
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			if (expression.StartsWith("ConditionalStats["))
			{
				int length = "ConditionalStats[".Length;
				int num = expression.IndexOf(']', length);
				if (num > length && int.TryParse(expression.Substring(length, num - length), out var result2) && ConditionalStats != null && result2 >= 0 && result2 < ConditionalStats.Length && num + 2 < expression.Length && expression[num + 1] == '.')
				{
					string expression2 = expression.Substring(num + 2);
					return ConditionalStats[result2].TryResolveExpression(expression2, out result);
				}
			}
			result = null;
			return false;
		}

		public override string[] ExtractExpressions()
		{
			if (ConditionalStats == null || ConditionalStats.Length == 0)
			{
				return new string[3] { "EffectType", "StatType", "StatPercent" };
			}
			return base.ExtractExpressions().Concat(ConditionalStats.SelectMany((ConditionalStat c, int i) => c.ExtractExpressions().Select((string e) => string.Format("ConditionalStats[{0}].{1}", i, e)))).ToArray();
		}

		public override bool Validate(out string errorMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool result = true;
			if (!ValidateBaseBuffEffect(out var errorMessage2))
			{
				result = false;
				stringBuilder.Append(errorMessage2);
			}
			errorMessage = stringBuilder.ToString();
			return result;
		}
	}
}
