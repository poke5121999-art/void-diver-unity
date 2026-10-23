using System;

[Serializable]
public class HpPercentCondition : HitableCondition
{
	public float HpPercent = 10f;

	public override bool TryResolveExpression(string expression, out string result)
	{
		result = ((expression.ToLower() == "hppercent") ? HpPercent.ToFloatString() : null);
		return result != null;
	}

	public override string[] ExtractExpressions()
	{
		return new string[1] { "HpPercent" };
	}
}
