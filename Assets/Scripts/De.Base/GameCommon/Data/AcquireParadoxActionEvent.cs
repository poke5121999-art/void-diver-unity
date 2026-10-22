using System;
using System.Linq;

namespace GameCommon.Data
{
	[Serializable]
	public class AcquireParadoxActionEvent : ActionEvent
	{
		public int ParadoxId;

		public float Probability = 100f;

		public bool IncludePositive;

		public bool IncludeNegative;

		public override bool TryResolveExpression(string expression, out string result)
		{
			string text = expression.ToLower();
			result = ((text == "starttime") ? startTime.ToFloatString() : null);
			if (result != null)
			{
				return true;
			}
			if (text == "paradoxid")
			{
				result = string.Format("{0}", ParadoxId);
			}
			else
			{
				result = ((text == "probability") ? Probability.ToFloatString() : null);
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[2] { "paradoxid", "probability" }).ToArray();
		}
	}
}
