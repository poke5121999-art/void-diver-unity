using System;

[Serializable]
public class PortalSpawnGroupData
{
	public bool SpawnAll;

	public float SpawnCount;

	public PortalSpawnData[] SpawnDatas;

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		return false;
	}
}
