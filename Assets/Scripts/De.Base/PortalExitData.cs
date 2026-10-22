using System;
using UnityEngine;

[Serializable]
public class PortalExitData
{
	public int Id;

	public Vector3 Position;

	public PortalExitData Clone(Vector3 positionOffset = default(Vector3))
	{
		return null;
	}

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		return false;
	}
}
