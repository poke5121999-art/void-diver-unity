using System.Text;
using System.Linq;
using GameCommon.Enum;

namespace GameCommon.Data
{
	public class BindingFieldEffect : BuffEffect
	{
		public float Range;

		public override EBuffEffectType EffectType => EBuffEffectType.BindingField;

		public override bool TryResolveExpression(string expression, out string result)
		{
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			if (expression == "Range")
			{
				result = Range.ToFloatString();
			}
			else
			{
				result = null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[1] { "Range" }).ToArray();
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
