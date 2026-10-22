using System;
using UnityEngine;

[Serializable]
public class SpecialFieldSpawnData
{
	public int SpecialFieldId;

	public Vector3 SpawnPosition;

	public Vector3 Scale;

	public SpecialFieldSpawnData Clone(Vector3 positionOffset = default(Vector3))
	{
		return null;
	}

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		return false;
	}
}
