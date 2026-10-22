using System;

namespace GameCommon.Data
{
	[Serializable]
	public abstract class DotBuffEffect : BuffEffect
	{
		public int TickAmount;

		public override bool TryResolveExpression(string expression, out string result)
		{
			result = null;
			return false;
		}

		public override string[] ExtractExpressions()
		{
			return null;
		}

		public bool ValidateBaseDotBuffEffect(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
