using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshTrailClone : MonoBehaviour
{
	private Vector3 _targetPosition;

	private Quaternion _targetRotation;

	private Vector3 _targetLocalScale;

	private MeshFilter _meshFilter;

	private Mesh _runtimeMesh;

	private static readonly List<Vector3> _sharedVertices = new List<Vector3>();

	private static readonly List<Vector3> _sharedNormals = new List<Vector3>();

	private static readonly List<Vector4> _sharedTangents = new List<Vector4>();

	private static readonly List<Vector2> _sharedUV = new List<Vector2>();

	private static readonly List<int> _sharedTriangles = new List<int>();

	private float _progress = 1f;

	private float _duration = 0.5f;

	private Color _startColor = Color.white;

	private Color _endColor = new Color(0f, 0f, 0f, 0f);

	private Color[] _cachedColors;

	private void Awake()
	{
		_runtimeMesh = new Mesh();
		_runtimeMesh.MarkDynamic();
		_meshFilter = GetComponent<MeshFilter>();
		_meshFilter.sharedMesh = _runtimeMesh;
	}

	private void LateUpdate()
	{
		KeepWorldTransform();
		UpdateMesh();
	}

	private void KeepWorldTransform()
	{
		base.gameObject.transform.position = _targetPosition;
		base.gameObject.transform.rotation = _targetRotation;
		Transform parent = base.transform.parent;
		if (parent != null)
		{
			Vector3 lossyScale = parent.lossyScale;
			base.transform.localScale = new Vector3(_targetLocalScale.x / lossyScale.x, _targetLocalScale.y / lossyScale.y, _targetLocalScale.z / lossyScale.z);
		}
		else
		{
			base.transform.localScale = _targetLocalScale;
		}
	}

	private bool IsValidRuntimeMesh()
	{
		if (!_runtimeMesh)
		{
			return false;
		}
		return _runtimeMesh.vertexCount > 0;
	}

	private bool IsValidVertexColorCaches(int vertexCount)
	{
		if (_cachedColors == null)
		{
			return false;
		}
		return _cachedColors.Length == vertexCount;
	}

	private void UpdateMesh()
	{
		if (_progress > 1f - Mathf.Epsilon)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (IsValidRuntimeMesh())
		{
			int vertexCount = _runtimeMesh.vertexCount;
			if (!IsValidVertexColorCaches(vertexCount))
			{
				_cachedColors = new Color[vertexCount];
			}
			float t = Mathf.Clamp01(_progress);
			float a = Mathf.Clamp01(1f - _progress * _progress);
			for (int i = 0; i < vertexCount; i++)
			{
				_cachedColors[i] = new Color((_endColor.r - _startColor.r) * t + _startColor.r, (_endColor.g - _startColor.g) * t + _startColor.g, (_endColor.b - _startColor.b) * t + _startColor.b, a);
			}
			_runtimeMesh.colors = _cachedColors;
		}
		_progress += Time.deltaTime / Mathf.Max(Mathf.Epsilon, _duration);
	}

	public void SetCloneMesh(Mesh sourceMesh)
	{
		if (sourceMesh == null || _runtimeMesh == null)
		{
			return;
		}
		sourceMesh.GetVertices(_sharedVertices);
		sourceMesh.GetNormals(_sharedNormals);
		sourceMesh.GetTangents(_sharedTangents);
		sourceMesh.GetUVs(0, _sharedUV);
		_sharedTriangles.Clear();
		for (int i = 0; i < sourceMesh.subMeshCount; i++)
		{
			_sharedTriangles.AddRange(sourceMesh.GetTriangles(i));
		}
		_runtimeMesh.Clear();
		_runtimeMesh.SetVertices(_sharedVertices);
		if (_sharedNormals.Count == _sharedVertices.Count)
		{
			_runtimeMesh.SetNormals(_sharedNormals);
		}
		else
		{
			_runtimeMesh.RecalculateNormals();
		}
		if (_sharedTangents.Count == _sharedVertices.Count)
		{
			_runtimeMesh.SetTangents(_sharedTangents);
		}
		if (_sharedUV.Count == _sharedVertices.Count)
		{
			_runtimeMesh.SetUVs(0, _sharedUV);
		}
		_runtimeMesh.SetTriangles(_sharedTriangles, 0);
	}

	public void ActiveClone(float period, Color startColor, Color endColor, Vector3 position, Quaternion rotation, Vector3 localScale)
	{
		base.gameObject.SetActive(true);
		_startColor = startColor;
		_progress = 0f;
		_duration = period;
		_endColor = endColor;
		_targetPosition = position;
		_targetRotation = rotation;
		_targetLocalScale = localScale;
	}

	public void DeactiveClone()
	{
		base.gameObject.SetActive(false);
	}
}
