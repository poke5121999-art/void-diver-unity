using System.Text;
using System.Linq;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class BrightnessRecoveryAmplifierEffect : BuffEffect
	{
		public float Percent;

		public bool ShowEffect;

		public override EBuffEffectType EffectType => EBuffEffectType.BrightnessRecoveryAmplifier;

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
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			if (!ValidateBaseBuffEffect(out var errorMessage2))
			{
				flag = false;
				stringBuilder.Append(errorMessage2);
			}
			errorMessage = stringBuilder.ToString();
			return flag;
		}
	}
}
