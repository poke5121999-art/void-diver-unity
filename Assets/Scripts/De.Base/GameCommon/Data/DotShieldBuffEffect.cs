using System.Linq;
using System.Text;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class DotShieldBuffEffect : DotBuffEffect
	{
		public int ShieldId;

		public int Value;

		public override EBuffEffectType EffectType => EBuffEffectType.DotShield;

		public override bool TryResolveExpression(string expression, out string result)
		{
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			if (expression == "ShieldId")
			{
				result = ShieldId.ToString();
			}
			else
			{
				result = ((expression == "Value") ? Value.ToString() : null);
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[2] { "ShieldId", "Value" }).ToArray();
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
			if (ShieldId <= 0)
			{
				result = false;
				stringBuilder.Append("ShieldId는 0 초과여야 합니다");
			}
			if (Value < 0)
			{
				result = false;
				stringBuilder.Append("Value는 0 이상이어야 합니다");
			}
			errorMessage = stringBuilder.ToString();
			return result;
		}
	}
}
