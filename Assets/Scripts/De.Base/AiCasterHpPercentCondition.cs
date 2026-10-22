using System;
using GameCommon.Enum;

[Serializable]
public class AiCasterHpPercentCondition : AiCasterCondition
{
	public float HpPercentValue;

	public ECompareType CompareType;
}
