using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class HpThresholdEffect : BuffEffect
	{
		public float ThresholdPercent;

		public override EBuffEffectType EffectType => EBuffEffectType.HpThreshold;

		public override bool TryResolveExpression(string expression, out string result)
		{
			result = null;
			return false;
		}

		public override string[] ExtractExpressions()
		{
			return null;
		}

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
