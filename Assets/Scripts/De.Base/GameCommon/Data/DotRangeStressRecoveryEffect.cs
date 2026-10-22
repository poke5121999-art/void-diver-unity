using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class DotRangeStressRecoveryEffect : DotRangeBuffEffect
	{
		public override EBuffEffectType EffectType => EBuffEffectType.DotRangeStressRecovery;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
