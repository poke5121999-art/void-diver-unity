using System.Linq;
using System.Text;
using System;
using GameCommon.Enum;
using UnityEngine.Serialization;

namespace GameCommon.Data
{
	[Serializable]
	public class HitBoxIteratorEvent : ActionEvent
	{
		public ESpawnPositionType SpawnPositionType;

		[FormerlySerializedAs("MaxTargetPlayerDistance")]
		public float TargetPlayerMaxDistance = -1f;

		public HitBoxIterator HitBoxIterator;

		public bool CancelOnActionEnd = true;

		public bool CancelOnOwnerDying;

		public override bool TryResolveExpression(string expression, out string result)
		{
			string text = expression.ToLower();
			result = (text == "starttime") ? startTime.ToFloatString() : null;
			if (result != null)
			{
				return true;
			}
			result = (text == "targetplayermaxdistance") ? TargetPlayerMaxDistance.ToFloatString() : null;
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[1] { "targetplayermaxdistance" }).ToArray();
		}

		public override bool Validate(out string errorMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool result = true;
			if (!base.Validate(out var errorMessage2))
			{
				stringBuilder.AppendLine(errorMessage2);
				result = false;
			}
			if (!HitBoxIterator.Validate(out var errorMessage3))
			{
				stringBuilder.AppendLine(errorMessage3);
				result = false;
			}
			errorMessage = stringBuilder.ToString();
			return result;
		}
	}
}
