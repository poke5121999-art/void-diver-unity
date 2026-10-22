using System;
using UnityEngine;

[Serializable]
public class NpcSpawnData
{
	public int NpcId;

	public Vector3 SpawnPosition;

	public NpcSpawnData Clone()
	{
		return null;
	}

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		return false;
	}
}
