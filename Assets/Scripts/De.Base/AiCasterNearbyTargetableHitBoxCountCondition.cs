using System;
using GameCommon.Enum;

[Serializable]
public class AiCasterNearbyTargetableHitBoxCountCondition : AiCasterCondition
{
	public int TargetableHitBoxId;

	public float DistanceValue;

	public ECompareType DistanceCompareType;

	public int CountValue;

	public ECompareType CountCompareType;
}
