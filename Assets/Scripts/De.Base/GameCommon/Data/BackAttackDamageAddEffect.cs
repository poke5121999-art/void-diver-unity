using System.Linq;
using GameCommon.Enum;

namespace GameCommon.Data
{
	public class BackAttackDamageAddEffect : BuffEffect
	{
		public float Percent;

		public override EBuffEffectType EffectType => EBuffEffectType.BackAttackDamageAdd;

		public override bool TryResolveExpression(string expression, out string result)
		{
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			if (expression == "Percent")
			{
				result = Percent.ToFloatString();
			}
			else
			{
				result = null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[1] { "Percent" }).ToArray();
		}

		public override bool Validate(out string errorMessage)
		{
			return ValidateBaseBuffEffect(out errorMessage);
		}
	}
}
