using System;
using UnityEngine;

[Serializable]
public class WaveExecutorSpawnData
{
	public int WaveExecutorId;

	public Vector3 SpawnPosition;

	public WaveExecutorSpawnData Clone(Vector3 positionOffset = default(Vector3))
	{
		return null;
	}
}
