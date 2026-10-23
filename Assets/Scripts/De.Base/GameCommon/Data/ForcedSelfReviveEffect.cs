using System.Text;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class ForcedSelfReviveEffect : BuffEffect
	{
		public bool TriggerOnReviveFail;

		public override EBuffEffectType EffectType => EBuffEffectType.ForcedSelfRevive;

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
