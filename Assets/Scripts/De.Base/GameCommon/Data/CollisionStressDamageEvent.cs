using GameCommon.Table;
using System.Linq;
using System.Text;
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
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			if (expression == "DamageMin")
			{
				result = string.Format("{0}", DamageMin);
			}
			else if (expression == "DamageMax")
			{
				result = string.Format("{0}", DamageMax);
			}
			else
			{
				result = null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return Array.Empty<string>().Concat(new string[2] { "DamageMin", "DamageMax" }).ToArray();
		}
	}
}
