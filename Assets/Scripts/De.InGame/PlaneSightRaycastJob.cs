using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public struct PlaneSightRaycastJob : IJobParallelFor
{
	public int TryCount;

	public int TryMaxCount;

	public float LongDist;

	public float BackDist;

	public float ViewAngle;

	public float ViewDirectionAngle;

	public float DeltaFrontAngle;

	public float DeltaBackAngle;

	public float DivisionRatio;

	public float SightRoundness;

	public Vector3 PivotPos;

	public LayerMask LayerMask;

	public NativeArray<RaycastCommand> RaycastCommands;

	public NativeArray<float> RayDistances;

	public NativeArray<bool> RaycastActives;

	public void Execute(int index)
	{
	}

	public void OnDestroy()
	{
	}
}
