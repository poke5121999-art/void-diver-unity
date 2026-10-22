using System.Linq;

public class EntranceSpawnGroupComponent : SpawnGroupComponent
{
	public EntranceSpawnGroupData ToData()
	{
		EntranceSpawnGroupData result = new EntranceSpawnGroupData();
		result.SpawnAll = SpawnAll;
		result.SpawnCount = SpawnCount;
		result.SpawnDatas = base.gameObject.GetComponentsInChildren<EntranceSpawnComponent>().Select((x) => x.ToData()).ToArray();
		return result;
	}
}
