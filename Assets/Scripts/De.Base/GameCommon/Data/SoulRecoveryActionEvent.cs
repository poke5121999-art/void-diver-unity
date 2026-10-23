using System.Linq;
using System;

namespace GameCommon.Data
{
	[Serializable]
	public class SoulRecoveryActionEvent : ActionEvent
	{
		public int Amount;

		public override bool TryResolveExpression(string expression, out string result)
		{
			string text = expression.ToLower();
			result = (text == "starttime") ? startTime.ToFloatString() : null;
			if (result != null)
			{
				return true;
			}
			text = expression.ToLower();
			if (text == "amount")
			{
				result = string.Format("{0}", Amount);
			}
			else
			{
				result = null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[1] { "amount" }).ToArray();
		}
	}
}
