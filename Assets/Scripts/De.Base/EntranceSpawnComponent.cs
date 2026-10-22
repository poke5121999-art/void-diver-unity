using UnityEngine;

public class EntranceSpawnComponent : MonoBehaviour
{
	public int EntranceId;

	public EntranceSpawnData ToData()
	{
		EntranceSpawnData result = new EntranceSpawnData();
		result.EntranceId = EntranceId;
		result.SpawnPosition = base.transform.position;
		result.Forward = base.transform.forward;
		return result;
	}
}
