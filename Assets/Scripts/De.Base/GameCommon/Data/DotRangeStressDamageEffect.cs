using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class DotRangeStressDamageEffect : DotRangeBuffEffect
	{
		public override EBuffEffectType EffectType => EBuffEffectType.DotRangeStressDamage;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
