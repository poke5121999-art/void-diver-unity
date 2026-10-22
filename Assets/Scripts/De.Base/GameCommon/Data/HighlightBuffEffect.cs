using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class HighlightBuffEffect : BuffEffect
	{
		public EUnitHighlightColor HighlightColor;

		public EUnitHighlightType HighlightType;

		public override EBuffEffectType EffectType => EBuffEffectType.None;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
