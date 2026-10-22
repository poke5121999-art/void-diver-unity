using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class CollisionBuffEvent : CollisionEvent
	{
		public float Probability;

		public EMonsterCategory FactionCondition;

		public ECrowdControlType CcCondition;

		public EStatusEffectTag EffectTag;

		public EMonsterType MonsterCondition;

		public int buffId;

		public int BuffStack;

		public override bool TryResolveExpression(string expression, out string result)
		{
			result = null;
			return false;
		}

		public override string[] ExtractExpressions()
		{
			return null;
		}

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
