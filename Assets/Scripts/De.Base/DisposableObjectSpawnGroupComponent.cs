using System.Linq;

public class DisposableObjectSpawnGroupComponent : SpawnGroupComponent
{
	public DisposableObjectSpawnGroupData ToData()
	{
		DisposableObjectSpawnGroupData result = new DisposableObjectSpawnGroupData();
		result.SpawnAll = SpawnAll;
		result.SpawnCount = SpawnCount;
		result.SpawnDatas = base.gameObject.GetComponentsInChildren<DisposableObjectSpawnComponent>().Select((x) => x.ToData()).ToArray();
		return result;
	}
}
