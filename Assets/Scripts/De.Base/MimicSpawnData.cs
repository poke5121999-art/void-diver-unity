using System;
using UnityEngine;

[Serializable]
public class MimicSpawnData
{
	public int MimicSpawnId;

	public Vector3 SpawnPosition;

	public Vector3 Forward;

	public MimicSpawnData Clone(Vector3 positionOffset = default(Vector3))
	{
		return null;
	}
}
