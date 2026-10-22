using System;
using UnityEngine;

[Serializable]
public class CollisionTriggerSpawnData
{
	public int TriggerId;

	public float CoolTime;

	public bool IsIndividual;

	public bool CanTriggered;

	public Vector3 Scale;

	public Vector3 SpawnPosition;

	public CollisionTriggerSpawnData Clone(Vector3 positionOffset = default(Vector3))
	{
		return null;
	}

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		return false;
	}
}
