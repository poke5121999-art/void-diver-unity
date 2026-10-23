using GameCommon.Table;
using System.Linq;
using System.Text;
using System;

namespace GameCommon.Data
{
	[Serializable]
	public class CollisionTargetableHitBoxEvent : CollisionEvent
	{
		public int hitBoxId;

		public override bool TryResolveExpression(string expression, out string result)
		{
			if (base.TryResolveExpression(expression, out result))
			{
				return true;
			}
			if (expression == "HitBoxId")
			{
				result = string.Format("{0}", hitBoxId);
			}
			else
			{
				result = null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return Array.Empty<string>().Concat(new string[1] { "HitBoxId" }).ToArray();
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
			if (!THitBox.Table.ContainsKey(hitBoxId))
			{
				flag = false;
				stringBuilder.AppendLine(string.Format("{0}.{1} [{2}] is not exist in  {3}", "CollisionTargetableHitBoxEvent", "hitBoxId", hitBoxId, "THitBox"));
			}
			errorMessage = stringBuilder.ToString();
			return flag;
		}
	}
}
