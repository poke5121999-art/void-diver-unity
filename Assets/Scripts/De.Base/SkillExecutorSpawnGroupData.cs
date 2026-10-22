using System;

[Serializable]
public class SkillExecutorSpawnGroupData
{
	public bool SpawnAll;

	public float SpawnCount;

	public SkillExecutorSpawnData[] SpawnDatas;

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		return false;
	}
}
