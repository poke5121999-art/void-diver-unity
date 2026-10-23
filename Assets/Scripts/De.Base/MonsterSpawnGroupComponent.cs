using System.Linq;
public class MonsterSpawnGroupComponent : SpawnGroupComponent
{
	public MonsterSpawnGroupData ToData()
	{
		return new MonsterSpawnGroupData
		{
			SpawnAll = SpawnAll,
			SpawnCount = SpawnCount,
			SpawnDatas = base.gameObject.GetComponentsInChildren<MonsterSpawnComponent>().Select((MonsterSpawnComponent x) => x.ToData()).ToArray()
		};
	}
}
