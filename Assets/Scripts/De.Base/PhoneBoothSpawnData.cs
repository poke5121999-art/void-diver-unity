using System;
using UnityEngine;

[Serializable]
public class PhoneBoothSpawnData
{
	public Vector3 SpawnPosition;

	// @0x1801FAE30: new PhoneBoothSpawnData, copies SpawnPosition.
	public PhoneBoothSpawnData Clone()
	{
		return new PhoneBoothSpawnData
		{
			SpawnPosition = SpawnPosition
		};
	}

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		return true;
	}
}
