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
		EntranceSpawnData result = new EntranceSpawnData();
		result.EntranceId = EntranceId;
		result.SpawnPosition = positionOffset + SpawnPosition;
		result.Forward = Forward;
		return result;
	}
}
