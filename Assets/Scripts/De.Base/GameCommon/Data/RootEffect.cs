using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class RootEffect : BuffEffect
	{
		public override EBuffEffectType EffectType => EBuffEffectType.Root;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
