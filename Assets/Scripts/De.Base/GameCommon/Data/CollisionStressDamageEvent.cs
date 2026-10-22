using System;

namespace GameCommon.Data
{
	[Serializable]
	public class CollisionStressDamageEvent : CollisionEvent
	{
		public int DamageMin;

		public int DamageMax;

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
