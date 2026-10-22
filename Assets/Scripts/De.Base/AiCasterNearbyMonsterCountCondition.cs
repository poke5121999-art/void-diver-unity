using System;
using GameCommon.Enum;

[Serializable]
public class AiCasterNearbyMonsterCountCondition : AiCasterCondition
{
	public int MonsterId;

	public float DistanceValue;

	public ECompareType DistanceCompareType;

	public int CountValue;

	public ECompareType CountCompareType;
}
