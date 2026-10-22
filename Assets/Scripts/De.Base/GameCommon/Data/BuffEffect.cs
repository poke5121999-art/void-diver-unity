using System;
using System.Text;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public abstract class BuffEffect
	{
		public bool UseStatValue;

		public EStatType StatType;

		public float StatPercent;

		public bool RealtimeStatValue;

		public abstract EBuffEffectType EffectType { get; }

		public virtual bool TryResolveExpression(string expression, out string result)
		{
			if (expression == "EffectType")
			{
				result = EffectType.ToExpressionText();
			}
			else if (expression == "StatType")
			{
				result = StatType.ToExpressionText();
			}
			else if (expression == "StatPercent")
			{
				result = StatPercent.ToFloatString();
			}
			else
			{
				result = null;
			}
			return result != null;
		}

		public virtual string[] ExtractExpressions()
		{
			return new string[3] { "EffectType", "StatType", "StatPercent" };
		}

		public abstract bool Validate(out string errorMessage);

		public virtual bool ValidateBaseBuffEffect(out string errorMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool result;
			if (EffectType == EBuffEffectType.None)
			{
				stringBuilder.Append(string.Format("개발자 확인 : {0}의  버프 효과가 설정되지 않았습니다", this));
				result = false;
			}
			else
			{
				result = true;
			}
			errorMessage = stringBuilder.ToString();
			return result;
		}
	}
}
