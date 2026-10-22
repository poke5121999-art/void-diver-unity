using System.Text;
using System.Linq;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class ApplyBuffOnAttackEffect : BuffEffect
	{
		public int TargetBuffId;

		public float Probability;

		public override EBuffEffectType EffectType => EBuffEffectType.ApplyBuffOnAttack;

		public override bool TryResolveExpression(string expression, out string result)
		{
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			if (expression == "TargetBuffId")
			{
				result = TargetBuffId.ToString();
			}
			else
			{
				result = ((expression == "Probability") ? Probability.ToFloatString() : null);
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[2] { "TargetBuffId", "Probability" }).ToArray();
		}

		public override bool Validate(out string errorMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool result = true;
			if (!ValidateBaseBuffEffect(out var errorMessage2))
			{
				result = false;
			}
			if (TargetBuffId < 1)
			{
				result = false;
				stringBuilder.Append("TargetBuffId가 설정되지 않았습니다.");
			}
			if (Probability <= 0f || 100f < Probability)
			{
				result = false;
				stringBuilder.Append(string.Format("{0}는 0 초과 100 이하여야 합니다. (현재: {1})", "Probability", Probability));
			}
			errorMessage = stringBuilder.ToString();
			return result;
		}
	}
}
