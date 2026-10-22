using System.Linq;

public class DropRewardSpawnGroupComponent : SpawnGroupComponent
{
	public DropRewardSpawnGroupData ToData()
	{
		DropRewardSpawnGroupData result = new DropRewardSpawnGroupData();
		result.SpawnAll = SpawnAll;
		result.SpawnCount = SpawnCount;
		result.SpawnDatas = base.gameObject.GetComponentsInChildren<DropRewardSpawnComponent>().Select((x) => x.ToData()).ToArray();
		return result;
	}
}
