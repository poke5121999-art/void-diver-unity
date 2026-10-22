using System;

[Serializable]
public class SpecialFieldSpawnGroupData
{
	public bool SpawnAll;

	public float SpawnCount;

	public SpecialFieldSpawnData[] SpawnDatas;

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		return false;
	}
}
