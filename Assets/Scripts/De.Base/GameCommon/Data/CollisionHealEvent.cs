using GameCommon.Table;
using System.Linq;
using System.Text;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class CollisionHealEvent : CollisionEvent
	{
		public int Power;

		public int PowerGrowth;

		public EStatType CoefficientStat;

		public float StatFactor;

		public float StatFactorGrowth;

		public override bool TryResolveExpression(string expression, out string result)
		{
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			if (expression == "Power")
			{
				result = string.Format("{0}", Power);
			}
			else if (expression == "PowerGrowth")
			{
				result = string.Format("{0}", PowerGrowth);
			}
			else if (expression == "CoefficientStat")
			{
				result = CoefficientStat.ToString();
			}
			else if (expression == "StatFactor")
			{
				result = StatFactor.ToFloatString();
			}
			else if (expression == "StatFactorGrowth")
			{
				result = StatFactorGrowth.ToFloatString();
			}
			else
			{
				result = null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return Array.Empty<string>().Concat(new string[5] { "Power", "PowerGrowth", "CoefficientStat", "StatFactor", "StatFactorGrowth" }).ToArray();
		}
	}
}
