using UnityEngine;

public static class PhysicsUtility
{
	public static bool IsRayCastHitBetween(Vector3 from, Vector3 to, int layerMask)
	{
		return false;
	}

	public static bool Raycast(Vector3 from, Vector3 to, int layerMask, out RaycastHit hit)
	{
		hit = default;
		return false;
	}

	public static bool Raycast(Vector3 position, string tag, out GameObject go)
	{
		go = null;
		return false;
	}

	public static bool Raycast(Vector3 position, out GameObject go, int layerMask = -5)
	{
		go = null;
		return false;
	}
}
