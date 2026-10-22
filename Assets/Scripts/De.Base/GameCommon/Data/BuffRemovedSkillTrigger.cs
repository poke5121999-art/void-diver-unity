using System;

namespace GameCommon.Data
{
	[Serializable]
	public class BuffRemovedSkillTrigger : SkillTrigger
	{
		public int BuffId;

		public bool OnlyExpired;
	}
}
