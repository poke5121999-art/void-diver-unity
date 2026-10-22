using System.Text;
using System.Linq;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class ChangeSkillEffect : BuffEffect
	{
		public int OriginalSkillId;

		public int ReplacementSkillId;

		public override EBuffEffectType EffectType => EBuffEffectType.ChangeSkill;

		public override bool TryResolveExpression(string expression, out string result)
		{
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			if (expression == "OriginalSkillId")
			{
				result = OriginalSkillId.ToString();
			}
			else if (expression == "ReplacementSkillId")
			{
				result = ReplacementSkillId.ToString();
			}
			else
			{
				result = null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[2] { "OriginalSkillId", "ReplacementSkillId" }).ToArray();
		}

		public override bool Validate(out string errorMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			if (!ValidateBaseBuffEffect(out var errorMessage2))
			{
				flag = false;
				stringBuilder.Append(errorMessage2);
			}
			if (OriginalSkillId < 1)
			{
				flag = false;
				stringBuilder.Append("OriginalSkillId가 0 이하입니다. ");
			}
			if (ReplacementSkillId < 1)
			{
				flag = false;
				stringBuilder.Append("ReplacementSkillId가 0 이하입니다. ");
			}
			errorMessage = stringBuilder.ToString();
			return flag;
		}
	}
}
