using System.Text;
using GameCommon.Table;
using System.Linq;
using System;

namespace GameCommon.Data
{
	[Serializable]
	public class ShowConsumeGaugeActionEvent : ActionEvent
	{
		public int ItemId;

		public float Duration;

		public bool OtherPlayerCanSee;

		public override bool TryResolveExpression(string expression, out string result)
		{
			string text = expression.ToLower();
			result = (text == "starttime") ? startTime.ToFloatString() : null;
			if (result != null)
			{
				return true;
			}
			text = expression.ToLower();
			if (text == "itemid")
			{
				result = string.Format("{0}", ItemId);
			}
			else if (text == "duration")
			{
				result = Duration.ToFloatString();
			}
			else
			{
				result = null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[2] { "itemid", "duration" }).ToArray();
		}

		public override bool Validate(out string errorMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool isValid = true;
			if (!base.Validate(out string baseMessage))
			{
				stringBuilder.AppendLine(baseMessage);
				isValid = false;
			}
			if (!TItem.Table.ContainsKey(ItemId))
			{
				isValid = false;
				stringBuilder.AppendLine(string.Format("{0} [{1}] is not exist in  {2}", "ItemId", ItemId, "TItem"));
			}
			errorMessage = stringBuilder.ToString();
			return isValid;
		}
	}
}
