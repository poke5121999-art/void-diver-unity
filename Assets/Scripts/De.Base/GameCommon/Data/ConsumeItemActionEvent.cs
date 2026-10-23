using GameCommon.Table;
using System.Linq;
using System.Text;
using System;

namespace GameCommon.Data
{
	[Serializable]
	public class ConsumeItemActionEvent : ActionEvent
	{
		public int ItemId;

		public override bool TryResolveExpression(string expression, out string result)
		{
			string text = expression.ToLower();
			result = (text == "starttime") ? startTime.ToFloatString() : null;
			if (result != null)
			{
				return true;
			}
			text = expression.ToLower();
			result = (text == "itemid") ? string.Format("{0}", ItemId) : null;
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[1] { "itemid" }).ToArray();
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
			if (!TItem.Table.ContainsKey(ItemId))
			{
				result = false;
				stringBuilder.AppendLine(string.Format("{0} [{1}] is not exist in  {2}", "ItemId", ItemId, "TItem"));
			}
			errorMessage = stringBuilder.ToString();
			return result;
		}
	}
}
