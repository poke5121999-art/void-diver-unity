using System;
using UnityEngine;

namespace GameCommon.Data
{
	[Serializable]
	public class DropProbabilityGroup
	{
		[Range(0f, 100f)]
		public float PickPercent;

		public int PickCount;

		public bool IgnoreDifficulty;

		public DropProbabilityEntry[] Entries;
	}
}
