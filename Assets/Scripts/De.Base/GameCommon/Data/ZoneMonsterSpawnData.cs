using System;
using UnityEngine;

namespace GameCommon.Data
{
	[Serializable]
	public class ZoneMonsterSpawnData : ZoneObjectSpawnData
	{
		public Vector3 Forward;

		public bool UseSpawnGroup;

		public int MonsterSpawnGroupId;

		public int MonsterId;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
