using System.Linq;
using System.Text;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class FrontGuardEffect : BuffEffect
	{
		public float DamageReductionPercent;

		public float AngleDeg = 180f;

		public float VfxForwardPos = 1.5f;

		public string Vfx;

		public float VfxDuration = 1f;

		public string Sfx;

		public override EBuffEffectType EffectType => EBuffEffectType.FrontGuard;

		public override bool TryResolveExpression(string expression, out string result)
		{
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			if (expression == "DamageReductionPercent")
			{
				result = DamageReductionPercent.ToFloatString();
			}
			else if (expression == "AngleDeg")
			{
				result = AngleDeg.ToFloatString();
			}
			else if (expression == "VfxForwardPos")
			{
				result = VfxForwardPos.ToFloatString();
			}
			else if (expression == "VfxDuration")
			{
				result = VfxDuration.ToFloatString();
			}
			else
			{
				result = null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[4] { "DamageReductionPercent", "AngleDeg", "VfxForwardPos", "VfxDuration" }).ToArray();
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
			if (DamageReductionPercent < 0f)
			{
				result = false;
				stringBuilder.Append(string.Format("{0}의 DamageReductionPercent는 0 이상이어야 합니다\n", this));
			}
			if (AngleDeg < 0f || AngleDeg > 360f)
			{
				result = false;
				stringBuilder.Append(string.Format("{0}의 AngleDeg는 0 ~ 360 범위여야 합니다\n", this));
			}
			if (VfxDuration < 0f)
			{
				result = false;
				stringBuilder.Append(string.Format("{0}의 VfxDuration는 0 이상이어야 합니다\n", this));
			}
			errorMessage = stringBuilder.ToString();
			return result;
		}
	}
}
