using System;

namespace GameCommon.Data
{
	[Serializable]
	public class CrowdControlActionEvent : ActionEvent
	{
		public CcInfo CrowdControlInfo;

		public override string[] ExtractExpressions()
		{
			return null;
		}

		public override bool TryResolveExpression(string expression, out string result)
		{
			result = null;
			return false;
		}
	}
}
