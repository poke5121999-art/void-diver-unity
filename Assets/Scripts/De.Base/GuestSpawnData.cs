using System;
using UnityEngine;

[Serializable]
public class GuestSpawnData
{
	public int Id;

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
