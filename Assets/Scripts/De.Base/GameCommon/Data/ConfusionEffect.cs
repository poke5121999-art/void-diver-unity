using System.Text;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class ConfusionEffect : BuffEffect
	{
		public override EBuffEffectType EffectType => EBuffEffectType.Confusion;

		public override bool Validate(out string errorMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool result = true;
			if (!ValidateBaseBuffEffect(out var errorMessage2))
			{
				result = false;
				stringBuilder.Append(errorMessage2);
			}
			errorMessage = stringBuilder.ToString();
			return result;
		}
	}
}
