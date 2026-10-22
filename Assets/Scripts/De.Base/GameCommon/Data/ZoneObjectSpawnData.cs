using System;
using UnityEngine;

namespace GameCommon.Data
{
	[Serializable]
	public abstract class ZoneObjectSpawnData
	{
		public Vector3 Offset;

		public abstract bool Validate(out string errorMessage);
	}
}
