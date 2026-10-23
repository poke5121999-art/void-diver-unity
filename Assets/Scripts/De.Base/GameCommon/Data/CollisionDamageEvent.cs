using GameCommon.Table;
using System.Linq;
using System.Text;
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
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			int num = expression.IndexOf('.');
			if (num >= 0)
			{
				string text = expression.Substring(0, num);
				string expression2 = expression.Substring(num + 1, expression.Length - (num + 1));
				if (text == "DamageInfo")
				{
					return DamageInfo.TryResolveExpression(expression2, out result);
				}
			}
			result = null;
			return false;
		}

		public override string[] ExtractExpressions()
		{
			return Array.Empty<string>().Concat(DamageInfo.ExtractExpressions().Select((string e) => "DamageInfo." + e)).ToArray();
		}
	}
}
