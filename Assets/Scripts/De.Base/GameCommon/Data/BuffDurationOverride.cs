using System;

namespace GameCommon.Data
{
	[Serializable]
	public class BuffDurationOverride
	{
		public float MinDuration = -1f;

		public float MaxDuration = -1f;

		public BuffDurationStatData[] Stats;
	}
}
