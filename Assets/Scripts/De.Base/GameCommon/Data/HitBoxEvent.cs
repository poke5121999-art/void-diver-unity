using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class HitBoxEvent : ActionEvent
	{
		public ESpawnPositionType SpawnPositionType;

		public float TargetPlayerMaxDistance;

		public int Id;

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
