using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class ConfusionEffect : BuffEffect
	{
		public override EBuffEffectType EffectType => EBuffEffectType.Confusion;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
