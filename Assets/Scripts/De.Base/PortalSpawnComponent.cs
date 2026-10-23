using UnityEngine;

public class PortalSpawnComponent : MonoBehaviour
{
	public string PrefabName;

	public int PortalExitId;

	public PortalSpawnData ToData()
	{
		return new PortalSpawnData
		{
			PrefabName = PrefabName,
			PortalExitId = PortalExitId,
			SpawnPosition = base.transform.position
		};
	}
}
