using System;
using System.Linq;
using System.Text;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class WeakPointStackAmplifierEffect : BuffEffect
	{
		public float Percent;

		public override EBuffEffectType EffectType => EBuffEffectType.WeakPointStackAmplifier;

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
			bool result;
			if (!ValidateBaseBuffEffect(out var baseError))
			{
				result = false;
				stringBuilder.Append(baseError);
			}
			else
			{
				result = true;
			}
			errorMessage = stringBuilder.ToString();
			return result;
		}
	}
}
