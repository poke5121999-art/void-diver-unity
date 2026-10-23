using System.Linq;
public class PortalSpawnGroupComponent : SpawnGroupComponent
{
	public PortalSpawnGroupData ToData()
	{
		return new PortalSpawnGroupData
		{
			SpawnAll = SpawnAll,
			SpawnCount = SpawnCount,
			SpawnDatas = base.gameObject.GetComponentsInChildren<PortalSpawnComponent>().Select((PortalSpawnComponent x) => x.ToData()).ToArray()
		};
	}
}
