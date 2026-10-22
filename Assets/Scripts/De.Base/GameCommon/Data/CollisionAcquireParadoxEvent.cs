using GameCommon.Table;
using System.Text;
using System.Linq;
using System;

namespace GameCommon.Data
{
	[Serializable]
	public class CollisionAcquireParadoxEvent : CollisionEvent
	{
		public int ParadoxId;

		public float Probability = 100f;

		public bool IncludePositive;

		public bool IncludeNegative;

		public override bool TryResolveExpression(string expression, out string result)
		{
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			string text = expression;
			if (text == "ParadoxId")
			{
				result = string.Format("{0}", ParadoxId);
			}
			else
			{
				result = (text == "Probability") ? Probability.ToFloatString() : null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return Array.Empty<string>().Concat(new string[2] { "ParadoxId", "Probability" }).ToArray();
		}

		public override bool Validate(out string errorMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			if (!base.Validate(out var errorMessage2))
			{
				stringBuilder.AppendLine(errorMessage2);
				flag = false;
			}
			if (ParadoxId > 0 && !TParadox.Table.ContainsKey(ParadoxId))
			{
				flag = false;
				stringBuilder.AppendLine(string.Format("{0}.{1} [{2}] is not exist in  {3}", "CollisionAcquireParadoxEvent", "ParadoxId", ParadoxId, "TParadox"));
			}
			errorMessage = stringBuilder.ToString();
			return flag;
		}
	}
}
