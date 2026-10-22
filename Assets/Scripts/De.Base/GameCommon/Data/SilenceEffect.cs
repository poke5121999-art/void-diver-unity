using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class SilenceEffect : BuffEffect
	{
		public override EBuffEffectType EffectType => EBuffEffectType.Silence;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
