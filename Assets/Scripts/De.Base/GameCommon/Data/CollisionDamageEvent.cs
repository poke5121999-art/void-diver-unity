using System;

namespace GameCommon.Data
{
	[Serializable]
	public class CollisionDamageEvent : CollisionEvent
	{
		public DamageInfo DamageInfo;

		public bool CanBackAttack;

		public override bool TryResolveExpression(string expression, out string result)
		{
			result = null;
			return false;
		}

		public override string[] ExtractExpressions()
		{
			return null;
		}
	}
}
