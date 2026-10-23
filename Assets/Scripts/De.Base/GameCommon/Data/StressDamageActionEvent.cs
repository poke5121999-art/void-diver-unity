using System.Linq;
using System;

namespace GameCommon.Data
{
	[Serializable]
	public class StressDamageActionEvent : ActionEvent
	{
		public float AmountMin;

		public float AmountMax;

		public override bool TryResolveExpression(string expression, out string result)
		{
			string text = expression.ToLower();
			result = (text == "starttime") ? startTime.ToFloatString() : null;
			if (result != null)
			{
				return true;
			}
			text = expression.ToLower();
			if (text == "amountmin")
			{
				result = AmountMin.ToFloatString();
			}
			else if (text == "amountmax")
			{
				result = AmountMax.ToFloatString();
			}
			else
			{
				result = null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[2] { "amountmin", "amountmax" }).ToArray();
		}
	}
}
