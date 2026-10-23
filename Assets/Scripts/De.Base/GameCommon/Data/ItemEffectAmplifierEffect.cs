using System.Text;
using System.Linq;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class ItemEffectAmplifierEffect : BuffEffect
	{
		public float Percent;

		public bool ShowEffect;

		public override EBuffEffectType EffectType => EBuffEffectType.ItemEffectAmplifier;

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
			bool result = true;
			if (!ValidateBaseBuffEffect(out string baseMessage))
			{
				result = false;
				stringBuilder.Append(baseMessage);
			}
			errorMessage = stringBuilder.ToString();
			return result;
		}
	}
}
