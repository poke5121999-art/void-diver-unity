using System.Text;
using System.Linq;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class StatEffect : BuffEffect
	{
		public EStatType statType;

		public float statValue;

		public bool showEffect;

		public override EBuffEffectType EffectType => EBuffEffectType.Stat;

		public override bool TryResolveExpression(string expression, out string result)
		{
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			if (expression == "StatValue")
			{
				result = statValue.ToString();
			}
			else
			{
				result = null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[1] { "StatValue" }).ToArray();
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
			if ((int)statType == 0)
			{
				result = false;
				stringBuilder.Append(string.Format("개발자 확인 : {0}의 StatType이 설정되지 않았습니다", this));
			}
			errorMessage = stringBuilder.ToString();
			return result;
		}
	}
}
