using GameCommon.Table;
using System.Text;
using System.Linq;
using System;

namespace GameCommon.Data
{
	[Serializable]
	public class CollisionAddSuppressionEvent : CollisionEvent
	{
		public float Amount;

		public override bool TryResolveExpression(string expression, out string result)
		{
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			string text = expression;
			result = (text == "Amount") ? Amount.ToFloatString() : null;
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return Array.Empty<string>().Concat(new string[1] { "Amount" }).ToArray();
		}
	}
}
