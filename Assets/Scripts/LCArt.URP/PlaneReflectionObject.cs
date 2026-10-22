using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(0)]
[ExecuteAlways]
public class PlaneReflectionObject : MonoBehaviour
{
	[SerializeField]
	private Renderer _renderer;

	private Vector3 _xzScaling;

	private List<Bounds> _overrideBoundsList;

	public bool HasOverrideBounds => false;

	private void Awake()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
	}

	public Bounds GetBounds()
	{
		return default;
	}

	public List<Bounds> GetOverrideBoundsList()
	{
		return null;
	}

	public void SetOverrideBounds(List<Bounds> boundsList)
	{
	}

	public void ClearOverrideBounds()
	{
	}

	private void OnDrawGizmosSelected()
	{
	}

	[ContextMenu("Cashing Components")]
	private void CacheComponents()
	{
	}

	public void SetMeshRenderer(MeshRenderer meshRenderer)
	{
	}
}
