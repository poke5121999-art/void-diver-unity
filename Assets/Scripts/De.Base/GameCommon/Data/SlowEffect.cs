using System.Text;
using System.Linq;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class SlowEffect : BuffEffect
	{
		public ESlowType SlowType;

		public float value;

		public override EBuffEffectType EffectType => EBuffEffectType.Slow;

		public override bool TryResolveExpression(string expression, out string result)
		{
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			if (expression == "value")
			{
				result = value.ToFloatString();
			}
			else
			{
				result = null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[1] { "value" }).ToArray();
		}

		public override bool Validate(out string errorMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool result = true;
			if (!ValidateBaseBuffEffect(out string baseMessage))
			{
				result = false;
				stringBuilder.Append(baseMessage);
			}
			if ((int)SlowType == 0)
			{
				result = false;
				stringBuilder.Append(string.Format("개발자 확인 : {0}의 SlowType이 설정되지 않았습니다", this));
			}
			if (value <= 0f)
			{
				stringBuilder.Append(string.Format("{0}의 value는 0 이하여야 합니다", this));
				result = false;
			}
			errorMessage = stringBuilder.ToString();
			return result;
		}
	}
}
