using System;
using UnityEngine;

[Serializable]
public class GuestSpawnData
{
	public int Id = -1;

	public Vector3 SpawnPosition;

	public GuestSpawnData Clone()
	{
		return new GuestSpawnData
		{
			Id = Id,
			SpawnPosition = SpawnPosition
		};
	}
}
