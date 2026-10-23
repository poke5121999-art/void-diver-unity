using GameCommon.Table;
using System.Linq;
using System.Text;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class HitBoxEvent : ActionEvent
	{
		public ESpawnPositionType SpawnPositionType;

		public float TargetPlayerMaxDistance = -1f;

		public int Id;

		public override bool TryResolveExpression(string expression, out string result)
		{
			string text = expression.ToLower();
			result = (text == "starttime") ? startTime.ToFloatString() : null;
			if (result != null)
			{
				return true;
			}
			result = (text == "targetplayermaxdistance") ? TargetPlayerMaxDistance.ToFloatString() : null;
			if (result != null)
			{
				return true;
			}
			text = expression.ToLower();
			result = (text == "id") ? string.Format("{0}", Id) : null;
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[2] { "targetplayermaxdistance", "id" }).ToArray();
		}

		public override bool Validate(out string errorMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool result = true;
			if (!base.Validate(out var errorMessage2))
			{
				stringBuilder.AppendLine(errorMessage2);
				result = false;
			}
			if (!THitBox.Table.ContainsKey(Id))
			{
				result = false;
				stringBuilder.AppendLine(string.Format("{0} [{1}] is not exist in  {2}", "Id", Id, "THitBox"));
			}
			errorMessage = stringBuilder.ToString();
			return result;
		}
	}
}
