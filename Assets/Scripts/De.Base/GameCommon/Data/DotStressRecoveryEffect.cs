using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class DotStressRecoveryEffect : DotBuffEffect
	{
		public override EBuffEffectType EffectType => EBuffEffectType.None;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
