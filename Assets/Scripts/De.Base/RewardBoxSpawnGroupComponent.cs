using System.Linq;
public class RewardBoxSpawnGroupComponent : SpawnGroupComponent
{
	public RewardBoxSpawnGroupData ToData()
	{
		return new RewardBoxSpawnGroupData
		{
			SpawnAll = SpawnAll,
			SpawnCount = SpawnCount,
			SpawnDatas = base.gameObject.GetComponentsInChildren<RewardBoxSpawnComponent>().Select((RewardBoxSpawnComponent x) => x.ToData()).ToArray()
		};
	}
}
