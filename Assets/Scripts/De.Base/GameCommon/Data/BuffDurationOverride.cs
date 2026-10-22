using System;

namespace GameCommon.Data
{
	[Serializable]
	public class BuffDurationOverride
	{
		public float MinDuration;

		public float MaxDuration;

		public BuffDurationStatData[] Stats;
	}
}
