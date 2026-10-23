using System;
using UnityEngine;

[Serializable]
public class PortalExitData
{
	public int Id;

	public Vector3 Position;

	public PortalExitData Clone(Vector3 positionOffset = default(Vector3))
	{
		return new PortalExitData
		{
			Id = Id,
			Position = positionOffset + Position
		};
	}

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		return true;
	}
}
