using System;

namespace GameCommon.Data
{
	[Serializable]
	public class ZoneWaveExecutorSpawnData : ZoneObjectSpawnData
	{
		public int WaveExecutorId;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
