using UnityEngine;

public class BreakablePropSpawnComponent : MonoBehaviour
{
	public int BreakablePropId;

	public BreakablePropSpawnData ToData()
	{
		BreakablePropSpawnData breakablePropSpawnData = new BreakablePropSpawnData();
		breakablePropSpawnData.BreakablePropId = BreakablePropId;
		breakablePropSpawnData.SpawnPosition = base.transform.position;
		breakablePropSpawnData.Forward = base.transform.forward;
		return breakablePropSpawnData;
	}
}
