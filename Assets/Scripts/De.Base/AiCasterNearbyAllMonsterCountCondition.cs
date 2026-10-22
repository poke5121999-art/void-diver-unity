using System;
using GameCommon.Enum;

[Serializable]
public class AiCasterNearbyAllMonsterCountCondition : AiCasterCondition
{
	public float DistanceValue;

	public ECompareType DistanceCompareType;

	public int CountValue;

	public ECompareType CountCompareType;
}
