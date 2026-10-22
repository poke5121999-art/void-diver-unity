using UnityEngine;

public class DropRewardSpawnComponent : MonoBehaviour
{
	public int DropRewardGroupId;

	public DropRewardSpawnData ToData()
	{
		DropRewardSpawnData dropRewardSpawnData = new DropRewardSpawnData();
		dropRewardSpawnData.DropRewardGroupId = DropRewardGroupId;
		dropRewardSpawnData.SpawnPosition = base.transform.position;
		return dropRewardSpawnData;
	}
}
