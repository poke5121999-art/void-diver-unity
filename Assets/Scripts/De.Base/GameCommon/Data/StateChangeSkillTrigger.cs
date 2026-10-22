using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class StateChangeSkillTrigger : SkillTrigger
	{
		public EActionState[] States;

		public bool Exclude;
	}
}
