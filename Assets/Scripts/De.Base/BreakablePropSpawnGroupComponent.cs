using System.Linq;

public class BreakablePropSpawnGroupComponent : SpawnGroupComponent
{
	public BreakablePropSpawnGroupData ToData()
	{
		BreakablePropSpawnGroupData result = new BreakablePropSpawnGroupData();
		result.SpawnAll = SpawnAll;
		result.SpawnCount = SpawnCount;
		result.SpawnDatas = base.gameObject.GetComponentsInChildren<BreakablePropSpawnComponent>().Select((x) => x.ToData()).ToArray();
		return result;
	}
}
