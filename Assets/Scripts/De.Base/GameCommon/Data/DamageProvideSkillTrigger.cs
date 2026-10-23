using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class DamageProvideSkillTrigger : SkillTrigger
	{
		public int[] ConditionSkillIds = Array.Empty<int>();

		public bool ConditionIsOnlyCritical;

		public bool ConditionIsOnlyFirstHit;

		public EHpDamageReasonType ConditionReasonTypes;

		public DamageProvideSkillTrigger()
		{
			Text = "설명 없음";
		}
	}
}
