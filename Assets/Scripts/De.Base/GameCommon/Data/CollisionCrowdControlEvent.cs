using System.Linq;
using System;

namespace GameCommon.Data
{
	[Serializable]
	public class CollisionCrowdControlEvent : CollisionEvent
	{
		public CcInfo CcInfo;

		public override bool TryResolveExpression(string expression, out string result)
		{
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			if (expression == null)
			{
				result = null;
				return false;
			}
			int num = expression.IndexOf('.');
			if (num >= 0)
			{
				string text = expression.Substring(0, num);
				string expression2 = expression.Substring(num + 1, expression.Length - (num + 1));
				if (text == "CcInfo")
				{
					return CcInfo.TryResolveExpression(expression2, out result);
				}
			}
			result = null;
			return false;
		}

		public override string[] ExtractExpressions()
		{
			return Array.Empty<string>().Concat(CcInfo.ExtractExpressions().Select((string e) => "CcInfo." + e)).ToArray();
		}
	}
}
