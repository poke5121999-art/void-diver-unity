using System;
using UnityEngine;

[Serializable]
public class EntranceSpawnData
{
	public int EntranceId;

	public Vector3 SpawnPosition;

	public Vector3 Forward;

	public EntranceSpawnData Clone(Vector3 positionOffset = default(Vector3))
	{
		return null;
	}
}
