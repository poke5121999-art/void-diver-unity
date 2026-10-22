using UnityEngine;

public class CollisionTriggerSpawnComponent : MonoBehaviour
{
	public int TriggerId;

	public float CoolTime;

	public bool IsIndividual;

	public bool CanTriggered;

	public Vector3 Scale = Vector3.one;

	public CollisionTriggerSpawnData ToData()
	{
		CollisionTriggerSpawnData collisionTriggerSpawnData = new CollisionTriggerSpawnData();
		collisionTriggerSpawnData.TriggerId = TriggerId;
		collisionTriggerSpawnData.CoolTime = CoolTime;
		collisionTriggerSpawnData.IsIndividual = IsIndividual;
		collisionTriggerSpawnData.CanTriggered = CanTriggered;
		collisionTriggerSpawnData.Scale = Scale;
		collisionTriggerSpawnData.SpawnPosition = base.transform.position;
		return collisionTriggerSpawnData;
	}
}
