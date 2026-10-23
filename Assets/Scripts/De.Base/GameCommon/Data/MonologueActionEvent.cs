using System.Linq;
using System;
using System.Collections.Generic;
using SomeNet.Utility;

namespace GameCommon.Data
{
	[Serializable]
	public class MonologueActionEvent : ActionEvent
	{
		[Serializable]
		public class MessageData
		{
			public string Text;

			public int Weight = 1;
		}

		public List<MessageData> Messages = new List<MessageData>();

		public int ActivationPercent = 100;

		public override bool TryResolveExpression(string expression, out string result)
		{
			string text = expression.ToLower();
			result = (text == "starttime") ? startTime.ToFloatString() : null;
			if (result != null)
			{
				return true;
			}
			text = expression.ToLower();
			if (text == "activationpercent")
			{
				result = string.Format("{0}", ActivationPercent);
			}
			else
			{
				result = null;
			}
			return result != null;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(new string[1] { "activationpercent" }).ToArray();
		}

		public string GetRandomMessage()
		{
			if (Messages != null)
			{
				List<MessageData> validMessages = Messages.Where((MessageData _) => _ != null && _.Weight >= 1 && !string.IsNullOrEmpty(_.Text)).ToList();
				if (validMessages != null && validMessages.Count != 0)
				{
					int totalWeight = validMessages.Sum((MessageData _) => _.Weight);
					if (totalWeight > 0)
					{
						int roll = RandomUtility.GetRandomInt(0, totalWeight);
						int accumulated = 0;
						foreach (MessageData message in validMessages)
						{
							accumulated += message.Weight;
							if (roll < accumulated)
							{
								return message.Text;
							}
						}
					}
				}
			}
			return string.Empty;
		}
	}
}
