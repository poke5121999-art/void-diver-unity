using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class ForcedSelfReviveEffect : BuffEffect
	{
		public bool TriggerOnReviveFail;

		public override EBuffEffectType EffectType => EBuffEffectType.ForcedSelfRevive;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
