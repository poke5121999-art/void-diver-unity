using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class BuffActionEvent : ActionEvent
	{
		public int Id;

		public int stackAmount;

		public bool IsRemove;

		public bool RemoveOnActionEnd;

		public EMonsterCategory FactionCondition;

		public EMonsterType MonsterCondition;

		public override bool TryResolveExpression(string expression, out string result)
		{
			result = null;
			return false;
		}

		public override string[] ExtractExpressions()
		{
			return null;
		}

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
