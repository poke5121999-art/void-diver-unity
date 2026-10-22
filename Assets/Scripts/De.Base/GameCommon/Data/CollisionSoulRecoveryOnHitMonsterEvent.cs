using System;
using System.Collections.Generic;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class CollisionSoulRecoveryOnHitMonsterEvent : CollisionEvent
	{
		public List<EMonsterType> MonsterTypes;

		public int AmountMin;

		public int AmountMax;

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
