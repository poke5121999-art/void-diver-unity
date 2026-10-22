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
