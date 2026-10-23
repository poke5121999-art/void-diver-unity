using UnityEngine;

public class MimicSpawnComponent : MonoBehaviour
{
	public int MimicSpawnId;

	public MimicSpawnData ToData()
	{
		return new MimicSpawnData
		{
			MimicSpawnId = MimicSpawnId,
			SpawnPosition = base.transform.position,
			Forward = base.transform.forward
		};
	}
}
