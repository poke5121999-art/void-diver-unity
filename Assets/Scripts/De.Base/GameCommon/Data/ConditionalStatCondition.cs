using System.Linq;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class ConditionalStatCondition
	{
		public EElementalType ElementalCondition;

		public EMonsterCategory FactionCondition;

		public ECrowdControlType CcCondition;

		public EStatusEffectTag EffectTag;

		public EMonsterType MonsterCondition;

		public bool TryResolveExpression(string expression, out string result)
		{
			if (expression == "ElementalCondition")
			{
				result = ElementalCondition.ToExpressionText();
			}
			else if (expression == "FactionCondition")
			{
				result = FactionCondition.ToExpressionText();
			}
			else if (expression == "CcCondition")
			{
				result = CcCondition.ToExpressionText();
			}
			else if (expression == "EffectTag")
			{
				result = EffectTag.ToExpressionText();
			}
			else if (expression == "MonsterCondition")
			{
				result = MonsterCondition.ToExpressionText();
			}
			else
			{
				result = null;
			}
			return result != null;
		}

		public string[] ExtractExpressions()
		{
			return new string[5] { "ElementalCondition", "FactionCondition", "CcCondition", "EffectTag", "MonsterCondition" };
		}
	}
}
