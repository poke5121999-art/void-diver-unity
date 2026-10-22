using System;
using GameCommon.Enum;

[Serializable]
public class AiTargetHpPercentCondition : AiTargetCondition
{
	public float HpPercentValue;

	public ECompareType CompareType;
}
