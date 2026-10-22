using System;
using GameCommon.Enum;

[Serializable]
public class MonsterTypeCondition : HitableCondition
{
	public EMonsterType MonsterType;

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
