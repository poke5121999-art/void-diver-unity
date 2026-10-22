using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class DotHpDamageEffect : DotBuffEffect
	{
		public override EBuffEffectType EffectType => EBuffEffectType.DotHpDamage;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
