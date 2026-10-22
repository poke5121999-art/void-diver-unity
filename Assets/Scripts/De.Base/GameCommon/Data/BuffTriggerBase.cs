using System;

namespace GameCommon.Data
{
	[Serializable]
	public abstract class BuffTriggerBase
	{
		public virtual bool TryResolveExpression(string expression, out string result)
		{
			result = null;
			return false;
		}

		public virtual string[] ExtractExpressions()
		{
			return null;
		}
	}
}
