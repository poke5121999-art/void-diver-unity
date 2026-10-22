using System;
using UnityEngine;

[Serializable]
public class LockedAreaObjectSpawnData
{
	public int AreaId;

	public string PrefabName;

	public Vector3 SpawnPosition;

	public LockedAreaObjectSpawnData Clone(Vector3 positionOffset = default(Vector3))
	{
		return null;
	}

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		return false;
	}
}
