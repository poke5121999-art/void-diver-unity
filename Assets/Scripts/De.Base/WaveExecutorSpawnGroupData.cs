using System;

[Serializable]
public class WaveExecutorSpawnGroupData
{
	public bool SpawnAll;

	public float SpawnCount;

	public WaveExecutorSpawnData[] SpawnDatas;

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		return false;
	}
}
