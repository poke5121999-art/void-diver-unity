using System;
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
		if (!(PlaneSightCore.Instance == null) && (TryCount == 1 || RaycastActives[index]))
		{
			PlaneSightCore.GetRaycastAngleAndDistance(in index, in TryCount, in TryMaxCount, in ViewAngle, in ViewDirectionAngle, in LongDist, in BackDist, in DeltaFrontAngle, in DeltaBackAngle, in DivisionRatio, in SightRoundness, out var outAngle, out var outDistance, out var _);
			float radians = outAngle * ((float)Math.PI / 180f);
			Vector3 direction = new Vector3(Mathf.Sin(radians), 0f, Mathf.Cos(radians));
			RayDistances[index] = outDistance;
			QueryParameters queryParameters = new QueryParameters(LayerMask, false, QueryTriggerInteraction.UseGlobal, false);
			RaycastCommands[index] = new RaycastCommand(PivotPos, direction, queryParameters, outDistance);
		}
	}

	public void OnDestroy()
	{
		if (RaycastCommands.IsCreated)
		{
			RaycastCommands.Dispose();
		}
		if (RayDistances.IsCreated)
		{
			RayDistances.Dispose();
		}
		if (RaycastActives.IsCreated)
		{
			RaycastActives.Dispose();
		}
	}
}
