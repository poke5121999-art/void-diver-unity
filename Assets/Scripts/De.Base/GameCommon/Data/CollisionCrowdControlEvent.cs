using System;

namespace GameCommon.Data
{
	[Serializable]
	public class CollisionCrowdControlEvent : CollisionEvent
	{
		public CcInfo CcInfo;

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
