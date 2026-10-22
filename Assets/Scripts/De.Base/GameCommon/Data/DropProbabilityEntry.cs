using System;

namespace GameCommon.Data
{
	[Serializable]
	public class DropProbabilityEntry
	{
		public int ItemId;

		public long Count;

		public int Weight;
	}
}
