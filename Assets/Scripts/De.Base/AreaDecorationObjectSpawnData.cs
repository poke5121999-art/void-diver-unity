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
		return null;
	}

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		return false;
	}
}
