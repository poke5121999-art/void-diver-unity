using System.Text;
using GameCommon.Table;
using System.Linq;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class ShieldActionEvent : ActionEvent
	{
		public int ShieldId;

		public int Value = 1;

		public EStatType CoefficientStat;

		public float StatFactor;

		public float TriggerValueFactor;

		public override bool TryResolveExpression(string expression, out string result)
		{
			string text = expression.ToLower();
			result = (text == "starttime") ? startTime.ToFloatString() : null;
			if (result != null)
			{
				return true;
			}
			text = expression.ToLower();
			if (text == "shieldid")
			{
				result = string.Format("{0}", ShieldId);
			}
			else if (text == "value")
			{
				result = string.Format("{0}", Value);
			}
			else if (text == "statfactor")
			{
				result = StatFactor.ToFloatString();
			}
			else if (text == "triggervaluefactor")
			{
				result = TriggerValueFactor.ToFloatString();
			}
			else
			{
				result = null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[4] { "shieldid", "value", "statfactor", "triggervaluefactor" }).ToArray();
		}

		public override bool Validate(out string errorMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool isValid = true;
			if (!base.Validate(out string baseMessage))
			{
				stringBuilder.AppendLine(baseMessage);
				isValid = false;
			}
			if (!TShield.Table.ContainsKey(ShieldId))
			{
				isValid = false;
				stringBuilder.AppendLine(string.Format("ShieldId [{0}] is not exist in {1}", ShieldId, "TShield"));
			}
			if (Value < 1)
			{
				isValid = false;
				stringBuilder.AppendLine("Value must be > 0");
			}
			errorMessage = stringBuilder.ToString();
			return isValid;
		}
	}
}
