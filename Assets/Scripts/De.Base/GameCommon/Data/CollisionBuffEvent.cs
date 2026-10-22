using GameCommon.Table;
using System.Text;
using System.Linq;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class CollisionBuffEvent : CollisionEvent
	{
		public float Probability = 100f;

		public EMonsterCategory FactionCondition;

		public ECrowdControlType CcCondition;

		public EStatusEffectTag EffectTag;

		public EMonsterType MonsterCondition;

		public int buffId;

		public int BuffStack = 1;

		public override bool TryResolveExpression(string expression, out string result)
		{
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			string text = expression;
			if (text == "Probability")
			{
				result = Probability.ToFloatString();
			}
			else if (text == "BuffId")
			{
				result = string.Format("{0}", buffId);
			}
			else
			{
				result = (text == "BuffStack") ? string.Format("{0}", BuffStack) : null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return Array.Empty<string>().Concat(new string[3] { "Probability", "BuffId", "BuffStack" }).ToArray();
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
			if (!TBuff.Table.ContainsKey(buffId))
			{
				flag = false;
				stringBuilder.AppendLine(string.Format("{0}.{1} [{2}] is not exist in  {3}", "CollisionBuffEvent", "buffId", buffId, "TBuff"));
			}
			errorMessage = stringBuilder.ToString();
			return flag;
		}
	}
}
