using System;
using GameCommon.Enum;
using UnityEngine;

[Serializable]
public class IndicatorInfo
{
	public EIndicatorVfxType IndicatorVfxType;

	public float CircleRadius;

	public Vector2 SquareSize;

	public float ConeRadius;

	public float ConeAngle;

	public Vector3 SpawnOffset;

	public float ShowDuration;

	public float FillDuration;

	public bool IsIndependent;

	public bool IsCancelOnOwnerDie;

	public int HitBoxId;

	public float HitBoxSpawnDelay;

	public bool SpawnHitBoxAtOwnerPosition;

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		return false;
	}
}
