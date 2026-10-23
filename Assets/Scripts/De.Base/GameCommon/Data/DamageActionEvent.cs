using System.Linq;
using System;

namespace GameCommon.Data
{
	[Serializable]
	public class DamageActionEvent : ActionEvent
	{
		public DamageInfo DamageInfo;

		public override bool TryResolveExpression(string expression, out string result)
		{
			string text = expression.ToLower();
			result = (text == "starttime") ? startTime.ToFloatString() : null;
			if (result != null)
			{
				return true;
			}
			int num = expression.IndexOf('.');
			if (num >= 0)
			{
				string text2 = expression.Substring(0, num).ToLower();
				string expression2 = expression.Substring(num + 1, expression.Length - (num + 1));
				if (text2 == "damageinfo")
				{
					return DamageInfo.TryResolveExpression(expression2, out result);
				}
			}
			result = null;
			return false;
		}

		public override string[] ExtractExpressions()
		{
			return base.ExtractExpressions().Concat(DamageInfo.ExtractExpressions().Select((string e) => "DamageInfo." + e)).ToArray();
		}
	}
}
