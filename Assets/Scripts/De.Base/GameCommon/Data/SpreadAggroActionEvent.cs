using System.Linq;
using System;

namespace GameCommon.Data
{
	[Serializable]
	public class SpreadAggroActionEvent : ActionEvent
	{
		public float Distance;

		public override bool TryResolveExpression(string expression, out string result)
		{
			string text = expression.ToLower();
			result = (text == "starttime") ? startTime.ToFloatString() : null;
			if (result != null)
			{
				return true;
			}
			text = expression.ToLower();
			if (text == "distance")
			{
				result = Distance.ToFloatString();
			}
			else
			{
				result = null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[1] { "distance" }).ToArray();
		}
	}
}
