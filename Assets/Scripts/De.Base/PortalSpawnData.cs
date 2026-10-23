using System;
using UnityEngine;

[Serializable]
public class PortalSpawnData
{
	public string PrefabName;

	public int PortalExitId;

	public Vector3 SpawnPosition;

	public PortalSpawnData Clone(Vector3 positionOffset = default(Vector3))
	{
		return new PortalSpawnData
		{
			PrefabName = PrefabName,
			PortalExitId = PortalExitId,
			SpawnPosition = positionOffset + SpawnPosition
		};
	}
}
