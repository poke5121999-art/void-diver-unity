using UnityEngine;

public class RewardBoxSpawnComponent : MonoBehaviour
{
	public bool IsReplace;

	public int RewardBoxGroupId;

	public RewardBoxSpawnData ToData()
	{
		return new RewardBoxSpawnData
		{
			RewardBoxGroupId = RewardBoxGroupId,
			SpawnPosition = base.transform.position,
			Forward = base.transform.forward,
			IsReplace = IsReplace
		};
	}
}
