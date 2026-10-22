using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class DotHpRecoveryEffect : DotBuffEffect
	{
		public override EBuffEffectType EffectType => EBuffEffectType.DotHpRecovery;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
