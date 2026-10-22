using System;
using System.Collections.Generic;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public abstract class ActionEvent
	{
		public List<BuffCondition> BuffConditionList;

		public List<TalentCondition> TalentConditionList;

		public string text;

		public ESkillActionTarget ActionTarget;

		public float startTime;

		public string TypeAndText => "[" + GetType().Name + "]_" + text;

		public virtual bool TryResolveExpression(string expression, out string result)
		{
			result = null;
			return false;
		}

		public virtual string[] ExtractExpressions()
		{
			return null;
		}

		public virtual bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
