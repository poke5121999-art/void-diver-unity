using System.Linq;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class ForceUpdateTargetActionEvent : ActionEvent
	{
		public EForceUpdateTargetPriorityType PriorityType = EForceUpdateTargetPriorityType.Random;

		public bool AllowCharacterTarget;

		public bool AllowMonsterTarget;

		public int[] MonsterIds;

		public bool AllowHitBox;

		public int[] HitBoxIds;

		public bool ThroughWalls;

		public float MaxDistance;

		public override bool TryResolveExpression(string expression, out string result)
		{
			string text = expression.ToLower();
			result = (text == "starttime") ? startTime.ToFloatString() : null;
			if (result != null)
			{
				return true;
			}
			result = (text == "maxdistance") ? MaxDistance.ToFloatString() : null;
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[1] { "maxdistance" }).ToArray();
		}
	}
}
