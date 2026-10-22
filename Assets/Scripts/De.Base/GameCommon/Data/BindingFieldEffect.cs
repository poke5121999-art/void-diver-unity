using GameCommon.Enum;

namespace GameCommon.Data
{
	public class BindingFieldEffect : BuffEffect
	{
		public float Range;

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
