using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class ItemBlockedEffect : BuffEffect
	{
		public override EBuffEffectType EffectType => EBuffEffectType.ItemBlocked;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
