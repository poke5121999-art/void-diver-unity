using System;
using UnityEngine;

[Serializable]
public class ZonePointData
{
	public EZoneFlags ZoneFlags;

	public Vector3 Position;

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		return false;
	}
}
