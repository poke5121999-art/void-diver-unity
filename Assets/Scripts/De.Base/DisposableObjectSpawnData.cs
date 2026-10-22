using System;
using UnityEngine;

[Serializable]
public class DisposableObjectSpawnData
{
	public int DisposableObjectId;

	public Vector3 SpawnPosition;

	public Vector3 Forward;

	public DisposableObjectSpawnData Clone(Vector3 positionOffset = default(Vector3))
	{
		return null;
	}
}
