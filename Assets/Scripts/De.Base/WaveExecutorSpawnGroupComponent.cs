using System.Linq;
public class WaveExecutorSpawnGroupComponent : SpawnGroupComponent
{
	public WaveExecutorSpawnGroupData ToData()
	{
		return new WaveExecutorSpawnGroupData
		{
			SpawnAll = SpawnAll,
			SpawnCount = SpawnCount,
			SpawnDatas = base.gameObject.GetComponentsInChildren<WaveExecutorSpawnComponent>().Select((WaveExecutorSpawnComponent x) => x.ToData()).ToArray()
		};
	}
}
