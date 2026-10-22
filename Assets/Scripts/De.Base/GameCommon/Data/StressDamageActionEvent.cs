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
			result = null;
			return false;
		}

		public override string[] ExtractExpressions()
		{
			return null;
		}
	}
}
