using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class DotStressDamageEffect : DotBuffEffect
	{
		public bool ShowDamageEffect;

		public override EBuffEffectType EffectType => EBuffEffectType.DotStressDamage;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
