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

		public ECompareType CompareType;

		public float CompareValue;

		[SerializeReference]
		public BuffTriggerBase[] NearUnitConditionTriggers;

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
