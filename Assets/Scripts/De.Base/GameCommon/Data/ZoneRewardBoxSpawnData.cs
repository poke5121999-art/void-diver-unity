using System;
using UnityEngine;

namespace GameCommon.Data
{
	[Serializable]
	public class ZoneRewardBoxSpawnData : ZoneObjectSpawnData
	{
		public Vector3 Forward;

		public int RewardBoxId;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
