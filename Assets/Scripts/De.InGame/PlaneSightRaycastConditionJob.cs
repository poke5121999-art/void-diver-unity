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
		if (PlaneSightCore.Instance == null)
		{
			return;
		}
		float longDistance = 0f;
		float shortDistance = 0f;
		int num = CharacterIndex * RayNum + index;
		int num2 = (index + 1) % RayNum;
		PlaneSightCore.GetRaycastAngleAndDistance(in index, in TryCount, in TryMaxCount, in ViewAngle, in ViewDirectionAngle, in longDistance, in shortDistance, in DeltaFrontAngle, in DeltaBackAngle, in DivisionRatio, in SightRoundness, out var outAngle, out var outDistance, out var isFront);
		if (TryCount == 1)
		{
			float dist = RayDistances[index];
			RaycastHit hit = RaycastHits[index];
			float dist2 = RayDistances[num2];
			RaycastHit hit2 = RaycastHits[num2];
			RaycastActives[index] = PlaneSightCore.Instance.GetIsDiffHitPoints(in dist, in hit, in dist2, in hit2, in DiffNormalDotThreshold, in DiffDistanceThreshold);
		}
		if (isFront && TryCount > 1 && !RaycastActives[index])
		{
			return;
		}
		float radians = outAngle * ((float)System.Math.PI / 180f);
		float sin = Mathf.Sin(radians);
		float cos = Mathf.Cos(radians);
		Color color = isFront ? Color.green : Color.red;
		if (TryCount > 1)
		{
			color = isFront ? Color.magenta : Color.black;
		}
		float x;
		float z;
		if (RaycastHits[index].distance > Mathf.Epsilon)
		{
			if (IsDrawOnGizmo)
			{
				Debug.DrawLine(PivotPos, RaycastHits[index].point, color);
				Debug.DrawLine(RaycastHits[index].point, RaycastHits[index].point + Vector3.up, color);
			}
			x = RaycastHits[index].point.x;
			z = RaycastHits[index].point.z;
		}
		else
		{
			float distance = RayDistances[index];
			if (IsDrawOnGizmo)
			{
				Vector3 end = new Vector3(PivotPos.x + distance * sin, PivotPos.y + distance * 0f, PivotPos.z + distance * cos);
				Debug.DrawLine(PivotPos, end, color);
				Debug.DrawLine(end, end + Vector3.up, color);
			}
			x = PivotPos.x + distance * sin;
			z = PivotPos.z + distance * cos;
		}
		x = (x - PlaneCameraBoundCore.CameraBoundsMinPos.x) / (PlaneCameraBoundCore.CameraBoundsMaxPos.x - PlaneCameraBoundCore.CameraBoundsMinPos.x);
		z = (z - PlaneCameraBoundCore.CameraBoundsMinPos.z) / (PlaneCameraBoundCore.CameraBoundsMaxPos.z - PlaneCameraBoundCore.CameraBoundsMinPos.z);
		int tryCount = TryCount;
		PlaneSightCore.RayStruct value = RayStructs[num];
		switch (tryCount)
		{
		case 1:
			value.HitPoint.x = x;
			value.HitPoint.y = z;
			break;
		case 2:
			value.HitPoint.z = x;
			value.HitPoint.w = z;
			break;
		case 3:
			value.HitPoint1.x = x;
			value.HitPoint1.y = z;
			break;
		case 4:
			value.HitPoint1.z = x;
			value.HitPoint1.w = z;
			break;
		case 5:
			value.HitPoint2.x = x;
			value.HitPoint2.y = z;
			break;
		case 6:
			value.HitPoint2.z = x;
			value.HitPoint2.w = z;
			break;
		}
		value.HitPointCount = tryCount;
		RayStructs[num] = value;
	}

	public void OnDestroy()
	{
		if (RaycastHits.IsCreated)
		{
			RaycastHits.Dispose();
		}
		if (RayDistances.IsCreated)
		{
			RayDistances.Dispose();
		}
		if (RaycastActives.IsCreated)
		{
			RaycastActives.Dispose();
		}
		if (RayStructs.IsCreated)
		{
			RayStructs.Dispose();
		}
	}
}
