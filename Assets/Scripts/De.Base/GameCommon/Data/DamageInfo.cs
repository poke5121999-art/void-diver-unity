using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class DamageInfo
	{
		public EElementalType ElementalType;

		public int Power;

		public int PowerGrowth;

		public EStatType CoefficientStat;

		public float StatFactor;

		public float StatFactorGrowth;

		public ConditionalStat[] ConditionalStats;

		public bool IgnoreShield;

		public bool TryResolveExpression(string expression, out string result)
		{
			result = null;
			return false;
		}

		public string[] ExtractExpressions()
		{
			return null;
		}
	}
}
