using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class CollisionHealEvent : CollisionEvent
	{
		public int Power;

		public int PowerGrowth;

		public EStatType CoefficientStat;

		public float StatFactor;

		public float StatFactorGrowth;

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
