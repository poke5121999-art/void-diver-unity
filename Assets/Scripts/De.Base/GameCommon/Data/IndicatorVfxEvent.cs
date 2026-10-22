using System;
using GameCommon.Enum;
using UnityEngine.Serialization;

namespace GameCommon.Data
{
	[Serializable]
	public class IndicatorVfxEvent : ActionEvent
	{
		public ESpawnPositionType SpawnPositionType;

		[FormerlySerializedAs("MaxTargetPlayerDistance")]
		public float TargetPlayerMaxDistance;

		public IndicatorInfo IndicatorInfo;

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
