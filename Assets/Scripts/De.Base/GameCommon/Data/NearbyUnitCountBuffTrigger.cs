using System.Linq;
using System;
using GameCommon.Enum;
using UnityEngine;

namespace GameCommon.Data
{
	[Serializable]
	public class NearbyUnitCountBuffTrigger : BuffTriggerBase
	{
		public EHitTarget Relation;

		public EUnitType UnitType;

		public float Range;

		public ECompareType CompareType = ECompareType.GreaterEqual;

		public float CompareValue = 1f;

		[SerializeReference]
		public BuffTriggerBase[] NearUnitConditionTriggers = Array.Empty<BuffTriggerBase>();

		public override bool TryResolveExpression(string expression, out string result)
		{
			result = null;
			string text = expression.ToLower();
			if (text == "range")
			{
				result = Range.ToFloatString();
			}
			else if (text == "comparevalue")
			{
				result = CompareValue.ToFloatString();
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return Array.Empty<string>().Concat(new string[2] { "range", "comparevalue" }).ToArray();
		}
	}
}
