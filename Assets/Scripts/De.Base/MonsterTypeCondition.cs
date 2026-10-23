using System;
using GameCommon.Enum;

[Serializable]
public class MonsterTypeCondition : HitableCondition
{
	public EMonsterType MonsterType = EMonsterType.MiniBoss;

	public override bool TryResolveExpression(string expression, out string result)
	{
		if (expression.ToLower() == "monstertype")
		{
			result = MonsterType.ToExpressionText();
		}
		else
		{
			result = null;
		}
		return result != null;
	}

	public override string[] ExtractExpressions()
	{
		return new string[1] { "MonsterType" };
	}
}
