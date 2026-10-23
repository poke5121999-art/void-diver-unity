using System.Linq;
using UnityEngine;

public static class PhysicsUtility
{
	public static bool IsRayCastHitBetween(Vector3 from, Vector3 to, int layerMask)
	{
		Vector3 direction = (to - from).normalized;
		float distance = Vector3.Distance(from, to);
		return Physics.Raycast(from, direction, out RaycastHit _, distance, layerMask);
	}

	public static bool Raycast(Vector3 from, Vector3 to, int layerMask, out RaycastHit hit)
	{
		Vector3 direction = (to - from).normalized;
		float distance = Vector3.Distance(from, to);
		return Physics.Raycast(from, direction, out hit, distance, layerMask);
	}

	public static bool Raycast(Vector3 position, string tag, out GameObject go)
	{
		go = null;
		var ray = new Ray(position + new Vector3(0f, 5f, 0f), Vector3.down.normalized);
		RaycastHit[] hits = Physics.RaycastAll(ray, 5f);
		if (hits != null && hits.Length != 0)
		{
			RaycastHit hit = hits.FirstOrDefault(h => h.collider.CompareTag(tag));
			if (hit.collider != null)
			{
				go = hit.collider.gameObject;
				return true;
			}
		}
		return false;
	}

	public static bool Raycast(Vector3 position, out GameObject go, int layerMask = -5)
	{
		go = null;
		var ray = new Ray(position + new Vector3(0f, 5f, 0f), Vector3.down.normalized);
		if (Physics.Raycast(ray, out RaycastHit hit, 10f, layerMask))
		{
			go = hit.collider.gameObject;
			return true;
		}
		return false;
	}
}
