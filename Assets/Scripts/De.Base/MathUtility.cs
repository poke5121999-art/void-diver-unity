using System;
using UnityEngine;

public static class MathUtility
{
	public static int FloatSecToMs(float second)
	{
		// Trap 59 (docs/DECODING.md): float32 product before the integer conversion, as IL2CPP; Mono needs the (float).
		return (int)(float)(second * 1000f);
	}

	public static bool RollChance(int chance, int max = 10000)
	{
		return UnityEngine.Random.Range(1, max + 1) <= chance;
	}

	public static Vector3 TopDownToIsometric(Vector2 xy)
	{
		if (xy == Vector2.zero)
		{
			return Vector3.zero;
		}
		return Quaternion.Euler(0f, 315f, 0f) * new Vector3(xy.x, 0f, xy.y);
	}

	public static Vector2 IsometricToTopDown(Vector3 xyz)
	{
		Vector3 rotated = Quaternion.Euler(0f, 45f, 0f) * xyz;
		return new Vector2(rotated.x, rotated.z);
	}

	public static Quaternion LookRotationOrIdentity(Vector3 direction)
	{
		if (direction != Vector3.zero)
		{
			return Quaternion.LookRotation(direction, Vector3.up);
		}
		return Quaternion.identity;
	}

	public static Vector3 SnapToCardinalDirection(Vector3 direction)
	{
		if (direction == Vector3.zero)
		{
			return direction;
		}
		direction = direction.normalized;
		Vector3[] cardinals = new Vector3[4] { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
		return cardinals.MaxBy(v => Vector3.Dot(v, direction));
	}

	public static float DivideInt(int numerator, int denominator)
	{
		if (denominator == 0)
		{
			Debug.LogError("Denominator cannot be zero.");
			return 0f;
		}
		return (float)numerator / (float)denominator;
	}

	public static float DivideIntClamp(int numerator, int denominator)
	{
		if (denominator == 0)
		{
			Debug.LogError("Denominator cannot be zero.");
			return 0f;
		}
		return Mathf.Clamp01((float)numerator / (float)denominator);
	}

	public static float SqrMagnitudeXZ(Vector3 a, Vector3 b)
	{
		float dx = a.x - b.x;
		float dz = a.z - b.z;
		return dx * dx + dz * dz;
	}

	public static Vector3 RotateAround(Vector3 point, Vector3 pivot, float angle)
	{
		float cos = Mathf.Cos(angle * ((float)Math.PI / 180f));
		float sin = Mathf.Sin(angle * ((float)Math.PI / 180f));
		float dx = point.x - pivot.x;
		float dz = point.z - pivot.z;
		return new Vector3(dx * cos - dz * sin + pivot.x, point.y, dx * sin + dz * cos + pivot.z);
	}

	public static Vector3 GetClosestPointOnSegment(Vector3 a, Vector3 b, Vector3 p)
	{
		Vector3 ab = b - a;
		float sqr = ab.x * ab.x + ab.y * ab.y + ab.z * ab.z;
		if (sqr == 0f)
		{
			return a;
		}
		float t = (ab.x * (p.x - a.x) + ab.y * (p.y - a.y) + (p.z - a.z) * ab.z) / sqr;
		t = Mathf.Clamp01(t);
		return new Vector3(a.x + ab.x * t, a.y + t * ab.y, a.z + t * ab.z);
	}
}
