using System.Text;
using System;
using System.Linq;
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
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			result = ((expression == "Percent") ? Percent.ToFloatString() : null);
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[1] { "Percent" }).ToArray();
		}

		public override bool Validate(out string errorMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool result = true;
			if (!ValidateBaseBuffEffect(out var errorMessage2))
			{
				result = false;
				stringBuilder.Append(errorMessage2);
			}
			if (ElementalType == EElementalType.None)
			{
				result = false;
				stringBuilder.AppendLine("ElementalType is None in DamageConversionEffect");
			}
			errorMessage = stringBuilder.ToString();
			return result;
		}
	}
}
