using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class LootingSpeedAmplifierEffect : BuffEffect
	{
		public float Percent;

		public override EBuffEffectType EffectType => EBuffEffectType.None;

		public bool ShowEffect => false;

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
