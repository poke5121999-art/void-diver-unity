using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class ForceUpdateTargetActionEvent : ActionEvent
	{
		public EForceUpdateTargetPriorityType PriorityType;

		public bool AllowCharacterTarget;

		public bool AllowMonsterTarget;

		public int[] MonsterIds;

		public bool AllowHitBox;

		public int[] HitBoxIds;

		public bool ThroughWalls;

		public float MaxDistance;

		public override bool TryResolveExpression(string expression, out string result)
		{
			result = null;
			return false;
		}

		public override string[] ExtractExpressions()
		{
			return null;
		}
	}
}
