using System.Linq;
public class SkillExecutorSpawnGroupComponent : SpawnGroupComponent
{
	public SkillExecutorSpawnGroupData ToData()
	{
		return new SkillExecutorSpawnGroupData
		{
			SpawnAll = SpawnAll,
			SpawnCount = SpawnCount,
			SpawnDatas = base.gameObject.GetComponentsInChildren<SkillExecutorSpawnComponent>().Select((SkillExecutorSpawnComponent x) => x.ToData()).ToArray()
		};
	}
}
