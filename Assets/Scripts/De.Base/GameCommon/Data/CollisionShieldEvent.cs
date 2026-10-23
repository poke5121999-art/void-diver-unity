using GameCommon.Table;
using System.Linq;
using System.Text;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class CollisionShieldEvent : CollisionEvent
	{
		public int ShieldId;

		public int Value = 1;

		public float Probability = 100f;

		public EMonsterCategory FactionCondition;

		public EMonsterType MonsterCondition;

		public override bool TryResolveExpression(string expression, out string result)
		{
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			if (expression == "ShieldId")
			{
				result = string.Format("{0}", ShieldId);
			}
			else if (expression == "Value")
			{
				result = string.Format("{0}", Value);
			}
			else if (expression == "Probability")
			{
				result = Probability.ToFloatString();
			}
			else
			{
				result = null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return Array.Empty<string>().Concat(new string[3] { "ShieldId", "Value", "Probability" }).ToArray();
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
			if (!TShield.Table.ContainsKey(ShieldId))
			{
				flag = false;
				stringBuilder.AppendLine(string.Format("ShieldId [{0}] is not exist in {1}", ShieldId, "TShield"));
			}
			if (Value < 1)
			{
				flag = false;
				stringBuilder.AppendLine("Value must be > 0");
			}
			errorMessage = stringBuilder.ToString();
			return flag;
		}
	}
}
