using System;

namespace GameCommon.Data
{
	[Serializable]
	public class ZoneExitSpawnData : ZoneObjectSpawnData
	{
		public int ExitId;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
