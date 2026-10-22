using System;

namespace GameCommon.Data
{
	[Serializable]
	public class MoveFinishSkillTrigger : SkillTrigger
	{
		public bool TriggerOnlyWhenBlocked;
	}
}
