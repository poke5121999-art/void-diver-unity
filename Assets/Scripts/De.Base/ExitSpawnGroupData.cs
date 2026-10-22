using System;

[Serializable]
public class ExitSpawnGroupData
{
	public bool SpawnAll;

	public float SpawnCount;

	public ExitSpawnData[] SpawnDatas;

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		return false;
	}
}
