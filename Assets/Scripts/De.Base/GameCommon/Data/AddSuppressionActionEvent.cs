using System;
using System.Linq;

namespace GameCommon.Data
{
	[Serializable]
	public class AddSuppressionActionEvent : ActionEvent
	{
		public float Amount;

		public override bool TryResolveExpression(string expression, out string result)
		{
			result = ((expression.ToLower() == "starttime") ? startTime.ToFloatString() : null);
			if (result != null)
			{
				return true;
			}
			result = ((expression.ToLower() == "amount") ? Amount.ToFloatString() : null);
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[1] { "amount" }).ToArray();
		}
	}
}
