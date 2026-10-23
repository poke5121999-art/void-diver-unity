using UnityEngine;

public class TrapSpawnComponent : MonoBehaviour
{
	public int TrapId;

	public TrapSpawnData ToData()
	{
		return new TrapSpawnData
		{
			TrapId = TrapId,
			SpawnPosition = base.transform.position,
			Forward = base.transform.forward
		};
	}
}
