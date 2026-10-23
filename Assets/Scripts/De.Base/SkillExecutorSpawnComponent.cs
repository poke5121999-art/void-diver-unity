using UnityEngine;

public class SkillExecutorSpawnComponent : MonoBehaviour
{
	public int SkillExecutorId;

	public SkillExecutorSpawnData ToData()
	{
		return new SkillExecutorSpawnData
		{
			SkillExecutorId = SkillExecutorId,
			SpawnPosition = base.transform.position
		};
	}
}
