using System;

[Serializable]
public class HpPercentCondition : HitableCondition
{
	public float HpPercent;

	public override bool TryResolveExpression(string expression, out string result)
	{
		result = null;
		return false;
	}

	public override string[] ExtractExpressions()
	{
		return null;
	}
}
