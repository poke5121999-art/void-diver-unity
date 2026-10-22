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
			result = null;
			return false;
		}

		public string[] ExtractExpressions()
		{
			return null;
		}
	}
}
