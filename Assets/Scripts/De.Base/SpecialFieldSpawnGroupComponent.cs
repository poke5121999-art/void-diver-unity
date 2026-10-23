using System.Linq;
public class SpecialFieldSpawnGroupComponent : SpawnGroupComponent
{
	public SpecialFieldSpawnGroupData ToData()
	{
		return new SpecialFieldSpawnGroupData
		{
			SpawnAll = SpawnAll,
			SpawnCount = SpawnCount,
			SpawnDatas = base.gameObject.GetComponentsInChildren<SpecialFieldSpawnComponent>().Select((SpecialFieldSpawnComponent x) => x.ToData()).ToArray()
		};
	}
}
