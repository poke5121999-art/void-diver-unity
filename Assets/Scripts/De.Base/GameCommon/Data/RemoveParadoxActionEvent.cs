using System;

namespace GameCommon.Data
{
	[Serializable]
	public class RemoveParadoxActionEvent : ActionEvent
	{
		public int ParadoxId;

		public float Probability;

		public bool IncludePositive;

		public bool IncludeNegative;

		public override bool TryResolveExpression(string expression, out string result)
		{
			result = null;
			return false;
		}

		public override string[] ExtractExpressions()
		{
			return null;
		}
	}
}
