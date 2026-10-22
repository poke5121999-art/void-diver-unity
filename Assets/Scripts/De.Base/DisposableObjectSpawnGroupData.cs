using System;

[Serializable]
public class DisposableObjectSpawnGroupData
{
	public bool SpawnAll;

	public float SpawnCount;

	public DisposableObjectSpawnData[] SpawnDatas;

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		return false;
	}
}
