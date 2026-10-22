using System;
using UnityEngine;

[Serializable]
public class TextMarkerSpawnData
{
	public string TextKey;

	public Vector3 SpawnPosition;

	public TextMarkerSpawnData Clone()
	{
		return null;
	}

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		return false;
	}
}
