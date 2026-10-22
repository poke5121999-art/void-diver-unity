using System;

namespace GameCommon.Data
{
	[Serializable]
	public class ZoneNpcSpawnData : ZoneObjectSpawnData
	{
		public int NpcId;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
