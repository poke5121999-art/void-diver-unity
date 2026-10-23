using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class MultiSkeletonBuffEffect : BuffEffect
	{
		public int[] AdditionalSkeletonIndices;

		public override EBuffEffectType EffectType => EBuffEffectType.MultiSkeleton;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = string.Empty;
			if (AdditionalSkeletonIndices == null || AdditionalSkeletonIndices.Length == 0)
			{
				errorMessage = "AdditionalSkeletonIndices가 비어있습니다.";
				return false;
			}
			return true;
		}
	}
}
