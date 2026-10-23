using System;
using UnityEngine;

[Serializable]
public class TrapSpawnData
{
	public int TrapId;

	public Vector3 SpawnPosition;

	public Vector3 Forward;

	public TrapSpawnData Clone(Vector3 positionOffset = default(Vector3))
	{
		return new TrapSpawnData
		{
			TrapId = TrapId,
			SpawnPosition = positionOffset + SpawnPosition,
			Forward = Forward
		};
	}
}
