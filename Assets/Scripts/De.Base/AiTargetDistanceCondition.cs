using System;
using GameCommon.Enum;

[Serializable]
public class AiTargetDistanceCondition : AiTargetCondition
{
	public float compareValue;

	public ECompareType compareType;
}
