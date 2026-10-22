using GameCommon.Table;
using System.Text;
using System.Linq;
using System;

namespace GameCommon.Data
{
	[Serializable]
	public class CollisionBrightnessDamageEvent : CollisionEvent
	{
		public int AmountMin;

		public int AmountMax;

		public override bool TryResolveExpression(string expression, out string result)
		{
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			string text = expression;
			if (text == "AmountMin")
			{
				result = string.Format("{0}", AmountMin);
			}
			else
			{
				result = (text == "AmountMax") ? string.Format("{0}", AmountMax) : null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return Array.Empty<string>().Concat(new string[2] { "AmountMin", "AmountMax" }).ToArray();
		}
	}
}
