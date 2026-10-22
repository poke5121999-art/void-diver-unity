using System;

[Serializable]
public class MonsterSpawnGroupData
{
	public bool SpawnAll;

	public float SpawnCount;

	public MonsterSpawnData[] SpawnDatas;

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		return false;
	}
}
