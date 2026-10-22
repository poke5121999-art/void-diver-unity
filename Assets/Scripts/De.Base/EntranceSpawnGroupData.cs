using System;

[Serializable]
public class EntranceSpawnGroupData
{
	public bool SpawnAll;

	public float SpawnCount;

	public EntranceSpawnData[] SpawnDatas;

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		return false;
	}
}
