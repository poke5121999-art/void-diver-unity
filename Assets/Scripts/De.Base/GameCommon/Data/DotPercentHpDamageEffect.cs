using System.Linq;
using System.Text;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class DotPercentHpDamageEffect : DotBuffEffect
	{
		public float Percent;

		public override EBuffEffectType EffectType => EBuffEffectType.DotPercentHpDamage;

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
			if (!ValidateBaseDotBuffEffect(out var errorMessage3))
			{
				result = false;
				stringBuilder.Append(errorMessage3);
			}
			if (Percent < 0f)
			{
				result = false;
				stringBuilder.AppendLine(string.Format("{0}의 Percent는 0 이상이어야 합니다", this));
			}
			errorMessage = stringBuilder.ToString();
			return result;
		}
	}
}
