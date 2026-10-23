using Unity.AI.Navigation;
using UnityEngine;

public static class RuntimeFlipUtility
{
	private const float _flipRotationY = -90f;

	public static void ApplyFlip(GameObject obj)
	{
		if (obj == null)
		{
			return;
		}
		Transform transform = obj.transform;
		Transform meshChild = transform.Find("mesh");
		Transform colliderChild = transform.Find("collider");
		Quaternion flipDelta = Quaternion.Euler(0f, _flipRotationY, 0f);
		if (meshChild != null)
		{
			ApplyMeshFlip(meshChild, flipDelta);
		}
		if (colliderChild != null)
		{
			ApplyColliderFlip(colliderChild, flipDelta);
		}
	}

	private static void ApplyMeshFlip(Transform meshChild, Quaternion flipDelta)
	{
		meshChild.localRotation = meshChild.localRotation * flipDelta;
		Vector3 localScale = meshChild.localScale;
		meshChild.localScale = new Vector3(0f - Mathf.Abs(localScale.x), Mathf.Abs(localScale.y), Mathf.Abs(localScale.z));
	}

	private static void ApplyColliderFlip(Transform colliderChild, Quaternion flipDelta)
	{
		colliderChild.localRotation = colliderChild.localRotation * flipDelta;
		BoxCollider[] componentsInChildren = colliderChild.GetComponentsInChildren<BoxCollider>(true);
		foreach (BoxCollider boxCollider in componentsInChildren)
		{
			Vector3 center = boxCollider.center;
			boxCollider.center = new Vector3(0f - center.x, center.y, center.z);
			Vector3 size = boxCollider.size;
			boxCollider.size = new Vector3(Mathf.Abs(size.x), Mathf.Abs(size.y), Mathf.Abs(size.z));
		}
		NavMeshModifierVolume[] volumes = colliderChild.GetComponentsInChildren<NavMeshModifierVolume>(true);
		foreach (NavMeshModifierVolume volume in volumes)
		{
			Vector3 center2 = volume.center;
			volume.center = new Vector3(0f - center2.x, center2.y, center2.z);
			Vector3 size2 = volume.size;
			volume.size = new Vector3(Mathf.Abs(size2.x), Mathf.Abs(size2.y), Mathf.Abs(size2.z));
		}
	}
}
