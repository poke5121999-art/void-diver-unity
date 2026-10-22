using System;

[Serializable]
public class BreakablePropSpawnGroupData
{
	public bool SpawnAll;

	public float SpawnCount;

	public BreakablePropSpawnData[] SpawnDatas;

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		return false;
	}
}
