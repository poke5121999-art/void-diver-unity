using GameCommon.Table;
using System.Linq;
using System.Text;
using System;

namespace GameCommon.Data
{
	[Serializable]
	public class CollisionStressRecoveryOwnerEvent : CollisionEvent
	{
		public int AmountMin;

		public int AmountMax;

		public override bool TryResolveExpression(string expression, out string result)
		{
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			if (expression == "AmountMin")
			{
				result = string.Format("{0}", AmountMin);
			}
			else if (expression == "AmountMax")
			{
				result = string.Format("{0}", AmountMax);
			}
			else
			{
				result = null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return Array.Empty<string>().Concat(new string[2] { "AmountMin", "AmountMax" }).ToArray();
		}
	}
}
