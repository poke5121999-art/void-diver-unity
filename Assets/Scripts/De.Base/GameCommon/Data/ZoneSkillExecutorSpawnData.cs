using System;

namespace GameCommon.Data
{
	[Serializable]
	public class ZoneSkillExecutorSpawnData : ZoneObjectSpawnData
	{
		public int SkillExecutorId;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
