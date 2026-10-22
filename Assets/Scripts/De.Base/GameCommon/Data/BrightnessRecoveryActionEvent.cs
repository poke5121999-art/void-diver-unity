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
			result = null;
			return false;
		}

		public override string[] ExtractExpressions()
		{
			return null;
		}
	}
}
