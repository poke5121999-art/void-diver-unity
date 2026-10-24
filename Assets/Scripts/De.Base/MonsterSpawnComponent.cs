using UnityEngine;

public class MonsterSpawnComponent : MonoBehaviour
{
	public bool UseMonsterSpawnTable = true;

	public int MonsterId;

	public int MonsterSpawnGroupId;

	public bool UseFixedAimPos;

	public MonsterSpawnData ToData()
	{
		return new MonsterSpawnData
		{
			UseMonsterSpawnTable = UseMonsterSpawnTable,
			MonsterId = MonsterId,
			MonsterSpawnGroupId = MonsterSpawnGroupId,
			SpawnPosition = base.transform.position,
			Forward = base.transform.forward,
			UseFixedAimPos = UseFixedAimPos,
			FixedAimPos = base.transform.position + base.transform.forward
		};
	}
}
