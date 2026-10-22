using System;

[Serializable]
public class RewardBoxSpawnGroupData
{
	public bool SpawnAll;

	public float SpawnCount;

	public RewardBoxSpawnData[] SpawnDatas;

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		return false;
	}
}
