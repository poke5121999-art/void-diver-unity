using System;
using UnityEngine.Serialization;

namespace GameCommon.Data
{
	[Serializable]
	public class DamageSkillTrigger : SkillTrigger
	{
		public bool ConditionIsOnlyCritical;

		public bool ConditionIsOnlyParryable;

		[FormerlySerializedAs("ConditionIsNotBackAttack")]
		public bool ConditionIsIgnoreBackAttack;

		public DamageSkillTrigger()
		{
			Text = "설명 없음";
		}
	}
}
