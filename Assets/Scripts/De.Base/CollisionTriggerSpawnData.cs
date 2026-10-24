using System;
using UnityEngine;

[Serializable]
public class CollisionTriggerSpawnData
{
	public int TriggerId;

	public float CoolTime;

	public bool IsIndividual;

	public bool CanTriggered;

	public Vector3 Scale = Vector3.one;

	public Vector3 SpawnPosition;

	public CollisionTriggerSpawnData Clone(Vector3 positionOffset = default(Vector3))
	{
		CollisionTriggerSpawnData collisionTriggerSpawnData = new CollisionTriggerSpawnData();
		collisionTriggerSpawnData.TriggerId = TriggerId;
		collisionTriggerSpawnData.CoolTime = CoolTime;
		collisionTriggerSpawnData.IsIndividual = IsIndividual;
		collisionTriggerSpawnData.CanTriggered = CanTriggered;
		collisionTriggerSpawnData.Scale = Scale;
		collisionTriggerSpawnData.SpawnPosition = SpawnPosition + positionOffset;
		return collisionTriggerSpawnData;
	}

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		// the whole body is `errorMessage = null; new StringBuilder(); return true` (machine code checked @0x1801f30f0)
		new System.Text.StringBuilder();
		return true;
	}
}
