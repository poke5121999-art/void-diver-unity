using System;
using GameCommon.Enum;
using UnityEngine;

[Serializable]
public class MonsterSpawnData
{
	public bool UseMonsterSpawnTable;

	public int MonsterId;

	public int MonsterSpawnGroupId;

	public Vector3 SpawnPosition;

	public Vector3 Forward;

	public bool IsRespawn;

	public float SpawnableTime;

	public ESpawnType SpawnType;

	public float LifeTime;

	public bool DieOnSummonerDeath;

	public Vector3 WaveStartPosition;

	public bool UseFixedAimPos;

	public Vector3 FixedAimPos;

	public bool IsPatrolMonster;

	public Vector3[] PatrolWanderPositions;

	[NonSerialized]
	public int MonsterSpawnId;

	[NonSerialized]
	public ulong SummonerNetworkObjectId;

	[NonSerialized]
	public ulong TargetNetworkObjectId;

	public MonsterSpawnData Clone(Vector3 positionOffset = default(Vector3))
	{
		return null;
	}
}
