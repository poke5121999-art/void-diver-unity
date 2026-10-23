using System.Linq;
using System;

namespace GameCommon.Data
{
	[Serializable]
	public class CrowdControlActionEvent : ActionEvent
	{
		public CcInfo CrowdControlInfo;

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(CrowdControlInfo.ExtractExpressions().Select((string e) => "CrowdControlInfo." + e)).ToArray();
		}

		public override bool TryResolveExpression(string expression, out string result)
		{
			result = (expression.ToLower() == "starttime") ? startTime.ToFloatString() : null;
			if (result != null)
			{
				return true;
			}
			int num = expression.IndexOf('.');
			if (num >= 0)
			{
				string text = expression.Substring(0, num);
				string expression2 = expression.Substring(num + 1, expression.Length - (num + 1));
				if (text == "CrowdControlInfo")
				{
					return CrowdControlInfo.TryResolveExpression(expression2, out result);
				}
			}
			result = null;
			return false;
		}
	}
}
