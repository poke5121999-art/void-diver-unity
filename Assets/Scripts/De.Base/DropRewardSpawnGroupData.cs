using System;

[Serializable]
public class DropRewardSpawnGroupData
{
	public bool SpawnAll;

	public float SpawnCount;

	public DropRewardSpawnData[] SpawnDatas;

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		return false;
	}
}
