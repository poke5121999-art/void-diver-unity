using GameCommon.Enum;

namespace GameCommon.Data
{
	public class DamageConversionEffect : BuffEffect
	{
		public EElementalType ElementalType;

		public float Percent;

		public override EBuffEffectType EffectType => EBuffEffectType.DamageConversion;

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
