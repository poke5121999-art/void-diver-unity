using UnityEngine;

public class DisposableObjectSpawnComponent : MonoBehaviour
{
	public int DisposableObjectId;

	public DisposableObjectSpawnData ToData()
	{
		DisposableObjectSpawnData disposableObjectSpawnData = new DisposableObjectSpawnData();
		disposableObjectSpawnData.DisposableObjectId = DisposableObjectId;
		disposableObjectSpawnData.SpawnPosition = base.transform.position;
		disposableObjectSpawnData.Forward = base.transform.forward;
		return disposableObjectSpawnData;
	}
}
