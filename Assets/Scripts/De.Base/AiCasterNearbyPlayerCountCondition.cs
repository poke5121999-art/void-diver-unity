using System;
using GameCommon.Enum;

[Serializable]
public class AiCasterNearbyPlayerCountCondition : AiCasterCondition
{
	public float DistanceValue;

	public ECompareType DistanceCompareType;

	public int CountValue;

	public ECompareType CountCompareType;
}
