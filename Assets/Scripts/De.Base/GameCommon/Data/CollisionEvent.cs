using System;
using System.Collections.Generic;
using GameCommon.Enum;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCommon.Data
{
	[Serializable]
	public abstract class CollisionEvent
	{
		public bool IsOnce;

		[FormerlySerializedAs("buffConditionList")]
		public List<BuffCondition> BuffConditionList;

		public List<TalentCondition> talentConditionList;

		[SerializeReference]
		public List<HitableCondition> HitableConditions;

		public EHitTarget targetType;

		public virtual bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}

		public virtual bool TryResolveExpression(string expression, out string result)
		{
			result = null;
			return false;
		}

		public virtual string[] ExtractExpressions()
		{
			return null;
		}
	}
}
