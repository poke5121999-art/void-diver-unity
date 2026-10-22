using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class StressStatEntry
	{
		public EStressState StressState;

		public EStatType StatType;

		public float StatValue;

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
