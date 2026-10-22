using System;

[Serializable]
public class TrapSpawnGroupData
{
	public bool SpawnAll;

	public float SpawnCount;

	public TrapSpawnData[] SpawnDatas;

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		return false;
	}
}
