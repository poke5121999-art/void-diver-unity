using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class DotRangeBrightnessDamageEffect : DotRangeBuffEffect
	{
		public override EBuffEffectType EffectType => EBuffEffectType.DotRangeBrightnessDamage;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
