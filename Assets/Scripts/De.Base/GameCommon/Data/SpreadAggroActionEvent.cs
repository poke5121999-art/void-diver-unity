using System;

namespace GameCommon.Data
{
	[Serializable]
	public class SpreadAggroActionEvent : ActionEvent
	{
		public float Distance;

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
