using GameCommon.Table;
using System.Text;
using System.Linq;
using System;

namespace GameCommon.Data
{
	[Serializable]
	public class BrightnessRecoveryActionEvent : ActionEvent
	{
		public int AmountMin;

		public int AmountMax;

		public override bool TryResolveExpression(string expression, out string result)
		{
			string text = expression.ToLower();
			result = (text == "starttime") ? startTime.ToFloatString() : null;
			if (result != null)
			{
				return true;
			}
			if (text == "amountmin")
			{
				result = string.Format("{0}", AmountMin);
			}
			else
			{
				result = (text == "amountmax") ? string.Format("{0}", AmountMax) : null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[2] { "amountmin", "amountmax" }).ToArray();
		}
	}
}
