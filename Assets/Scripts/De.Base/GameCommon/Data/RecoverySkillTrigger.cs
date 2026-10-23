using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class RecoverySkillTrigger : SkillTrigger
	{
		public EHitEffectType[] ConditionHitEffectType = Array.Empty<EHitEffectType>();
	}
}
