using System.Linq;
public class TrapSpawnGroupComponent : SpawnGroupComponent
{
	public TrapSpawnGroupData ToData()
	{
		return new TrapSpawnGroupData
		{
			SpawnAll = SpawnAll,
			SpawnCount = SpawnCount,
			SpawnDatas = base.gameObject.GetComponentsInChildren<TrapSpawnComponent>().Select((TrapSpawnComponent x) => x.ToData()).ToArray()
		};
	}
}
