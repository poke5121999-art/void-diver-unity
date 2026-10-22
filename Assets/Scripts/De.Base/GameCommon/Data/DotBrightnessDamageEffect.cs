using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class DotBrightnessDamageEffect : DotBuffEffect
	{
		public override EBuffEffectType EffectType => EBuffEffectType.DotBrightnessDamage;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
