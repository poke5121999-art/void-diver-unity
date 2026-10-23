using System.Text;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class ParryEffect : BuffEffect
	{
		public bool IgnoreBackAttack;

		public bool IgnoreCriticalHit;

		public override EBuffEffectType EffectType => EBuffEffectType.Parry;

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
