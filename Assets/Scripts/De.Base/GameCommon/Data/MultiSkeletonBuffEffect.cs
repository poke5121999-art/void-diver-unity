using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class MultiSkeletonBuffEffect : BuffEffect
	{
		public int[] AdditionalSkeletonIndices;

		public override EBuffEffectType EffectType => EBuffEffectType.None;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
