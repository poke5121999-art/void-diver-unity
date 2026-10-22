using System;
using GameCommon.Enum;

namespace GameCommon.Data
{
	[Serializable]
	public class BuffDurationStatData
	{
		public EStatType StatType;

		public EStatOwnerType StatOwner;

		public float BasePercent;
	}
}
