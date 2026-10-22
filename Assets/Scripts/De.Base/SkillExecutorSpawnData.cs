using System;
using UnityEngine;

[Serializable]
public class SkillExecutorSpawnData
{
	public int SkillExecutorId;

	public Vector3 SpawnPosition;

	public SkillExecutorSpawnData Clone(Vector3 positionOffset = default(Vector3))
	{
		return null;
	}
}
