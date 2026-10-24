using System;
using System.Collections.Generic;
using GameCommon.Enum;
using UnityEngine;

[Serializable]
public class RewardBoxSpawnData
{
	public int RewardBoxGroupId;

	public Vector3 SpawnPosition;

	public Vector3 Forward;

	public bool IsReplace;

	[NonSerialized]
	public int RewardBoxId;

	[NonSerialized]
	public IEnumerable<GoodsSlot> RespawnItemSlots;

	[NonSerialized]
	public ELockState LockState;

	[NonSerialized]
	public int SlotCount = -1;

	public RewardBoxSpawnData Clone(Vector3 positionOffset = default(Vector3))
	{
		return new RewardBoxSpawnData
		{
			RewardBoxGroupId = RewardBoxGroupId,
			SpawnPosition = positionOffset + SpawnPosition,
			Forward = Forward,
			IsReplace = IsReplace
		};
	}
}
