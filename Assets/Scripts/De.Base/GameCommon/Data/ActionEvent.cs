using System;
using System.Collections.Generic;
using System.Text;
using GameCommon.Enum;
using GameCommon.Table;

namespace GameCommon.Data
{
	[Serializable]
	public abstract class ActionEvent
	{
		public List<BuffCondition> BuffConditionList = new List<BuffCondition>();

		public List<TalentCondition> TalentConditionList = new List<TalentCondition>();

		public string text = "설명 없음";

		public ESkillActionTarget ActionTarget = ESkillActionTarget.Self;

		public float startTime;

		public string TypeAndText => "[" + GetType().Name + "]_" + text;

		public virtual bool TryResolveExpression(string expression, out string result)
		{
			result = ((expression.ToLower() == "starttime") ? startTime.ToFloatString() : null);
			return result != null;
		}

		public virtual string[] ExtractExpressions()
		{
			return new string[1] { "starttime" };
		}

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
			if (TalentConditionList != null)
			{
				TalentConditionList.ForEach(delegate(TalentCondition _)
				{
					if (!TTalent.Table.ContainsKey(_.TalentId))
					{
						isValid = false;
						stringBuilder.AppendLine(string.Format("{0} TalentId {1} is not exist in  {2}", "TalentConditionList", _.TalentId, "TTalent"));
					}
				});
			}
			errorMessage = stringBuilder.ToString();
			return isValid;
		}
	}
}
