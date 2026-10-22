using System;
using UnityEngine;

[Serializable]
public class DropRewardSpawnData
{
	public int DropRewardGroupId;

	public Vector3 SpawnPosition;

	public DropRewardSpawnData Clone(Vector3 positionOffset = default(Vector3))
	{
		DropRewardSpawnData dropRewardSpawnData = new DropRewardSpawnData();
		dropRewardSpawnData.DropRewardGroupId = DropRewardGroupId;
		dropRewardSpawnData.SpawnPosition = SpawnPosition + positionOffset;
		return dropRewardSpawnData;
	}
}
