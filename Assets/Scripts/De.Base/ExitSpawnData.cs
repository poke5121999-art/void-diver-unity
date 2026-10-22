using System;
using UnityEngine;

[Serializable]
public class ExitSpawnData
{
	public int ExitId;

	public Vector3 SpawnPosition;

	public ExitSpawnData Clone(Vector3 positionOffset = default(Vector3))
	{
		ExitSpawnData result = new ExitSpawnData();
		result.ExitId = ExitId;
		result.SpawnPosition = positionOffset + SpawnPosition;
		return result;
	}
}
