using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public abstract class DotRangeBuffEffect : BuffEffect
	{
		public EHitTarget TargetType;

		public float Range;

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

		public bool ValidateBaseDotRangeBuffEffect(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
