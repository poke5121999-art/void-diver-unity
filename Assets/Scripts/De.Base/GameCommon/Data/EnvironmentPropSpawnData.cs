using System;
using UnityEngine;

namespace GameCommon.Data
{
	[Serializable]
	public class EnvironmentPropSpawnData : ZoneObjectSpawnData
	{
		public Vector3 Forward = Vector3.back;

		public string Prefab;

		public override bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return true;
		}
	}
}
