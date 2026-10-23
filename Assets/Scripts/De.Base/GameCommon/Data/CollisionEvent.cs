using System;
using System.Collections.Generic;
using System.Text;
using GameCommon.Table;
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
		public List<BuffCondition> BuffConditionList = new List<BuffCondition>();

		public List<TalentCondition> talentConditionList = new List<TalentCondition>();

		[SerializeReference]
		public List<HitableCondition> HitableConditions = new List<HitableCondition>();

		public EHitTarget targetType;

		public virtual bool Validate(out string errorMessage)
		{
			bool isValid = true;
			StringBuilder stringBuilder = new StringBuilder();
			if (BuffConditionList != null)
			{
				BuffConditionList.ForEach(delegate(BuffCondition _)
				{
					if (!TBuff.Table.ContainsKey(_.BuffId))
					{
						isValid = false;
						stringBuilder.AppendLine(string.Format("{0} BuffId {1} is not exist in {2}", "BuffConditionList", _.BuffId, "TBuff"));
					}
				});
			}
			if (talentConditionList != null)
			{
				talentConditionList.ForEach(delegate(TalentCondition _)
				{
					if (!TTalent.Table.ContainsKey(_.TalentId))
					{
						isValid = false;
						stringBuilder.AppendLine(string.Format("{0} TalentId {1} is not exist in  {2}", "talentConditionList", _.TalentId, "TTalent"));
					}
				});
			}
			errorMessage = stringBuilder.ToString();
			return isValid;
		}

		public virtual bool TryResolveExpression(string expression, out string result)
		{
			const string prefix = "hitableconditions[";
			if (expression != null && expression.ToLower().StartsWith(prefix))
			{
				int num = expression.IndexOf(']', prefix.Length);
				if (num > prefix.Length
					&& int.TryParse(expression.Substring(prefix.Length, num - prefix.Length), out var result2)
					&& HitableConditions != null
					&& result2 >= 0 && result2 < HitableConditions.Count
					&& num + 2 < expression.Length
					&& expression[num + 1] == '.')
				{
					string expression2 = expression.Substring(num + 2, expression.Length - (num + 2));
					return HitableConditions[result2].TryResolveExpression(expression2, out result);
				}
			}
			result = null;
			return false;
		}

		public virtual string[] ExtractExpressions()
		{
			return Array.Empty<string>();
		}
	}
}
