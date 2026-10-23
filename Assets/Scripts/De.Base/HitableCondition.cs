using System;

[Serializable]
public abstract class HitableCondition
{
	public virtual bool TryResolveExpression(string expression, out string result)
	{
		result = null;
		return false;
	}

	public virtual string[] ExtractExpressions()
	{
		return Array.Empty<string>();
	}
}
