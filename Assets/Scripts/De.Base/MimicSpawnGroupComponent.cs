using System.Linq;
public class MimicSpawnGroupComponent : SpawnGroupComponent
{
	public MimicSpawnGroupData ToData()
	{
		return new MimicSpawnGroupData
		{
			SpawnAll = SpawnAll,
			SpawnCount = SpawnCount,
			SpawnDatas = base.gameObject.GetComponentsInChildren<MimicSpawnComponent>().Select((MimicSpawnComponent x) => x.ToData()).ToArray()
		};
	}
}
