using UnityEngine;

public class NpcSpawnComponent : MonoBehaviour
{
	public int NpcId;

	public NpcSpawnData ToData()
	{
		return new NpcSpawnData
		{
			NpcId = NpcId,
			SpawnPosition = base.transform.position
		};
	}
}
