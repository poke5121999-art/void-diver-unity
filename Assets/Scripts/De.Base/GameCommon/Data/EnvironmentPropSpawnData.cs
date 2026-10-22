using System;
using UnityEngine;

namespace GameCommon.Data
{
	[Serializable]
	public class EnvironmentPropSpawnData : ZoneObjectSpawnData
	{
		public Vector3 Forward;

		public string Prefab;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
