using System.Linq;

public class ExitSpawnGroupComponent : SpawnGroupComponent
{
	public ExitSpawnGroupData ToData()
	{
		ExitSpawnGroupData result = new ExitSpawnGroupData();
		result.SpawnAll = SpawnAll;
		result.SpawnCount = SpawnCount;
		result.SpawnDatas = base.gameObject.GetComponentsInChildren<ExitSpawnComponent>().Select((x) => x.ToData()).ToArray();
		return result;
	}
}
