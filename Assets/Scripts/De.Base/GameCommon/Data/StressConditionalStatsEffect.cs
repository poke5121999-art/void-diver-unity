using System.Text;
using System.Linq;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class StressConditionalStatsEffect : BuffEffect
	{
		public StressStatEntry[] Entries;

		public override EBuffEffectType EffectType => EBuffEffectType.StressConditionalStats;

		public override bool TryResolveExpression(string expression, out string result)
		{
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			const string prefix = "Entries[";
			if (expression.StartsWith(prefix))
			{
				int close = expression.IndexOf(']', prefix.Length);
				if (close <= prefix.Length)
				{
					result = null;
					return false;
				}
				string indexText = expression.Substring(prefix.Length, close - prefix.Length);
				if (!int.TryParse(indexText, out int index) || Entries == null || index >= Entries.Length || expression[close + 1] != '.')
				{
					result = null;
					return false;
				}
				string rest = expression.Substring(close + 2, expression.Length - (close + 2));
				return Entries[index].TryResolveExpression(rest, out result);
			}
			result = null;
			return false;
		}

		public override string[] ExtractExpressions()
		{
			if (Entries == null || Entries.Length == 0)
			{
				return base.ExtractExpressions();
			}
			return base.ExtractExpressions().Concat(Entries.SelectMany((entry, i) => new string[3] { "StressState", "StatType", "StatValue" }.Select(e => string.Format("Entries[{0}].{1}", i, e)))).ToArray();
		}

		public override bool Validate(out string errorMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool result = true;
			if (!ValidateBaseBuffEffect(out string baseMessage))
			{
				result = false;
				stringBuilder.Append(baseMessage);
			}
			if (Entries == null || Entries.Length == 0)
			{
				result = false;
				stringBuilder.Append("Entries가 비어있습니다.");
			}
			else
			{
				for (int i = 0; i < Entries.Length; i++)
				{
					if ((int)Entries[i].StatType == 0)
					{
						result = false;
						stringBuilder.Append(string.Format("{0}[{1}]의 StatType이 설정되지 않았습니다.", "Entries", i));
					}
				}
			}
			errorMessage = stringBuilder.ToString();
			return result;
		}
	}
}
