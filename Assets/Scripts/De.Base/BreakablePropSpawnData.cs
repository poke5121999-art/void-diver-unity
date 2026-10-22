using System;
using UnityEngine;

[Serializable]
public class BreakablePropSpawnData
{
	public int BreakablePropId;

	public Vector3 SpawnPosition;

	public Vector3 Forward;

	public BreakablePropSpawnData Clone(Vector3 positionOffset = default(Vector3))
	{
		BreakablePropSpawnData breakablePropSpawnData = new BreakablePropSpawnData();
		breakablePropSpawnData.BreakablePropId = BreakablePropId;
		breakablePropSpawnData.SpawnPosition = positionOffset + SpawnPosition;
		breakablePropSpawnData.Forward = Forward;
		return breakablePropSpawnData;
	}
}
