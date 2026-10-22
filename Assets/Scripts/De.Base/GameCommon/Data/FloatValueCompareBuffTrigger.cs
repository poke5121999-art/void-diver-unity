using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class FloatValueCompareBuffTrigger : BuffTriggerBase
	{
		public EBuffTrigger TriggerType;

		public ECompareType CompareType;

		public float CompareValue;

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
