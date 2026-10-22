using System;
using UnityEngine.Serialization;

namespace GameCommon.Data
{
	[Serializable]
	public class HitBoxCollectActionEvent : ActionEvent
	{
		[FormerlySerializedAs("TargetableHitBoxIds")]
		public int[] HitBoxIds;

		public float CollectingHitBoxSpeed;

		public override bool TryResolveExpression(string expression, out string result)
		{
			result = null;
			return false;
		}

		public override string[] ExtractExpressions()
		{
			return null;
		}

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
