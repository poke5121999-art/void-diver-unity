using System;
using GameCommon.Enum;

[Serializable]
public class AiTargetHasCrowdControlCondition : AiTargetCondition
{
	public ECrowdControlType CrowdControlType;
}
