using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class EscapeOnActiveEffect : BuffEffect
	{
		public override EBuffEffectType EffectType => EBuffEffectType.EscapeOnActive;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
