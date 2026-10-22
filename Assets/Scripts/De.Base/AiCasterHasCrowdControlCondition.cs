using System;
using GameCommon.Enum;

[Serializable]
public class AiCasterHasCrowdControlCondition : AiCasterCondition
{
	public ECrowdControlType CrowdControlType;
}
