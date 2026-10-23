using System.Linq;
using System.Text;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public abstract class DotRangeBuffEffect : BuffEffect
	{
		public EHitTarget TargetType;

		public float Range;

		public int TickAmount;

		public override bool TryResolveExpression(string expression, out string result)
		{
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			if (expression == "TickAmount")
			{
				result = TickAmount.ToString();
			}
			else
			{
				result = ((expression == "Range") ? Range.ToString() : null);
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[2] { "TickAmount", "Range" }).ToArray();
		}

		public bool ValidateBaseDotRangeBuffEffect(out string errorMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool result = true;
			if (Range < 0f)
			{
				result = false;
				stringBuilder.Append(string.Format("{0}의 Range는 0 이상이어야 합니다", this));
			}
			if (TickAmount < 0)
			{
				result = false;
				stringBuilder.Append(string.Format("{0}의 TickAmount는 0 이상이어야 합니다", this));
			}
			errorMessage = stringBuilder.ToString();
			return result;
		}
	}
}
