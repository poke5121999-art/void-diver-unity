using UnityEngine;

public class WaveExecutorSpawnComponent : MonoBehaviour
{
	public int WaveExecutorId;

	public WaveExecutorSpawnData ToData()
	{
		return new WaveExecutorSpawnData
		{
			WaveExecutorId = WaveExecutorId,
			SpawnPosition = base.transform.position
		};
	}
}
