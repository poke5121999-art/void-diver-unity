using UnityEngine;

public class ExitSpawnComponent : MonoBehaviour
{
	public int ExitId;

	public ExitSpawnData ToData()
	{
		ExitSpawnData result = new ExitSpawnData();
		result.ExitId = ExitId;
		result.SpawnPosition = base.transform.position;
		return result;
	}
}
