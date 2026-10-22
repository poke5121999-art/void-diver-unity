using System;
using System.Collections.Generic;

namespace GameCommon.Data
{
	[Serializable]
	public class MonologueActionEvent : ActionEvent
	{
		[Serializable]
		public class MessageData
		{
			public string Text;

			public int Weight;
		}

		public List<MessageData> Messages;

		public int ActivationPercent;

		public override bool TryResolveExpression(string expression, out string result)
		{
			result = null;
			return false;
		}

		public override string[] ExtractExpressions()
		{
			return null;
		}

		public string GetRandomMessage()
		{
			return null;
		}
	}
}
