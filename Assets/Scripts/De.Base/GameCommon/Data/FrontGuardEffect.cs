using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class FrontGuardEffect : BuffEffect
	{
		public float DamageReductionPercent;

		public float AngleDeg;

		public float VfxForwardPos;

		public string Vfx;

		public float VfxDuration;

		public string Sfx;

		public override EBuffEffectType EffectType => EBuffEffectType.None;

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
