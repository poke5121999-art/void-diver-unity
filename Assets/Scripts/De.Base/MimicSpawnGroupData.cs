using System;

[Serializable]
public class MimicSpawnGroupData
{
	public bool SpawnAll;

	public float SpawnCount;

	public MimicSpawnData[] SpawnDatas;

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		return false;
	}
}
