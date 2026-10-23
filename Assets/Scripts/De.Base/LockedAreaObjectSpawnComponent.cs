using UnityEngine;

public class LockedAreaObjectSpawnComponent : MonoBehaviour
{
	public int AreaId;

	public string PrefabName;

	public LockedAreaObjectSpawnData ToData()
	{
		return new LockedAreaObjectSpawnData
		{
			AreaId = AreaId,
			PrefabName = PrefabName,
			SpawnPosition = base.transform.position
		};
	}
}
