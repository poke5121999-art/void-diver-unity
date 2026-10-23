using System.Linq;
using System.Text;
using System;

namespace GameCommon.Data
{
	[Serializable]
	public abstract class DotBuffEffect : BuffEffect
	{
		public int TickAmount;

		public override bool TryResolveExpression(string expression, out string result)
		{
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			result = ((expression == "TickAmount") ? TickAmount.ToString() : null);
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[1] { "TickAmount" }).ToArray();
		}

		public bool ValidateBaseDotBuffEffect(out string errorMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool result = true;
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
