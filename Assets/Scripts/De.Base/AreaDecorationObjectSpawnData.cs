using System;
using UnityEngine;

[Serializable]
public class AreaDecorationObjectSpawnData
{
	[Tooltip("TAreaDecoration.GroupId")]
	public int GroupId;

	public Vector3 SpawnPosition;

	[Tooltip("Prefab per TAreaDecoration.Index. PrefabNames[Index] is used.")]
	public string[] PrefabNames;

	public bool IsFlipped;

	public AreaDecorationObjectSpawnData Clone(Vector3 positionOffset = default(Vector3))
	{
		AreaDecorationObjectSpawnData areaDecorationObjectSpawnData = new AreaDecorationObjectSpawnData();
		areaDecorationObjectSpawnData.GroupId = GroupId;
		areaDecorationObjectSpawnData.SpawnPosition = positionOffset + SpawnPosition;
		areaDecorationObjectSpawnData.PrefabNames = PrefabNames;
		areaDecorationObjectSpawnData.IsFlipped = IsFlipped;
		return areaDecorationObjectSpawnData;
	}

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		return true;
	}
}
