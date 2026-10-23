using System.Text;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class DotRangeHpDamageEffect : DotRangeBuffEffect
	{
		public override EBuffEffectType EffectType => EBuffEffectType.DotRangeHpDamage;

		public override bool Validate(out string errorMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool result = true;
			if (!ValidateBaseBuffEffect(out var errorMessage2))
			{
				result = false;
				stringBuilder.Append(errorMessage2);
			}
			if (!ValidateBaseDotRangeBuffEffect(out var errorMessage3))
			{
				result = false;
				stringBuilder.Append(errorMessage3);
			}
			errorMessage = stringBuilder.ToString();
			return result;
		}
	}
}
