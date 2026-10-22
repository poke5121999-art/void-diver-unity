using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public struct PlaneSightRaycastConditionJob : IJobParallelFor
{
	public int TryCount;

	public int TryMaxCount;

	public int RayNum;

	public int CharacterIndex;

	public float DiffDistanceThreshold;

	public float DiffNormalDotThreshold;

	public bool IsDrawOnGizmo;

	public Vector3 PivotPos;

	public float ViewAngle;

	public float ViewDirectionAngle;

	public float DeltaFrontAngle;

	public float DeltaBackAngle;

	public float DivisionRatio;

	public float SightRoundness;

	[ReadOnly]
	public NativeArray<RaycastHit> RaycastHits;

	[ReadOnly]
	public NativeArray<float> RayDistances;

	[NativeDisableParallelForRestriction]
	public NativeArray<PlaneSightCore.RayStruct> RayStructs;

	[NativeDisableParallelForRestriction]
	public NativeArray<bool> RaycastActives;

	public void Execute(int index)
	{
	}

	public void OnDestroy()
	{
	}
}
