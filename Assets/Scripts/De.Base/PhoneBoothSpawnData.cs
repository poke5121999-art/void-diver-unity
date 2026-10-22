using System;
using UnityEngine;

[Serializable]
public class PhoneBoothSpawnData
{
	public Vector3 SpawnPosition;

	public PhoneBoothSpawnData Clone()
	{
		return null;
	}

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		return false;
	}
}
