using System.Text;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class DotStressDamageEffect : DotBuffEffect
	{
		public bool ShowDamageEffect = true;

		public override EBuffEffectType EffectType => EBuffEffectType.DotStressDamage;

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
			errorMessage = stringBuilder.ToString();
			return result;
		}
	}
}
