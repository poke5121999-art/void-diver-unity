using GameCommon.Table;
using System.Text;
using System.Linq;
using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class BuffActionEvent : ActionEvent
	{
		public int Id;

		public int stackAmount = 1;

		public bool IsRemove;

		public bool RemoveOnActionEnd;

		public EMonsterCategory FactionCondition;

		public EMonsterType MonsterCondition;

		public override bool TryResolveExpression(string expression, out string result)
		{
			string text = expression.ToLower();
			result = (text == "starttime") ? startTime.ToFloatString() : null;
			if (result != null)
			{
				return true;
			}
			if (text == "id")
			{
				result = string.Format("{0}", Id);
			}
			else
			{
				result = (text == "stackamount") ? string.Format("{0}", stackAmount) : null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[2] { "id", "stackamount" }).ToArray();
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
			if (!TBuff.Table.ContainsKey(Id))
			{
				flag = false;
				stringBuilder.AppendLine(string.Format("{0} [{1}] is not exist in  {2}", "Id", Id, "TBuff"));
			}
			errorMessage = stringBuilder.ToString();
			return flag;
		}
	}
}
