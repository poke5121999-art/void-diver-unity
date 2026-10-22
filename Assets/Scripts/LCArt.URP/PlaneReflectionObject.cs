using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(0)]
[ExecuteAlways]
public class PlaneReflectionObject : MonoBehaviour
{
	[SerializeField]
	private Renderer _renderer;

	private Vector3 _xzScaling = new Vector3(1f, 0f, 1f);

	private List<Bounds> _overrideBoundsList;

	public bool HasOverrideBounds => _overrideBoundsList != null && _overrideBoundsList.Count > 0;

	private void Awake()
	{
		if (_renderer == null)
		{
			_renderer = GetComponent<Renderer>();
		}
	}

	private void OnEnable()
	{
		if (!PlaneReflectionManager.Instance)
		{
			if (PlaneReflectionManager.WaitingPlaneReflectionObjects.Contains(this))
			{
				return;
			}
			PlaneReflectionManager.WaitingPlaneReflectionObjects.Add(this);
		}
		else
		{
			PlaneReflectionManager.Instance.AddPlaneReflectionObjectToList(this);
		}
	}

	private void OnDisable()
	{
		if (PlaneReflectionManager.Instance != null)
		{
			PlaneReflectionManager.Instance.RemovePlaneReflectionObjectFromList(this);
		}
	}

	private void OnDestroy()
	{
		if (PlaneReflectionManager.Instance != null)
		{
			PlaneReflectionManager.Instance.RemovePlaneReflectionObjectFromList(this);
		}
	}

	public Bounds GetBounds()
	{
		Bounds bounds = new Bounds(Vector3.zero, Vector3.one);
		if (_renderer != null && PlaneReflectionManager.Instance != null)
		{
			bounds = _renderer.bounds;
			Vector3 extents = bounds.extents;
			float bump = PlaneReflectionManager.Instance.BoundBumpSize;
			extents += _xzScaling * bump * 0.5f;
			Vector3 size = extents * 2f;
			float minSize = PlaneReflectionManager.Instance.BoundMinSize;
			size.x = Mathf.Max(size.x, _xzScaling.x * minSize);
			size.y = Mathf.Max(size.y, _xzScaling.y * minSize);
			size.z = Mathf.Max(size.z, _xzScaling.z * minSize);
			bounds.extents = size * 0.5f;
		}
		return bounds;
	}

	public List<Bounds> GetOverrideBoundsList()
	{
		return _overrideBoundsList;
	}

	public void SetOverrideBounds(List<Bounds> boundsList)
	{
		_overrideBoundsList = boundsList;
	}

	public void ClearOverrideBounds()
	{
		_overrideBoundsList = null;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(0f, 1f, 1f, 0.3f);
		if (_overrideBoundsList == null || _overrideBoundsList.Count < 1)
		{
			Bounds bounds = GetBounds();
			Gizmos.DrawCube(bounds.center, bounds.size);
			Gizmos.DrawWireCube(bounds.center, bounds.size);
			return;
		}
		foreach (Bounds item in _overrideBoundsList)
		{
			Gizmos.DrawCube(item.center, item.size);
			Gizmos.DrawWireCube(item.center, item.size);
		}
	}

	[ContextMenu("Cashing Components")]
	private void CacheComponents()
	{
		_renderer = GetComponent<Renderer>();
	}

	public void SetMeshRenderer(MeshRenderer meshRenderer)
	{
		_renderer = meshRenderer;
	}
}
